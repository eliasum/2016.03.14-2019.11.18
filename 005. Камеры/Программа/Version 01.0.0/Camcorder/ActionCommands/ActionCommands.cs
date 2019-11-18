/* 
   This sample illustrates how to grab images 
   using a GigE Vision action command to trigger multiple cameras.
   At least 2 connected GigE cameras are required for this sample.
*/

using System;
using System.Collections.Generic;
using PylonC.NET;
using System.Threading;

namespace TriggeredGrab
{
    class TriggeredGrab
    {
        /* Limits the amount of cameras used for grabbing.
        It is important to manage the available bandwidth when grabbing with multiple 
        cameras. This applies, for instance, if two GigE cameras are connected to the 
        same network adapter via a switch. To manage the bandwidth, the GevSCPD 
        interpacket delay parameter and the GevSCFTD transmission delay parameter can 
        be set for each GigE camera device. The "Controlling Packet Transmission Timing 
        with the Interpacket and Frame Transmission Delays on Basler GigE Vision Cameras"
        Application Note (AW000649xx000) provides more information about this topic. */
        const uint MAX_NUM_DEVICES = 4;
        const uint NUM_BUFFERS = 10;        /* Number of buffers used for grabbing. */

        const uint GIGE_PACKET_SIZE = 1500; /* Size of one Ethernet packet. */
        const uint GIGE_PROTOCOL_OVERHEAD = 36;   /* Total number of bytes of protocol overhead. */

        const uint AllGroupMask = 0xffffffff;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            /* Use a random number as the device key. */
            uint DeviceKey = (uint)(new Random()).Next(int.MaxValue);
            /* In this sample all cameras belong to the same group. */
            const uint GroupKey = 0x24;
                        
            PYLON_DEVICE_HANDLE[] hDev = new PYLON_DEVICE_HANDLE[MAX_NUM_DEVICES];        /* Handles for the pylon devices. */
            for (int deviceIndex = 0; deviceIndex < MAX_NUM_DEVICES; ++deviceIndex)
            {
                hDev[deviceIndex] = new PYLON_DEVICE_HANDLE();
            }

            try
            {
                uint numDevicesEnumerated;    /* Number of the devices connected to this PC. */
                uint numDevicesToUse;         /* Number of the devices to use in this sample. */
                bool isAvail;                 /* Used for checking feature availability. */
                bool isReady;                 /* Used as an output parameter. */
                int i;                        /* Counter. */
                uint deviceIndex;             /* Index of device used in the following variables. */
                PYLON_WAITOBJECTS_HANDLE wos; /* Wait objects. */

                /* These are camera specific variables: */
                PYLON_STREAMGRABBER_HANDLE[] hGrabber = new PYLON_STREAMGRABBER_HANDLE[MAX_NUM_DEVICES]; /* Handle for the pylon stream grabber. */
                PYLON_WAITOBJECT_HANDLE[]    hWait = new PYLON_WAITOBJECT_HANDLE[MAX_NUM_DEVICES];       /* Handle used for waiting for a grab to be finished. */
                uint[]                       payloadSize = new uint[MAX_NUM_DEVICES];                    /* Size of an image frame in bytes. */
                uint[]                       nStreams = new uint[MAX_NUM_DEVICES];                       /* The number of streams provided by the device. */
                PYLON_STREAMBUFFER_HANDLE[]  hBuffer = new PYLON_STREAMBUFFER_HANDLE[MAX_NUM_DEVICES];
                PylonBuffer<Byte>[]          buffer = new PylonBuffer<Byte>[MAX_NUM_DEVICES];

#if DEBUG
                /* This is a special debug setting needed only for GigE cameras.
                See 'Building Applications with pylon' in the Programmer's Guide. */
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
#endif

                /* Before using any pylon methods, the pylon runtime must be initialized. */
                Pylon.Initialize();

                /* Enumerate all camera devices. You must call 
                PylonEnumerateDevices() before creating a device. */
                numDevicesEnumerated = Pylon.EnumerateDevices();

                if (numDevicesEnumerated == 0)
                {
                    Pylon.Terminate();

                    Console.Error.WriteLine("No devices found!");
                    Console.Error.WriteLine("\nPress enter to exit.");
                    Console.ReadLine();
                    return;
                }

                /* Create wait objects. This must be done outside of the loop. */
                wos = Pylon.WaitObjectsCreate();

                /* Open cameras and set parameter */
                deviceIndex = 0;
                for (uint enumeratedDeviceIndex = 0; enumeratedDeviceIndex < numDevicesEnumerated; ++enumeratedDeviceIndex)
                {
                    /* only open GigE devices */
                    PYLON_DEVICE_INFO_HANDLE hDI = Pylon.GetDeviceInfoHandle(enumeratedDeviceIndex);
                    if (Pylon.DeviceInfoGetPropertyValueByName(hDI, Pylon.cPylonDeviceInfoDeviceClassKey) != "BaslerGigE")
                    {
                        continue;
                    }

                    /* Get handles for the devices. */
                    hDev[deviceIndex] = Pylon.CreateDeviceByIndex((uint)enumeratedDeviceIndex);

                    /* Before using the device, it must be opened. Open it for configuring
                    parameters and for grabbing images. */
                    Pylon.DeviceOpen(hDev[deviceIndex], Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

                    /* Print out the name of the camera we are using. */
                    Console.WriteLine("Using camera '{0}'", Pylon.DeviceInfoGetPropertyValueByName(hDI, Pylon.cPylonDeviceInfoModelNameKey));

                    isAvail = Pylon.DeviceFeatureIsReadable(hDev[deviceIndex], "ActionControl");
                    if (!isAvail)
                    {
                        throw new Exception("Device doesn't support the Action Command");
                    }
                    
                    /* Configure the first action */
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "ActionSelector", 1);
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "ActionDeviceKey", DeviceKey);
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "ActionGroupKey", GroupKey);
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "ActionGroupMask", AllGroupMask);

                    /* Set the pixel format to Mono8, where gray values will be output as 8 bit values for each pixel. */
                    /* ... Check first to see if the device supports the Mono8 format. */
                    //isAvail = Pylon.DeviceFeatureIsAvailable(hDev[deviceIndex], "EnumEntry_PixelFormat_Mono8");
                    isAvail = Pylon.DeviceFeatureIsAvailable(hDev[deviceIndex], "EnumEntry_PixelFormat_BayerRG12");
                    if (!isAvail)
                    {
                        /* Feature is not available. */
                        throw new Exception("Device doesn't support the BayerRG12 pixel format.");
                    }

                    /* ... Set the pixel format to Mono8. */
                    Pylon.DeviceFeatureFromString(hDev[deviceIndex], "PixelFormat", "BayerRG12");  // цветное изображение


                    /* Disable acquisition start trigger if available */
                    isAvail = Pylon.DeviceFeatureIsAvailable(hDev[deviceIndex], "EnumEntry_TriggerSelector_AcquisitionStart");
                    if (isAvail)
                    {
                        Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerSelector", "AcquisitionStart");
                        Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerMode", "Off");
                    }
                    /* Disable line1 trigger if available */
                    isAvail = Pylon.DeviceFeatureIsAvailable(hDev[deviceIndex], "EnumEntry_TriggerSelector_Line1");
                    if (isAvail)
                    {
                        Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerSelector", "Line1");
                        Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerMode", "Off");
                    }
                    
                    /* Enable frame start trigger with first action */
                    Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerSelector", "FrameStart");
                    Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerMode", "On");
                    Pylon.DeviceFeatureFromString(hDev[deviceIndex], "TriggerSource", "Action1");
                    Pylon.DeviceFeatureFromString(hDev[deviceIndex], "ExposureAuto", "Continuous");  // не моргать

                    /* For GigE cameras, we recommend increasing the packet size for better 
                        performance. When the network adapter supports jumbo frames, set the packet 
                        size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
                        to 1500.
            
                        We also set the Inter-Packet and the Frame Transmission delay
                        so the switch can line up packets better.
                    */
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "GevSCPSPacketSize", GIGE_PACKET_SIZE);
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "GevSCPD", (GIGE_PACKET_SIZE + GIGE_PROTOCOL_OVERHEAD) * (MAX_NUM_DEVICES - 1));
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "GevSCFTD", (GIGE_PACKET_SIZE + GIGE_PROTOCOL_OVERHEAD) * deviceIndex);

                    /* one device opened */
                    ++deviceIndex;
                }

                /* remember how many devices we have actually created */
                numDevicesToUse = deviceIndex;

                /* Remember the number of devices actually created */
                numDevicesToUse = deviceIndex;

                if (numDevicesToUse == 0)
                {
                    Console.Error.WriteLine("No suitable cameras found!");
                    Pylon.Terminate();  /* Releases all pylon resources. */
                    Console.Error.WriteLine("\nPress enter to exit.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                if (numDevicesToUse < 2)
                {
                    Console.Error.WriteLine("WARNING: This sample works best with two or more GigE cameras supporting action commands.");
                }

                /* Allocate and register buffers for grab. */
                for (deviceIndex = 0; deviceIndex < numDevicesToUse; ++deviceIndex)
                {
                    /* Determine the required size for the grab buffer. */
                    payloadSize[deviceIndex] = checked((uint)Pylon.DeviceGetIntegerFeature(hDev[deviceIndex], "PayloadSize"));

                    /* Image grabbing is done using a stream grabber.  
                      A device may be able to provide different streams. A separate stream grabber must 
                      be used for each stream. In this sample, we create a stream grabber for the default 
                      stream, i.e., the first stream ( index == 0 ).
                      */

                    /* Get the number of streams supported by the device and the transport layer. */
                    nStreams[deviceIndex] = Pylon.DeviceGetNumStreamGrabberChannels(hDev[deviceIndex]);

                    if (nStreams[deviceIndex] < 1)
                    {
                        throw new Exception("The transport layer doesn't support image streams.");
                    }

                    /* Create and open a stream grabber for the first channel. */
                    hGrabber[deviceIndex] = Pylon.DeviceGetStreamGrabber(hDev[deviceIndex], 0);

                    Pylon.StreamGrabberOpen(hGrabber[deviceIndex]);


                    /* Get a handle for the stream grabber's wait object. The wait object
                       allows waiting for buffers to be filled with grabbed data. */
                    hWait[deviceIndex] = Pylon.StreamGrabberGetWaitObject(hGrabber[deviceIndex]);

                    /* Add the stream grabber's wait object to our wait objects.
                       This is needed to be able to wait until all cameras have 
                       grabbed an image in our grab loop below. */
                    Pylon.WaitObjectsAdd(wos, hWait[deviceIndex]);

                    /* We must tell the stream grabber the number and size of the buffers 
                        we are using. */
                    /* .. We will not use more than NUM_BUFFERS for grabbing. */
                    Pylon.StreamGrabberSetMaxNumBuffer(hGrabber[deviceIndex], NUM_BUFFERS);

                    /* .. We will not use buffers bigger than payloadSize bytes. */
                    Pylon.StreamGrabberSetMaxBufferSize(hGrabber[deviceIndex], payloadSize[deviceIndex]);

                    /*  Allocate the resources required for grabbing. After this, critical parameters 
                        that impact the payload size must not be changed until FinishGrab() is called. */
                    Pylon.StreamGrabberPrepareGrab(hGrabber[deviceIndex]);

                    /* Before using the buffers for grabbing, they must be registered at
                       the stream grabber. For each registered buffer, a buffer handle
                       is returned. After registering, these handles are used instead of the
                       buffer objects pointers. The buffer objects are held in a dictionary,
                       that provides access to the buffer using a handle as key.
                     */
                    buffer[deviceIndex] = new PylonBuffer<byte>(payloadSize[deviceIndex], true);
                    hBuffer[deviceIndex] = Pylon.StreamGrabberRegisterBuffer(hGrabber[deviceIndex], ref buffer[deviceIndex]);

                    /* Feed the buffers into the stream grabber's input queue. */
                    Pylon.StreamGrabberQueueBuffer(hGrabber[deviceIndex], hBuffer[deviceIndex], 0);
                }
                /* The stream grabber is now prepared. Start the image acquisition.
                   The camera won't send any image data, since it's configured to wait
                   for the action to trigger the acquisition */
                for (deviceIndex = 0; deviceIndex < numDevicesToUse; ++deviceIndex)
                {
                    Pylon.DeviceExecuteCommandFeature(hDev[deviceIndex], "AcquisitionStart");
                }


                /*  ======================================================================
                    Issue an ActionCommand and retrieve the images.
                    ====================================================================== */
                Console.WriteLine("*** Issuing action command ***");

                /* Trigger the camera using an action command (w/o waiting for results).
                   If your setup support PTP, you could use a scheduled action command. 
                   Pylon.GigEIssueScheduledActionCommand(subnet, DefaultDeviceKey, DefaultGroupKey, 1, triggertime, 0)
                 */
                string subnet = Pylon.DeviceInfoGetPropertyValueByName( Pylon.DeviceGetDeviceInfoHandle(hDev[0]), "SubnetAddress" );

                Pylon.GigEIssueActionCommand(DeviceKey, GroupKey, 1, subnet);

                /* Grab one image from each camera. */
                for (i = 0; i < numDevicesToUse; ++i)
                {
                    uint woIndex; /* this corresponds to the index in hDev and hGrabber */

                    /* Wait for the next buffer to be filled. Wait up to 5000 ms. */
                    isReady = Pylon.WaitObjectsWaitForAny(wos, 5000, out woIndex);

                    if (!isReady)
                    {   /* Timeout occurred */
                        /* Grab Timeout occurred. */
                        throw new Exception("Grab timeout occurred.");
                    }

                    PylonGrabResult_t grabResult;
                    
                    /* Since the wait operation was successful, the result of at least one grab 
                       operation is available. Retrieve it. */
                    isReady = Pylon.StreamGrabberRetrieveResult(hGrabber[woIndex], out grabResult);

                    if (!isReady)
                    {
                        /* Oops. No grab result available? We should never have reached this point. 
                           Since the wait operation above returned without a timeout, a grab result 
                           should be available. */
                        throw new Exception("Failed to retrieve a grab result.");
                    }

                    /* Check to see if the image was grabbed successfully. */
                    if (grabResult.Status == EPylonGrabStatus.Grabbed)
                    {
                        /* Success. Perform image processing. Since we passed more than one buffer
                           to the stream grabber, the remaining buffers are filled in the background while
                           we do the image processing. The processed buffer won't be touched by
                           the stream grabber until we pass it back to the stream grabber. */

                        /* We only use one buffer per camera */
                        System.Diagnostics.Debug.Assert(grabResult.hBuffer == hBuffer[woIndex]);

                        byte pixel = buffer[woIndex].Array[0];

                        /* Perform processing. */
                        Console.WriteLine("Grabbed a frame from camera {0}.", woIndex);

                        /* Display image */
                        if (woIndex < 32)
                        {
                            Pylon.ImageWindowDisplayImage<Byte>(woIndex, buffer[woIndex], grabResult);
                        }
                    }
                    else if (grabResult.Status == EPylonGrabStatus.Failed)
                    {
                        /* If a buffer has been incompletely grabbed the network bandwidth is possibly insufficient for transferring
                           multiple images simultaneously. See note above MAX_NUM_DEVICES. */
                        Console.Error.WriteLine("Frame from camera {0} wasn't grabbed successfully.  Error code = {1}",
                            woIndex, grabResult.ErrorCode);
                    }
                }

                /* Stop the image acquisition on the cameras. */
                for (deviceIndex = 0; deviceIndex < numDevicesToUse; ++deviceIndex)
                {
                    /*  Stop the camera. */
                    Pylon.DeviceExecuteCommandFeature(hDev[deviceIndex], "AcquisitionStop");
                }

                // Remove all wait objects from WaitObjects.
                Pylon.WaitObjectsRemoveAll(wos);
                Pylon.WaitObjectsDestroy(wos);

                for (deviceIndex = 0; deviceIndex < numDevicesToUse; ++deviceIndex)
                {
                    /* We must issue a cancel call to ensure that all pending buffers are put into the
                       stream grabber's output queue. */
                    Pylon.StreamGrabberCancelGrab(hGrabber[deviceIndex]);

                    /* The buffers can now be retrieved from the stream grabber. */
                    do
                    {
                        PylonGrabResult_t grabResult;
                        isReady = Pylon.StreamGrabberRetrieveResult(hGrabber[deviceIndex], out grabResult);

                    } while (isReady);

                    /* When all buffers are retrieved from the stream grabber, they can be de-registered.
                       After de-registering the buffers, it is safe to free the memory. */

                    Pylon.StreamGrabberDeregisterBuffer(hGrabber[deviceIndex], hBuffer[deviceIndex]);
                    buffer[deviceIndex].Dispose();
                    buffer[deviceIndex] = null;

                    /* Release grabbing related resources. */
                    Pylon.StreamGrabberFinishGrab(hGrabber[deviceIndex]);

                    /* After calling PylonStreamGrabberFinishGrab(), parameters that impact the payload size (e.g., 
                    the AOI width and height parameters) are unlocked and can be modified again. */

                    /* Close the stream grabber. */
                    Pylon.StreamGrabberClose(hGrabber[deviceIndex]);

                    /* Close and release the pylon device. The stream grabber becomes invalid
                       after closing the pylon device. Don't call stream grabber related methods after 
                       closing or releasing the device. */
                    Pylon.DeviceClose(hDev[deviceIndex]);
                    Pylon.DestroyDevice(hDev[deviceIndex]);
                }


                Console.Error.WriteLine("\nPress enter to exit.");
                Console.ReadLine();

                /* Shut down the pylon runtime system. Don't call any pylon function after 
                   calling PylonTerminate(). */
                Pylon.Terminate();
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

                for (uint deviceIndex = 0; deviceIndex < MAX_NUM_DEVICES; ++deviceIndex)
                {
                    try
                    {
                        if (hDev[deviceIndex].IsValid)
                        {
                            /* Close and release the pylon device. */
                            if (Pylon.DeviceIsOpen(hDev[deviceIndex]))
                            {
                                Pylon.DeviceClose(hDev[deviceIndex]);
                            }
                            Pylon.DestroyDevice(hDev[deviceIndex]);
                        }
                    }
                    catch (Exception)
                    {
                        /* No further handling here.*/
                    }
                }
                Pylon.Terminate();  /* Releases all pylon resources. */

                Console.Error.WriteLine("\nPress enter to exit.");
                Console.ReadLine();

                Environment.Exit(1);
            }
        }
    }
}
