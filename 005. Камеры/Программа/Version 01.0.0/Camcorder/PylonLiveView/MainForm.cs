using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PylonC.NETSupportLibrary;
using System.Runtime.InteropServices;
using PylonC.NET;
using System.IO;
using System.Text;
using System.Threading;
/// <summary>
/// Название - Camcorder.
/// Назначение - программа вывода изображения с ip-камер.
/// Автор - Минин Илья Михайлович.
/// Номер версии - 1.0.0.0.
namespace Camcorder
{
    /* The main window. */
    public partial class MainForm : Form
    {
        private ImageProvider m_imageProvider = new ImageProvider(); /* Create one image provider. */
        private Bitmap m_bitmap = null;                              /* The bitmap is used for displaying the image. */

        public string thisText = null;                               // строка для сохранения заголовка формы

        // константы для настройки изображения камеры:
        const uint MAX_NUM_DEVICES = 4;
        const uint NUM_BUFFERS = 10;                                 /* Number of buffers used for grabbing. */

        const uint GIGE_PACKET_SIZE = 1500;                          /* Size of one Ethernet packet. */
        const uint GIGE_PROTOCOL_OVERHEAD = 36;                      /* Total number of bytes of protocol overhead. */

        const uint AllGroupMask = 0xffffffff;
        
        /* Set up the controls and events to be used and update the device list. */
        public MainForm()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            // Добавить обработчик событий для обработки необработанных исключений пользовательского интерфейса
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            // Добавить обработчик событий для обработки исключений потока пользовательского интерфейса
            Application.ThreadException += new ThreadExceptionEventHandler(Form1_UIThreadException);

            TopMost = true;  // окно приложения поверх всех остальных окон

            InitializeComponent();

            /* Register for the events of the image provider needed for proper operation. */
            m_imageProvider.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
            m_imageProvider.DeviceRemovedEvent += new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback);
            m_imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
            m_imageProvider.DeviceClosedEvent += new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
            m_imageProvider.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
            m_imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
            m_imageProvider.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);

            /* Update the list of available devices in the upper left area. */
            UpdateDeviceList();

            /* Enable the tool strip buttons according to the state of the image provider. */
            EnableButtons(m_imageProvider.IsOpen, false);
        }

        /* Handles the click on the continuous frame button. */
        private void toolStripButtonContinuousShot_Click(object sender, EventArgs e)
        {
            ContinuousShot(); /* Start the grabbing of images until grabbing is stopped. */
        }

        /* Handles the click on the stop frame acquisition button. */
        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            Stop(); /* Stops the grabbing of images. */
        }

        /* Handles the event related to the occurrence of an error while grabbing proceeds. */
        private void OnGrabErrorEventCallback(Exception grabException, string additionalErrorMessage)
        {
            Log.Write(grabException);

            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback), grabException, additionalErrorMessage);
                return;
            }

            if (Marshal.GetHRForException(grabException) == -2146233088)
                MessageBox.Show("Произошел сбой захвата изображения  в окне " + thisText + "!", "Ошибка");
            else
            ShowException(grabException, additionalErrorMessage);
            //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(grabException)));
        }

        /* Handles the event related to the removal of a currently open device. */
        private void OnDeviceRemovedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback));
                return;
            }
            /* Disable the buttons. */
            EnableButtons(false, false);
            /* Stops the grabbing of images. */
            Stop();
            /* Close the image provider. */
            CloseTheImageProvider();
            /* Since one device is gone, the list needs to be updated. */
            UpdateDeviceList();
        }

        /* Handles the event related to a device being open. */
        private void OnDeviceOpenedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback));
                return;
            }
            /* The image provider is ready to grab. Enable the grab buttons. */
            EnableButtons(true, false);
        }

        /* Handles the event related to a device being closed. */
        private void OnDeviceClosedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback));
                return;
            }
            /* The image provider is closed. Disable all buttons. */
            EnableButtons(false, false);
        }

        /* Handles the event related to the image provider executing grabbing. */
        private void OnGrabbingStartedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback));
                return;
            }

            /* Do not update device list while grabbing to avoid jitter because the GUI-Thread is blocked for a short time when enumerating. */
            updateDeviceListTimer.Stop();

            /* The image provider is grabbing. Disable the grab buttons. Enable the stop button. */
            EnableButtons(false, true);
        }

        /* Handles the event related to an image having been taken and waiting for processing. */
        private void OnImageReadyEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback));
                return;
            }

            try
            {
                /* Acquire the image from the image provider. Only show the latest image. The camera may acquire images faster than images can be displayed*/
                ImageProvider.Image image = m_imageProvider.GetLatestImage();

                /* Check if the image has been removed in the meantime. */
                if (image != null)
                {
                    /* Check if the image is compatible with the currently used bitmap. */
                    if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    {
                        /* Update the bitmap with the image data. */
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* To show the new image, request the display control to update itself. */
                        pictureBox.Refresh();
                    }
                    else /* A new bitmap is required. */
                    {
                        BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* We have to dispose the bitmap after assigning the new one to the display control. */
                        Bitmap bitmap = pictureBox.Image as Bitmap;
                        /* Provide the display control with the new bitmap. This action automatically updates the display. */
                        pictureBox.Image = m_bitmap;
                        if (bitmap != null)
                        {
                          /* Dispose the bitmap. */
                          bitmap.Dispose();
                        }
                    }
                    /* The processing of the image is done. Release the image buffer. */
                    m_imageProvider.ReleaseImage();
                    /* The buffer can be used for the next image grabs. */
                }
            }
            catch (Exception e)
            {
                ShowException(e, m_imageProvider.GetLastErrorMessage());
                Log.Write(e);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
            }
        }
        
        /* Handles the event related to the image provider having stopped grabbing. */
        private void OnGrabbingStoppedEventCallback()
        {
            try
            {
                if (InvokeRequired)
                {
                    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                    BeginInvoke(new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback));
                    return;
                }

                /* Enable device list update again */
                updateDeviceListTimer.Start();

                /* The image provider stopped grabbing. Enable the grab buttons. Disable the stop button. */
                EnableButtons(m_imageProvider.IsOpen, false);
            }
            catch (Exception e)
            {
                ShowException(e, m_imageProvider.GetLastErrorMessage());
                Log.Write(e);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
            }
        }

        /* Helps to set the states of all buttons. */
        private void EnableButtons(bool canGrab, bool canStop)
        {
            tSB_ContinuousShot.Enabled = canGrab;
            tSB_Stop.Enabled = canStop;
        }

        /* Stops the image provider and handles exceptions. */
        private void Stop()
        {
            /* Stop the grabbing. */
            try
            {
                m_imageProvider.Stop();
            }
            catch (Exception e)
            {
                ShowException(e, m_imageProvider.GetLastErrorMessage());
                Log.Write(e);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
            }
        }

        /* Closes the image provider and handles exceptions. */
        private void CloseTheImageProvider()
        {
            /* Close the image provider. */
            try
            {
                m_imageProvider.Close();
            }
            catch (Exception e)
            {
                ShowException(e, m_imageProvider.GetLastErrorMessage());
                Log.Write(e);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
            }
        }

        /* Starts the grabbing of images until the grabbing is stopped and handles exceptions. */
        private void ContinuousShot()
        {
            try
            {
                m_imageProvider.ContinuousShot(); /* Start the grabbing of images until grabbing is stopped. */
            }
            catch (Exception e)
            {
                ShowException(e, m_imageProvider.GetLastErrorMessage());
                Log.Write(e);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
            }
        }

        /* Updates the list of available devices in the upper left area. */
        private void UpdateDeviceList()
        {
            try
            {
                /* Ask the device enumerator for a list of devices. */
                List<DeviceEnumerator.Device> list = DeviceEnumerator.EnumerateDevices();

                ListView.ListViewItemCollection items = deviceListView.Items;
 
                /* Add each new device to the list. */
                foreach (DeviceEnumerator.Device device in list)
                {
                    bool newitem = true;
                    /* For each enumerated device check whether it is in the list view. */
                    foreach (ListViewItem item in items)
                    {
                        /* Retrieve the device data from the list view item. */
                        DeviceEnumerator.Device tag = item.Tag as DeviceEnumerator.Device;

                        if ( tag.FullName == device.FullName)
                        {
                            /* Update the device index. The index is used for opening the camera. It may change when enumerating devices. */
                            tag.Index = device.Index;
                            /* No new item needs to be added to the list view */
                            newitem = false;
                            break; 
                        }
                    }

                    /* If the device is not in the list view yet the add it to the list view. */
                    if (newitem)
                    {
                        ListViewItem item = new ListViewItem(device.Name);
                        if (device.Tooltip.Length > 0)
                        {
                            item.ToolTipText = device.Tooltip;
                        }
                        item.Tag = device;

                        /* Attach the device data. */
                        deviceListView.Items.Add(item);
                    }
                }

                /* Delete old devices which are removed. */
                foreach (ListViewItem item in items)
                {
                    bool exists = false;

                    /* For each device in the list view check whether it has not been found by device enumeration. */
                    foreach (DeviceEnumerator.Device device in list)
                    {
                        if (((DeviceEnumerator.Device)item.Tag).FullName == device.FullName)
                        {
                            exists = true;
                            break; 
                        }
                    }
                    /* If the device has not been found by enumeration then remove from the list view. */
                    if (!exists)
                    {
                        deviceListView.Items.Remove(item);
                    }
                }
            }
            catch (Exception e)
            {
                ShowException(e, m_imageProvider.GetLastErrorMessage());
                Log.Write(e);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
            }
        }

        /* Shows exceptions in a message box. */
        private void ShowException(Exception e, string additionalErrorMessage)
        {
            Log.Write(e);

            string more = "\n\nLast error message (may not belong to the exception):\n" + additionalErrorMessage;

            if (Marshal.GetHRForException(e) == -1023410172)
            {
                //MessageBox.Show("Перед закрытием окна нужно отключить захват видео!", "Ошибка");

                // закрыть все процессы Camcorder:
                string nameproc = "Camcorder";
                System.Diagnostics.Process[] AllProc = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process iProc in AllProc)
                    if (iProc.ProcessName.Contains(nameproc)) iProc.Kill();
            }
            else
                MessageBox.Show("Exception caught:\n" + e.Message + (additionalErrorMessage.Length > 0 ? more : ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)) + "ShowException");
        }

        /* Closes the image provider when the window is closed. */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs ev)
        {
            /* Stops the grabbing of images. */
            Stop();
            /* Close the image provider. */
            CloseTheImageProvider();
        }

        /* Handles the selection of cameras from the list box. The currently open device is closed and the first 
         selected device is opened. */
        private void deviceListView_SelectedIndexChanged(object sender, EventArgs ev)
        {
            this.Text = thisText;

            /* Close the currently open image provider. */
            /* Stops the grabbing of images. */
            Stop();
            /* Close the image provider. */
            CloseTheImageProvider();

            /* Open the selected image provider. */
            if (deviceListView.SelectedItems.Count > 0)
            {
                /* Get the first selected item. */
                ListViewItem item = deviceListView.SelectedItems[0];

                string deviceItem = deviceListView.SelectedItems[0].ToString();  // имя камеры

                // Записать в заголовок окна имя камеры:
                if (deviceItem != null)
                {
                    string camName = deviceItem.Replace("ListViewItem:", "");  // убрать "ListViewItem:" из имени камеры
                    this.Text += camName;
                }
                else
                    this.Text = thisText;

                /* Get the attached device data. */
                DeviceEnumerator.Device device = item.Tag as DeviceEnumerator.Device;

                try
                {
                    /* Open the image provider using the index from the device data. */
                    m_imageProvider.Open(device.Index);
                }
                catch (Exception e)
                {
                    Log.Write(e);

                    if (Marshal.GetHRForException(e) == -1040187391)
                        MessageBox.Show("Камера уже выбрана в другом окне или используется другим приложением! Выберите другую камеру!", "Ошибка");
                    else
                        ShowException(e, m_imageProvider.GetLastErrorMessage());
                        //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
                }
            }
        }

        /* If the F5 key has been pressed update the list of devices. */
        private void deviceListView_KeyDown(object sender, KeyEventArgs ev)
        {
            if (ev.KeyCode == Keys.F5)
            {
                ev.Handled = true;
                /* Update the list of available devices in the upper left area. */
                UpdateDeviceList();
            }
        }

        /* Timer callback used for periodically checking whether displayed devices are still attached to the PC. */
        private void updateDeviceListTimer_Tick(object sender, EventArgs e)
        {
            UpdateDeviceList();
        }

        /// <summary>
        /// Кнопка создания нового ведомого окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MainForm MainForm1 = new MainForm();            // создать форму
            MainForm1.Text = "Camcorder: slave window";     // заголовок второстепенной формы
            MainForm1.Show();                               // открыть форму
        }

        /// <summary>
        /// Свернуть/развернуть боковую панель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            // если ширина боковой панели = 200, то сделать ширину равной 25
            if (splitContainerImageView.SplitterDistance != 40)
            {
                splitContainerImageView.SplitterDistance = 40;
            }
            else
            {
                splitContainerImageView.SplitterDistance = 200;  // иначе ширина боковой панели = 200
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                thisText = this.Text;  // сохранить заголовок формы

                if (thisText == "Camcorder: master window") ActionCommands();

                /*
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;
                */
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        /// настройки изображения камеры
        /// </summary>
        public void ActionCommands()
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
                PYLON_WAITOBJECT_HANDLE[] hWait = new PYLON_WAITOBJECT_HANDLE[MAX_NUM_DEVICES];       /* Handle used for waiting for a grab to be finished. */
                uint[] payloadSize = new uint[MAX_NUM_DEVICES];                    /* Size of an image frame in bytes. */
                uint[] nStreams = new uint[MAX_NUM_DEVICES];                       /* The number of streams provided by the device. */
                PYLON_STREAMBUFFER_HANDLE[] hBuffer = new PYLON_STREAMBUFFER_HANDLE[MAX_NUM_DEVICES];
                PylonBuffer<Byte>[] buffer = new PylonBuffer<Byte>[MAX_NUM_DEVICES];

                /* Before using any pylon methods, the pylon runtime must be initialized. */
                Pylon.Initialize();

                /* Enumerate all camera devices. You must call 
                PylonEnumerateDevices() before creating a device. */
                numDevicesEnumerated = Pylon.EnumerateDevices();

                if (numDevicesEnumerated == 0)
                {
                    Pylon.Terminate();

                    MessageBox.Show("Устройства не найдены!", "Ошибка");
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

                    /* Set the pixel format to BayerRG12, where gray values will be output as 12 bit values for each pixel. */
                    /* ... Check first to see if the device supports the BayerRG12 format. */
                    isAvail = Pylon.DeviceFeatureIsAvailable(hDev[deviceIndex], "EnumEntry_PixelFormat_BayerRG12");
                    if (!isAvail)
                    {
                        /* Feature is not available. */
                        throw new Exception("Device doesn't support the BayerRG12 pixel format.");
                    }

                    /* ... Set the pixel format to BayerRG12. */
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
#if DEBUG
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "GevSCPD", 10000);
#else
                    Pylon.DeviceSetIntegerFeature(hDev[deviceIndex], "GevSCPD", (GIGE_PACKET_SIZE + GIGE_PROTOCOL_OVERHEAD) * (MAX_NUM_DEVICES - 1));
#endif
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
                    MessageBox.Show("Подходящие устройства не найдены!", "Ошибка");
                    Pylon.Terminate();  /* Releases all pylon resources. */
                    Environment.Exit(0);
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
                    Issue an ActionCommand.
                    ====================================================================== */
                /* Trigger the camera using an action command (w/o waiting for results).
                   If your setup support PTP, you could use a scheduled action command. 
                   Pylon.GigEIssueScheduledActionCommand(subnet, DefaultDeviceKey, DefaultGroupKey, 1, triggertime, 0)
                 */
                string subnet = Pylon.DeviceInfoGetPropertyValueByName(Pylon.DeviceGetDeviceInfoHandle(hDev[0]), "SubnetAddress");

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

                /* Shut down the pylon runtime system. Don't call any pylon function after 
                   calling PylonTerminate(). */
                Pylon.Terminate();
            }
            catch (Exception e)
            {
                Log.Write(e);

                /* Retrieve the error message. */
                string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();

                if (Marshal.GetHRForException(e) == -1040187391)
                    MessageBox.Show("Камера уже выбрана в другом окне или используется другим приложением!", "Ошибка");
                else
                {
                    MessageBox.Show(e.Message, "Ошибка");
                    if (msg != "\n")
                    {
                        MessageBox.Show(msg, "Ошибка");
                    }
                    //MessageBox.Show(Convert.ToString(Marshal.GetHRForException(e)));
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
                    catch (Exception ex)
                    {
                        /* No further handling here.*/
                        Log.Write(ex);
                    }
                }
                Pylon.Terminate();  /* Releases all pylon resources. */

                Environment.Exit(1);
            }
        }

        private void tSB_Help_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Camcorder.chm");

            //throw new Exception("2");
        }

        /// <summary>
        /// Класс записи логов
        /// </summary>
        public class Log
        {
            private static object sync = new object();
            public static void Write(Exception ex)
            {
                try
                {
                    // Путь .\\Log
                    string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                    if (!Directory.Exists(pathToLog))
                        Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                    string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
                    AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
                    string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n",
                    DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message);
                    lock (sync)
                    {
                        File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                    }
                }
                catch
                {
                    // Перехватываем все и ничего не делаем
                }
            }
        }

        /// <summary>
        /// Обработчик необработанных исключений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Log.Write(e);

            MessageBox.Show("MyHandler caught : " + e.Message);
            MessageBox.Show("Runtime terminating: {0}", args.IsTerminating.ToString());
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether
        // or not they wish to abort execution.
        private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
                result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Windows Forms Error",
                        "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
                Application.Exit();
        }

        // Creates the error message and displays it.
        private static DialogResult ShowThreadExceptionDialog(string title, Exception e)
        {
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:\n\n";
            errorMsg = errorMsg + e.Message + "\n\nStack Trace:\n" + e.StackTrace;
            return MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Stop);
        }

        /// <summary>
        /// Изменение размера блока картинки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Resize(object sender, EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            pictureBox.Width = (screenWidth * pictureBox.Height) / screenHeight;
        }
    }
}