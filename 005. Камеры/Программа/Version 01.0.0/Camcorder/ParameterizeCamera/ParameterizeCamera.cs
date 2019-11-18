/* 
  This sample illustrates how to read and write the different camera
  parameter types.
*/

using System;
using System.Collections.Generic;
using PylonC.NET;

namespace ParameterizeCamera
{
    class ParameterizeCamera
    {
        static void Main(string[] args)
        {
            PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE(); /* Handle for the pylon device. */
            try
            {
                uint numDevices;    /* Number of devices available. */

#if DEBUG
                /* This is a special debug setting needed only for GigE cameras.
                See 'Building Applications with pylon' in the Programmer's Guide. */
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
                parameters and for grabbing images. */
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

                /* Demonstrate how to check the accessibility of a feature. */
                demonstrateAccessibilityCheck(hDev);

                /* Demonstrate how to handle integer camera parameters. */
                demonstrateIntFeature(hDev);

                /* Demonstrate how to handle floating point camera parameters. */
                demonstrateFloatFeature(hDev);

                /* Demonstrate how to handle boolean camera parameters. */
                demonstrateBooleanFeature(hDev);

                /* Each feature can be read as a string and also set as a string. */
                demonstrateFromStringToString(hDev);

                /* Demonstrate how to handle enumeration camera parameters. */
                demonstrateEnumFeature(hDev);

                /* Demonstrate how to execute actions. */
                demonstrateCommandFeature(hDev);

                /* Clean up. Close and release the pylon device. */
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

        /* This function demonstrates how to check the presence, readability, and writability
            of a feature. */
        private static void demonstrateAccessibilityCheck(PYLON_DEVICE_HANDLE hDev)
        {
            bool val;  /* Output of the check functions. */

            /* Check to see if a feature is implemented at all. */
            val = Pylon.DeviceFeatureIsImplemented(hDev, "Width");

            Console.WriteLine("The 'Width' feature {0} implemented.", val ? "is" : "isn't");
            val = Pylon.DeviceFeatureIsImplemented(hDev, "MyCustomFeature");
            Console.WriteLine("The 'MyCustomFeature' feature {0} implemented.", val ? "is" : "isn't");

            /* Although a feature is implemented by the device, it may not be available
               with the device in its current state. Check to see if the feature is currently 
               available. The PylonDeviceFeatureIsAvailable sets val to 0 if either the feature
               is not implemented or if the feature is not currently available. */

            val = Pylon.DeviceFeatureIsAvailable(hDev, "BinningVertical");
            Console.WriteLine("The 'BinningVertical' feature {0} available.", val ? "is" : "isn't");

            /* If a feature is available, it can be read-only, write-only, or both
               readable and writable. Use the PylonDeviceFeatureIsReadable() and the
               PylonDeviceFeatureIsWritable() functions(). It is safe to call these functions
               for features that are currently not available or not implemented by the device.
               A feature that is not available or not implemented is neither readable nor writable.
               The readability and writability of a feature can change depending on the current 
               state of the device. For example, the Width parameter may not be writable when
               the camera is acquiring images. */

            val = Pylon.DeviceFeatureIsReadable(hDev, "Width");
            Console.WriteLine("The 'Width' feature {0} readable.", val ? "is" : "isn't");

            val = Pylon.DeviceFeatureIsReadable(hDev, "MyCustomFeature");
            Console.WriteLine("The 'MyCustomFeature' feature {0} readable.", val ? "is" : "isn't");

            val = Pylon.DeviceFeatureIsWritable(hDev, "Width");
            Console.WriteLine("The 'Width' feature {0} writable.", val ? "is" : "isn't");
            Console.WriteLine("");
        }

        /* This function demonstrates how to handle integer camera parameters. */
        private static void demonstrateIntFeature(PYLON_DEVICE_HANDLE hDev)
        {
            string featureName = "Width";  /* Name of the feature used in this sample: AOI Width. */
            long val, min, max, incr;      /* Properties of the feature. */

            /* 
               Query the current value, the allowed value range, and the increment of the feature. 
               For some integer features, you are not allowed to set every value within the 
               value range. For example, for some cameras the Width parameter must be a multiple 
               of 2. These constraints are expressed by the increment value. Valid values 
               follow the rule: val >= min && val <= max && val == min + n * inc.
       
               To help clarify this sample, we don't check for the feature accessibility as demonstrated 
               in the demonstrateAccessibility() function. 
            */
            min = Pylon.DeviceGetIntegerFeatureMin(hDev, featureName);  /* Get the minimum value. */
            max = Pylon.DeviceGetIntegerFeatureMax(hDev, featureName);  /* Get the maximum value. */
            incr = Pylon.DeviceGetIntegerFeatureInc(hDev, featureName);  /* Get the increment value. */
            val = Pylon.DeviceGetIntegerFeature(hDev, featureName);     /* Get the current value. */

            Console.WriteLine("{0}: min= {1}  max= {2}  incr={3}  Value={4}", featureName, min, max, incr, val);

            /* Set the Width to its maximum allowed value. */
            Pylon.DeviceSetIntegerFeature(hDev, featureName, max);
            Console.WriteLine("The {0} was set to {1}.", featureName, max);
        }
        
        /* Some features are floating point features. This function illustrates how to set and get floating
           point parameters. */
        private static void demonstrateFloatFeature(PYLON_DEVICE_HANDLE hDev)
        {
            string featureName = "Gamma";  /* The name of the feature used. */
            bool isAvailable;              /* Is the feature available? */
            bool isWritable;               /* Is the feature writable? */
            double min, max, value;          /* Value range and current value. */

            isAvailable = Pylon.DeviceFeatureIsAvailable(hDev, featureName);

            if (isAvailable)
            {
                /* Query the value range and the current value. */
                min = Pylon.DeviceGetFloatFeatureMin(hDev, featureName);
                max = Pylon.DeviceGetFloatFeatureMax(hDev, featureName);
                value = Pylon.DeviceGetFloatFeature(hDev, featureName);

                Console.WriteLine("{0}: min = {1}, max = {2}, value = {3}", featureName, min, max, value);

                /* Set a new value. */
                isWritable = Pylon.DeviceFeatureIsWritable(hDev, featureName);
                if (isWritable)
                {
                    value = 0.5 * (min + max);
                    Console.WriteLine("Setting {0} to {1}.", featureName, value);
                    Pylon.DeviceSetFloatFeature(hDev, featureName, value);
                }
            }
            else
            {
                Console.WriteLine("The {0} feature isn't available.", featureName);
            }

        }

        /* Some features are boolean features that can be switched on and off. 
           This function illustrates how to access boolean features. */
        private static void demonstrateBooleanFeature(PYLON_DEVICE_HANDLE hDev)
        {
            string featureName = "GammaEnable"; /* The name of the feature. */
            bool isWritable;                    /* Is the feature writable? */
            bool value;                         /* The value of the feature. */

            /* Check to see if the feature is writable. */
            isWritable = Pylon.DeviceFeatureIsWritable(hDev, featureName);

            if (isWritable)
            {
                /* Retrieve the current state of the feature. */
                value = Pylon.DeviceGetBooleanFeature(hDev, featureName);

                Console.WriteLine("The {0} features is {1}.", featureName, value ? "on" : "off");

                /* Set a new value. */
                value = !value;  /* New value. */
                Console.WriteLine("Switching the {0} feature {1}.", featureName, value ? "on" : "off");
                Pylon.DeviceSetBooleanFeature(hDev, featureName, value);
            }
            else
            {
                Console.WriteLine("The {0} feature isn't writable.", featureName);
            }
        }
        
        /*
          Regardless of the parameter's type, any parameter value can be retrieved as a string. Likewise, each parameter
          can be set by passing in a string. This function illustrates how to set and get the 
          Width parameter as a string. As demonstrated above, the Width parameter is of the integer type.  
          */
        private static void demonstrateFromStringToString(PYLON_DEVICE_HANDLE hDev)
        {
            string featureName = "Width";   /* The name of the feature. */
            string value;

            /* Get the value of a feature as a string. */
            value = Pylon.DeviceFeatureToString(hDev, featureName);

            Console.WriteLine("{0}: {1}", featureName, value);

            /* A feature can be set as a string using the PylonDeviceFeatureFromString() function. 
               If the content of a string can not be converted to the type of the feature, an 
               error is returned. */

            try
            {
                Pylon.DeviceFeatureFromString(hDev, featureName, "fourty-two"); /* Cannot be converted to an integer. */
            }
            catch (Exception e)
            {
                /* Retrieve the error message. */
                string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();
                Console.WriteLine("Exception caught:");
                Console.WriteLine(e.Message);
                if (msg != "\n")
                {
                    Console.WriteLine("Last error message:");
                    Console.WriteLine(msg);
                }
            }
        }
        
        /* There are camera features that behave like enumerations. These features can take a value from a fixed 
           set of possible values. One example is the pixel format feature. This function illustrates how to deal with 
           enumeration features. 

        */
        private static void demonstrateEnumFeature(PYLON_DEVICE_HANDLE hDev)
        {
            string value;                     /* The current value of the feature. */
            bool isWritable,
                supportsMono8,
                supportsYUV422Packed,
                supportsMono16;


            /* The allowed values for an enumeration feature are represented as strings. Use the 
            PylonDeviceFeatureFromString() and PylonDeviceFeatureToString() methods for setting and getting
            the value of an enumeration feature. */

            /* Get the current value of the enumeration feature. */
            value = Pylon.DeviceFeatureToString(hDev, "PixelFormat");

            Console.WriteLine("PixelFormat: {0}", value);

            /*
              For an enumeration feature, the pylon Viewer's "Feature Documentation" window lists the 
              names of the possible values. Some of the values might not be supported by the device. 
              To check if a certain "SomeValue" value for a "SomeFeature" feature can be set, call the 
              PylonDeviceFeatureIsAvailable() function with "EnumEntry_SomeFeature_SomeValue" as an argument. 
            */
            /* Check to see if the Mono8 pixel format can be set. */
            supportsMono8 = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_PixelFormat_Mono8");
            Console.WriteLine("Mono8 {0} a supported value for the PixelFormat feature.", supportsMono8 ? "is" : "isn't");

            /* Check to see if the YUV422Packed pixel format can be set. */
            supportsYUV422Packed = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_PixelFormat_YUV422Packed");
            Console.WriteLine("YUV422Packed {0} a supported value for the PixelFormat feature.", supportsYUV422Packed ? "is" : "isn't");

            /* Check to see if the Mono16 pixel format can be set. */
            supportsMono16 = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_PixelFormat_Mono16");
            Console.WriteLine("Mono16 {0} a supported value for the PixelFormat feature.", supportsMono16 ? "is" : "isn't");

            /* Before writing a value, we recommend checking to see if the enumeration feature itself is
               currently writable. */
            isWritable = Pylon.DeviceFeatureIsWritable(hDev, "PixelFormat");
            if (isWritable)
            {
                /* The PixelFormat feature is writable. Set it to one of the supported values. */
                if (supportsMono16)
                {
                    Console.WriteLine("Setting PixelFormat to Mono16.");
                    Pylon.DeviceFeatureFromString(hDev, "PixelFormat", "Mono16");
                }
                else if (supportsYUV422Packed)
                {
                    Console.WriteLine("Setting PixelFormat to YUV422Packed.");
                    Pylon.DeviceFeatureFromString(hDev, "PixelFormat", "YUV422Packed");
                }
                else if (supportsMono8)
                {
                    Console.WriteLine("Setting PixelFormat to Mono8.");
                    Pylon.DeviceFeatureFromString(hDev, "PixelFormat", "Mono8");
                }

                /* Reset the PixelFormat feature to its previous value. */
                Pylon.DeviceFeatureFromString(hDev, "PixelFormat", value);
            }
        }

        /* There are camera features, such as AcquisitionStart, that represent a command. 
           This function that loads the default set, illustrates how to execute a command feature.  */
        private static void demonstrateCommandFeature(PYLON_DEVICE_HANDLE hDev)
        {
            /* Before executing the user set load command, the user set selector must be
               set to the default set. Since we are focusing on the command feature, 
               we skip the recommended steps for checking the availability of the user set 
               related features and values. */

            /* Choose the default set (which includes one of the factory setups). */
            Pylon.DeviceFeatureFromString(hDev, "UserSetSelector", "Default");

            /* Execute the user set load command. */
            Console.WriteLine("Loading the default settings.");
            Pylon.DeviceExecuteCommandFeature(hDev, "UserSetLoad");
        }
    }
}
