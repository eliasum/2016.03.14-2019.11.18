/*
This sample program demonstrates how to be informed about the removal of a device.

  Attention:
  If you build this sample in debug mode and run it using a GigE camera device, pylon will set the heartbeat
  timeout to 5 minutes. This is done to allow debugging and single stepping of the code without
  the camera thinking we're hung because we don't send any heartbeats.
  This also means that it would normally take 5 minutes for the application to notice that a GigE device
  has been disconnected.
  
  To work around this, the heartbeat timeout will be set to 1000 ms before we remove a device and wait to
  notice the removal.
    
*/

using System;
using System.Collections.Generic;
using PylonC.NET;

namespace SurpriseRemoval
{
    class SurpriseRemoval
    {
        static int callbackCounter = 0;  /* Will be incremented by the callback function. */

        static void Main(string[] args)
        {
            PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE(); /* Handle for the pylon device. */
            try
            {
                uint numDevices;         /* Number of available devices.   */
                PYLON_DEVICECALLBACK_HANDLE hCb;  /* Required for deregistering the callback. */
                int loopCount;          /* Counter. */
#if DEBUG
                bool isGigECamera;       /* 1 if the device is a GigE device. */
#endif
                DeviceCallbackHandler callbackHandler = new DeviceCallbackHandler(); /* Handles callbacks from a device. */

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
                Pylon.DeviceOpen(hDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

                /* Print out the name of the camera we are using. */
                {
                    bool isReadable = Pylon.DeviceFeatureIsReadable(hDev, "DeviceModelName");
                    if (isReadable)
                    {
                        string name = Pylon.DeviceFeatureToString(hDev, "DeviceModelName");
                        Console.WriteLine("Using camera {0}", name);
                    }
                }

                /* Register the callback function. */
                callbackHandler.CallbackEvent += new DeviceCallbackHandler.DeviceCallback(removalCallbackFunction);
                hCb = Pylon.DeviceRegisterRemovalCallback(hDev, callbackHandler);

#if DEBUG
                /*  For GigE cameras, the application periodically sends heartbeat signals to the camera to keep the 
                    connection to the camera alive. If the camera doesn't receive heartbeat signals within the time 
                    period specified by the heartbeat timeout counter, the camera resets the connection.
                    When the application is stopped by the debugger, the application cannot create the heartbeat signals. 
                    For that reason, the pylon runtime extends the heartbeat timeout in debug mode to 5 minutes to allow
                    debugging.  For GigE cameras, we will set the heartbeat timeout to a shorter period before testing the
	            callbacks.
                    The heartbeat mechanism is also used for detection of device removal. When the pylon runtime doesn't
                    receive an acknowledge for the heartbeat signal, it is assumed that the device has been removed. A 
                    removal callback will be fired in that case. 
                    By decreasing the heartbeat timeout in debug mode for GigE cameras, the surprise removal will be noticed sooner than set by the pylon runtime. */
                {
                    /* Find out if we are using a GigE camera. */
                    PYLON_DEVICE_INFO_HANDLE hDi = Pylon.DeviceGetDeviceInfoHandle(hDev);
                    string deviceClass = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoDeviceClassKey);

                    isGigECamera = deviceClass == "BaslerGigE";
                    /* Adjust the heartbeat timeout. */
                    if (isGigECamera)
                    {
                        setHeartbeatTimeout(hDev, 1000);  /* 1000 ms */
                    }
                }
#endif

                /* Ask the user to disconnect a device. */
                loopCount = 20 * 4;
                Console.WriteLine("Please disconnect the device (timeout {0} s) ", loopCount / 4);

                /* Wait until the removal has been noticed and the callback function has been fired. */
                do
                {
                    /* Print a . every few seconds to tell the user we're waiting for the callback. */
                    if (--loopCount % 4 == 0)
                    {
                        Console.Write(".");
                    }
                    System.Threading.Thread.Sleep(250);
                }
                while (callbackCounter < 1 && loopCount >= 0);  /*  Check loopCount so we won't wait forever. */


                if (callbackCounter < 1)
                    Console.WriteLine("\nTimeout expired. Device hasn't been removed.");


                /* Clean up. */

                /* ... Deregister the removal callback. */
                Pylon.DeviceDeregisterRemovalCallback(hDev, hCb);

                /* ....Close and release the pylon device. */
                Pylon.DeviceClose(hDev);
                Pylon.DestroyDevice(hDev);

                /* Shut down the pylon runtime system. Don't call any pylon method after 
                   calling Pylon.Terminate(). */
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

        /* The function to be called when the removal of an open device is detected. */
        private static void removalCallbackFunction(PYLON_DEVICE_HANDLE hDevice)
        {
            /* Print out the name of the device. It is not possible to read the name 
            from the camera since it has been removed. Use the device's device 
            information instead. For accessing the device information, no reading from 
            the device is required. */

            /* Retrieve the device information for the removed device. */
            PYLON_DEVICE_INFO_HANDLE hDi = Pylon.DeviceGetDeviceInfoHandle(hDevice);
            string fullName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoFullNameKey);
            string friendlyName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoFriendlyNameKey);

            /* Print out the name. */
            Console.WriteLine("\nCallback function for removal of device {0} ({1}).", friendlyName, fullName);

            /* Increment the counter to indicate that the callback has been fired. */
            callbackCounter++;
        }

        /* If the device provides a heartbeat timeout, this function will set the heartbeat timeout. 
           When the device provides the parameter, the old value is returned, -1 otherwise. 
           The heartbeat timeout is a parameter provided by the transport layer.
           The transport layer parameters are exposed as a GenApi node map that
           can be retrieved from the device. 
        */
        private static long setHeartbeatTimeout(PYLON_DEVICE_HANDLE hDev, long timeout_ms)
        {
            NODEMAP_HANDLE hNodemap;   /* Handle to the node map. */
            NODE_HANDLE hNode;      /* Handle to a node, i.e., a feature. */
            long oldTimeout; /* The current timeout value. */

            /* Get the node map for the transport layer parameters. */
            hNodemap = Pylon.DeviceGetTLNodeMap(hDev);

            if (!hNodemap.IsValid)
            {
                /* The device doesn't provide a transport layer node map. Nothing to do. */
                Console.WriteLine("The device doesn't provide a transport layer node map. Cannot set heartbeat timeout.");
                return -1;
            }
            /* Get the node for the heartbeat timeout parameter. */
            hNode = GenApi.NodeMapGetNode(hNodemap, "HeartbeatTimeout");

            if (!hNode.IsValid)
            {
                /* There is no heartbeat timeout parameter. Nothing to do. */
                Console.WriteLine("There is no heartbeat timeout parameter. Cannot set heartbeat timeout.");
                return -1;
            }

            /* Get the current value. */
            oldTimeout = GenApi.IntegerGetValue(hNode);

            /* Set the new value. */
            GenApi.IntegerSetValue(hNode, timeout_ms);

            /* Return the old value. */
            return oldTimeout;
        }
    }
}
