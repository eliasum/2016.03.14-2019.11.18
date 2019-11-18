/* 
Basler GigE Vision, IEEE 1394, and Camera Link cameras can send event messages. For example, when a sensor 
exposure has finished, the camera can send an end-of-exposure event to the PC. The event 
can be received by the PC before the image data for the finished exposure has been completely 
transferred. This sample illustrates the retrieving and processing of event messages.

Receiving events is very similar to grabbing images. An event grabber provides a wait object that
is signalled when an event message is available. When an event message is available, it can be 
retrieved from the event grabber. In contrast to grabbing images, memory buffers for receiving 
events need not be provided by the application. Memory buffers to store event messages are organized 
by the event grabber itself.

The specific layout of event messages depends on the event type and the camera type. The pylon API 
uses GenICam support for parsing event messages. This means that the message layout is described in the 
camera's XML description file. A GenApi node map is created from the XML camera description file. 
This node map contains node objects representing the elements of the XML file. Since the layout of event 
messages is described in the camera description file, the information carried by the event messages is 
exposed as nodes in the node map and can be accessed like "normal" camera parameters. 


You can register callback functions that are fired when a parameter has been changed. To be 
informed that a received event message contains a specific event, a callback must be registered for 
the parameter(s) associated with the event. 

These mechanisms are demonstrated with the end-of-exposure event. The event carries the following 
information: 
* ExposureEndEventFrameID: indicates the number of the image frame that has been exposed. 
* ExposureEndEventTimestamp: indicates the moment when the event was generated. 
* ExposureEndEventStreamChannelIndex: indicates the number of the image data stream used to 
transfer the exposed frame.
A callback for the ExposureEndEventFrameID will be registered as an indicator for the arrival 
of an end-of-exposure event.
*/

using System;
using System.Collections.Generic;
using PylonC.NET;

namespace Events
{
    class Events
    {
        const uint NUM_GRABS = 200;          /* Number of images to grab. */
        const uint NUM_IMAGE_BUFFERS = 5;         /* Number of buffers used for grabbing. */
        const uint NUM_EVENT_BUFFERS = 20;   /* Number of buffers used for grabbing. */

        static void Main(string[] args)
        {
            PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE(); /* Handle for the pylon device. */
            try
            {
                uint numDevices;               /* Number of available devices. */
                PYLON_STREAMGRABBER_HANDLE hStreamGrabber;        /* Handle for the pylon stream grabber. */
                PYLON_EVENTGRABBER_HANDLE hEventGrabber;          /* Handle for the event grabber used for receiving events. */
                PYLON_EVENTADAPTER_HANDLE hEventAdapter;          /* Handle for the event adapter used for dispatching events. */
                PYLON_WAITOBJECT_HANDLE hWaitStream;              /* Handle used for waiting for a grab to be finished. */
                PYLON_WAITOBJECT_HANDLE hWaitEvent;               /* Handle used for waiting for an event message. */
                PYLON_WAITOBJECTS_HANDLE hWaitObjects;            /* Container allowing waiting for multiple wait objects. */
                NODEMAP_HANDLE hNodeMap;                 /* Handle for the node map containing the 
                                                             camera parameters. */
                NODE_CALLBACK_HANDLE hCallback;       /* Used for deregistering a callback function. */
                NODE_HANDLE hNode;                    /* Handle for a camera parameter. */
                uint payloadSize;                     /* Size of an image frame in bytes. */
                Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> buffers; /* Holds handles and buffers used for grabbing. */
                PylonGrabResult_t grabResult;         /* Stores the result of a grab operation. */
                NodeCallbackHandler callbackHandler = new NodeCallbackHandler(); /* Handles incoming callbacks. */
                int nGrabs;                   /* Counts the number of buffers grabbed. */
                uint nStreams;                /* The number of streams the device provides. */
                bool isAvail;                 /* Used for checking feature availability. */
                bool isReady;                 /* Used as an output parameter. */
                int i;                        /* Counter. */
                PylonEventResult_t eventMsg = new PylonEventResult_t(); /* Event data container. */
                long sfncVersionMajor;         /* The major number of the Standard Feature Naming Convention (SFNC)
                                                  version used by the camera device. */

#if DEBUG
                /* This is a special debug setting needed only for GigE cameras.
                See 'Building Applications with pylon' in the programmer's guide */
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
#endif

                /* Before using any pylon methods, the pylon runtime must be initialized. */
                Pylon.Initialize();

                /* Enumerate all camera devices. You must call 
                PylonEnumerateDevices() before creating a device. */
                numDevices = Pylon.EnumerateDevices();

                if (0 == numDevices)
                {
                    throw new Exception("No devices found.");
                }

                /* Get a handle for the first device found.  */
                hDev = Pylon.CreateDeviceByIndex(0);

                /* Before using the device, it must be opened. Open it for configuring
                   parameters, for grabbing images, and for grabbing events. */
                Pylon.DeviceOpen(hDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream | Pylon.cPylonAccessModeEvent);

                /* Print out the name of the camera we are using. */
                {
                    bool isReadable = Pylon.DeviceFeatureIsReadable(hDev, "DeviceModelName");
                    if (isReadable)
                    {
                        string name = Pylon.DeviceFeatureToString(hDev, "DeviceModelName");
                        Console.WriteLine("Using camera {0}", name);
                    }
                }

                /* Set the pixel format to Mono8, where gray values will be output as 8 bit values for each pixel. */
                /* ... Check first to see if the device supports the Mono8 format. */
                isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_PixelFormat_Mono8");

                if (!isAvail)
                {
                    /* Feature is not available. */
                    throw new Exception("Device doesn't support the Mono8 pixel format.");
                }
                /* ... Set the pixel format to Mono8. */
                Pylon.DeviceFeatureFromString(hDev, "PixelFormat", "Mono8");

                /* Disable acquisition start trigger if available. */
                isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_AcquisitionStart");
                if (isAvail)
                {
                    Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                    Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
                }

                /* Disable frame burst start trigger if available */
                isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_FrameBurstStart");
                if (isAvail)
                {
                    Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameBurstStart");
                    Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
                }

                /* Disable frame start trigger if available. */
                isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_FrameStart");
                if (isAvail)
                {
                    Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameStart");
                    Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
                }

                /* We will use the Continuous frame mode, i.e., the camera delivers
                images continuously. */
                Pylon.DeviceFeatureFromString(hDev, "AcquisitionMode", "Continuous");

                /* For GigE cameras, we recommend increasing the packet size for better 
                   performance. If the network adapter supports jumbo frames, set the packet 
                   size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
                   to 1500. */
                /* ... Check first to see if the GigE camera packet size parameter is supported and if it is writable. */
                isAvail = Pylon.DeviceFeatureIsWritable(hDev, "GevSCPSPacketSize");

                if (isAvail)
                {
                    /* ... The device supports the packet size feature. Set a value. */
                    Pylon.DeviceSetIntegerFeature(hDev, "GevSCPSPacketSize", 1500);
                }

                /* Determine the required size of the grab buffer. */
                payloadSize = checked((uint)Pylon.DeviceGetIntegerFeature(hDev, "PayloadSize"));

                /* Determine the major number of the SFNC version used by the camera device. */
                if (Pylon.DeviceFeatureIsAvailable( hDev, "DeviceSFNCVersionMajor"))
                {
                    sfncVersionMajor = Pylon.DeviceGetIntegerFeature(hDev, "DeviceSFNCVersionMajor");
                }
                else
                {
                    /* No SFNC version information is provided by the camera device. */
                    sfncVersionMajor = 0;
                }

                /* Enable camera events. */
                /* Select the end-of-exposure event.*/
                Pylon.DeviceFeatureFromString(hDev, "EventSelector", "ExposureEnd");
                /* Enable the event. Select the enumeration entry name depending on the SFNC version used by the camera device. */
                if (sfncVersionMajor >= 2)
                    Pylon.DeviceFeatureFromString(hDev, "EventNotification", "On");
                else
                    Pylon.DeviceFeatureFromString(hDev, "EventNotification", "GenICamEvent");

                /* Image grabbing is done using a stream grabber.  
                A device may be able to provide different streams. A separate stream grabber must 
                be used for each stream. In this sample, we create a stream grabber for the default 
                stream, i.e., the first stream ( index == 0 ).
                */

                /* Get the number of streams supported by the device and the transport layer. */
                nStreams = Pylon.DeviceGetNumStreamGrabberChannels(hDev);
                if (nStreams < 1)
                {
                    throw new Exception("The transport layer doesn't support image streams");
                }

                /* Create and open a stream grabber for the first channel. */
                hStreamGrabber = Pylon.DeviceGetStreamGrabber(hDev, 0);
                Pylon.StreamGrabberOpen(hStreamGrabber);

                /* Get a handle for the stream grabber's wait object. The wait object
                allows waiting for buffers to be grabbed. */
                hWaitStream = Pylon.StreamGrabberGetWaitObject(hStreamGrabber);

                /* We must tell the stream grabber the number and size of the buffers 
                we are using. */
                /* .. We will not use more than NUM_BUFFERS for grabbing. */
                Pylon.StreamGrabberSetMaxNumBuffer(hStreamGrabber, NUM_IMAGE_BUFFERS);

                /* .. We will not use buffers bigger than payloadSize bytes. */
                Pylon.StreamGrabberSetMaxBufferSize(hStreamGrabber, payloadSize);

                /*  Allocate the resources required for grabbing. After this, critical parameters 
                that impact the payload size must not be changed until FinishGrab() is called. */
                Pylon.StreamGrabberPrepareGrab(hStreamGrabber);

                /* Before using the buffers for grabbing, they must be registered at
                   the stream grabber. For each registered buffer, a buffer handle
                   is returned. After registering, these handles are used instead of the
                   buffer object pointers. The buffer objects are held in a dictionary,
                   that provides access to the buffer using a handle as key.
                 */
                buffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>>();
                for (i = 0; i < NUM_IMAGE_BUFFERS; ++i)
                {
                    PylonBuffer<Byte> buffer = new PylonBuffer<byte>(payloadSize, true);
                    PYLON_STREAMBUFFER_HANDLE handle = Pylon.StreamGrabberRegisterBuffer(hStreamGrabber, ref buffer);
                    buffers.Add(handle, buffer);
                }

                /* Feed the buffers into the stream grabber's input queue. For each buffer, the API 
                   allows passing in an integer as additional context information. This integer
                   will be returned unchanged when the grab is finished. In our example, we use the index of the 
                   buffer as context information. */
                i = 0;
                foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in buffers)
                {
                    Pylon.StreamGrabberQueueBuffer(hStreamGrabber, pair.Key, i++);
                }

                /* The stream grabber is now prepared. As soon the camera starts to acquire images,
                the image data will be grabbed into the provided buffers.  */


                /* Create and prepare an event grabber. */
                /* ... Get a handle for the event grabber. */
                hEventGrabber = Pylon.DeviceGetEventGrabber(hDev);

                if (!hEventGrabber.IsValid)
                {
                    /* The transport layer doesn't support event grabbers. */
                    throw new Exception("No event grabber supported.");
                }

                /* ... Tell the grabber how many buffers to use. */
                Pylon.EventGrabberSetNumBuffers(hEventGrabber, NUM_EVENT_BUFFERS);

                /* ... Open the event grabber. */
                Pylon.EventGrabberOpen(hEventGrabber);  /* The event grabber is now ready
                                                   for receiving events. */

                /* Retrieve the wait object that is associated with the event grabber. The event 
                will be signaled when an event message has been received. */
                hWaitEvent = Pylon.EventGrabberGetWaitObject(hEventGrabber);

                /* For extracting the event data from an event message, an event adapter is used. */
                hEventAdapter = Pylon.DeviceCreateEventAdapter(hDev);

                if (!hEventAdapter.IsValid)
                {
                    /* The transport layer doesn't support event grabbers. */
                    throw new Exception("No event adapter supported.");
                }

                /* Register the callback function for the ExposureEndEventFrameID parameter. */
                /*.Get the node map containing all parameters. */
                hNodeMap = Pylon.DeviceGetNodeMap(hDev);

                /* Get the ExposureEndEventFrameID parameter.
                Select the parameter name depending on the SFNC version used by the camera device.
                */
                if (sfncVersionMajor >= 2)
                    hNode = GenApi.NodeMapGetNode(hNodeMap, "EventExposureEndFrameID");
                else
                    hNode = GenApi.NodeMapGetNode(hNodeMap, "ExposureEndEventFrameID");

                if (!hNode.IsValid)
                {
                    /* There is no ExposureEndEventFrameID parameter. */
                    throw new Exception("There is no ExposureEndEventFrameID or EventExposureEndFrameID parameter!");
                }
                /* ... Register the callback function. */
                callbackHandler.CallbackEvent += new NodeCallbackHandler.NodeCallback(endOfExposureCallback);
                hCallback = GenApi.NodeRegisterCallback(hNode, callbackHandler);

                /* Put the wait objects into a container. */
                /* ... Create the container. */
                hWaitObjects = Pylon.WaitObjectsCreate();

                /* ... Add the wait objects' handles. */
                Pylon.WaitObjectsAdd(hWaitObjects, hWaitEvent);
                Pylon.WaitObjectsAdd(hWaitObjects, hWaitStream);

                /* Let the camera acquire images. */
                Pylon.DeviceExecuteCommandFeature(hDev, "AcquisitionStart");

                /* Grab NUM_GRABS images. */
                nGrabs = 0;                         /* Counts the number of images grabbed. */
                while (nGrabs < NUM_GRABS)
                {
                    int bufferIndex;              /* Index of the buffer. */
                    uint waitObjectIndex;         /* Index of the wait object that is signalled.*/
                    Byte min, max;

                    /* Wait for either an image buffer grabbed or an event received. Wait up to 1000 ms. */
                    isReady = Pylon.WaitObjectsWaitForAny(hWaitObjects, 1000, out waitObjectIndex);

                    if (!isReady)
                    {
                        /* Timeout occurred. */
                        throw new Exception("Timeout. Neither grabbed an image nor received an event.");
                    }

                    if (0 == waitObjectIndex)
                    {
                        /* hWaitEvent has been signalled. At least one event message is available. Retrieve it. */
                        isReady = Pylon.EventGrabberRetrieveEvent(hEventGrabber, ref eventMsg);

                        if (!isReady)
                        {
                            /* Oops. No event message available? We should never have reached this point. 
                            Since the wait operation above returned without a timeout, an event message 
                            should be available. */
                            throw new Exception("Failed to retrieve an event.");
                        }
                        /* Check to see if the event was successfully received. */
                        if (0 == eventMsg.ErrorCode)
                        {
                            /* Successfully received an event message. */
                            /* Pass the event message to the event adapter. The event adapter will 
                            update the parameters related to events and will fire the callbacks
                            registered to event related parameters. */
                            Pylon.EventAdapterDeliverMessage(hEventAdapter, eventMsg);

                        }
                        else
                        {
                            Console.Error.WriteLine("Error when receiving an event: {1}", eventMsg.ErrorCode);
                        }
                    }
                    else if (1 == waitObjectIndex)
                    {
                        /* hWaitStream  has been signalled. The result of at least one grab 
                        operation is available. Retrieve it. */
                        isReady = Pylon.StreamGrabberRetrieveResult(hStreamGrabber, out grabResult);

                        if (!isReady)
                        {
                            /* Oops. No grab result available? We should never have reached this point. 
                            Since the wait operation above returned without a timeout, a grab result 
                            should be available. */
                            throw new Exception("Failed to retrieve a grab result.");
                        }

                        nGrabs++;

                        /* Get the buffer index from the context information. */
                        bufferIndex = (int)grabResult.Context;

                        /* Check to see if the image was grabbed successfully. */
                        if (grabResult.Status == EPylonGrabStatus.Grabbed)
                        {
                            /*  Success. Perform image processing. Since we passed more than one buffer
                            to the stream grabber, the remaining buffers are filled while
                            we do the image processing. The processed buffer won't be touched by
                            the stream grabber until we pass it back to the stream grabber. */

                            PylonBuffer<Byte> buffer;        /* Reference to the buffer attached to the grab result. */

                            /* Get the buffer from the dictionary. Since we also got the buffer index, 
                               we could alternatively use an array, e.g. buffers[bufferIndex]. */
                            if (!buffers.TryGetValue(grabResult.hBuffer, out buffer))
                            {
                                /* Oops. No buffer available? We should never have reached this point. Since all buffers are
                                   in the dictionary. */
                                throw new Exception("Failed to find the buffer associated with the handle returned in grab result.");
                            }

                            getMinMax(buffer.Array, out min, out max);
                            Console.WriteLine("Grabbed frame {0} into buffer {1}. Min. gray value = {2}, Max. gray value = {3}",
                                nGrabs, bufferIndex, min, max);
                        }
                        else if (grabResult.Status == EPylonGrabStatus.Failed)
                        {
                            Console.Error.WriteLine("Frame {0} wasn't grabbed successfully.  Error code = {1}",
                                nGrabs, grabResult.ErrorCode);
                        }

                        /* Once finished with the processing, requeue the buffer to be filled again. */
                        Pylon.StreamGrabberQueueBuffer(hStreamGrabber, grabResult.hBuffer, bufferIndex);
                    }
                }

                /* Clean up. */

                /*  ... Stop the camera. */
                Pylon.DeviceExecuteCommandFeature(hDev, "AcquisitionStop");

                /* ... Switch off the events. */
                Pylon.DeviceFeatureFromString(hDev, "EventSelector", "ExposureEnd");
                Pylon.DeviceFeatureFromString(hDev, "EventNotification", "Off");


                /* ... We must issue a cancel call to ensure that all pending buffers are put into the
                stream grabber's output queue. */
                Pylon.StreamGrabberCancelGrab(hStreamGrabber);

                /* ... The buffers can now be retrieved from the stream grabber. */
                do
                {
                    isReady = Pylon.StreamGrabberRetrieveResult(hStreamGrabber, out grabResult);

                } while (isReady);

                /* ... When all buffers are retrieved from the stream grabber, they can be deregistered.
                       After deregistering the buffers, it is safe to free the memory. */
                foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in buffers)
                {
                    Pylon.StreamGrabberDeregisterBuffer(hStreamGrabber, pair.Key);
                    pair.Value.Dispose();
                }
                buffers = null;


                /* ... Release grabbing related resources. */
                Pylon.StreamGrabberFinishGrab(hStreamGrabber);

                /* After calling PylonStreamGrabberFinishGrab(), parameters that impact the payload size (e.g., 
                the AOI width and height parameters) are unlocked and can be modified again. */

                /* ... Close the stream grabber. */
                Pylon.StreamGrabberClose(hStreamGrabber);

                /* ... Deregister the callback. */
                GenApi.NodeDeregisterCallback(hNode, hCallback);

                /* ... Close the event grabber.*/
                Pylon.EventGrabberClose(hEventGrabber);

                /* ... Release the event adapter. */
                Pylon.DeviceDestroyEventAdapter(hDev, hEventAdapter);

                /* ... Release the wait object container. */
                Pylon.WaitObjectsDestroy(hWaitObjects);

                /* ... Close and release the pylon device. The stream grabber becomes invalid
                after closing the pylon device. Don't call stream grabber related methods after 
                closing or releasing the device. */
                Pylon.DeviceClose(hDev);
                Pylon.DestroyDevice(hDev);

                /* ... Shut down the pylon runtime system. Don't call any pylon method after 
                calling PylonTerminate(). */
                Pylon.Terminate();

                Console.Error.WriteLine("\nPress enter to exit.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                /* Retrieve the error message. */
                string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();
                Console.Error.WriteLine("Exception caught:");
                Console.Error.WriteLine(e.Message);
                if (msg != "\n")
                {
                    Console.Error.WriteLine("Last error message:");
                    Console.Error.WriteLine(msg);
                }

                try
                {
                    if (hDev.IsValid)
                    {
                        /*  Disable the software trigger. */
                        Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
                        /* ... Close and release the pylon device. */
                        if (Pylon.DeviceIsOpen(hDev))
                        {
                            Pylon.DeviceClose(hDev);
                        }
                        Pylon.DestroyDevice(hDev);
                    }
                }
                catch (Exception)
                {
                    /*No further handling here.*/
                }

                Pylon.Terminate();  /* Releases all pylon resources. */

                Console.Error.WriteLine("\nPress enter to exit.");
                Console.ReadLine();

                Environment.Exit(1);
            }
        }

        /* Simple "image processing" function returning the minumum and maximum gray 
        value of an 8 bit gray value image. */
        static void getMinMax(Byte[] imageBuffer, out Byte min, out Byte max)
        {
            min = (Byte)((imageBuffer.Length > 0) ? 255 : 0); max = 0;
            foreach (Byte val in imageBuffer)
            {
                if (val > max)
                    max = val;
                if (val < min)
                    min = val;
            }
        }

        /* Callback will be fired when an event message contains an end-of-exposure event. */
        private static void endOfExposureCallback(NODE_HANDLE hNode)
        {
            try
            {
                long frame;
                frame = GenApi.IntegerGetValue(hNode);
                Console.WriteLine("Got end-of-exposure event. Frame number: {0}.", frame);
            }
            catch( Exception e)
            {
                string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();
                Console.Error.WriteLine("Exception caught:");
                Console.Error.WriteLine(e.Message);
                if (msg != "\n")
                {
                    Console.Error.WriteLine("Last error message:");
                    Console.Error.WriteLine(msg);
                }                
            }
        }
    }
}
