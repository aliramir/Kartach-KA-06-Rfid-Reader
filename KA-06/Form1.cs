using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using System.Management; // need to add System.Management to your project references.
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace KA_06
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            LoadPorts();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private static List<UsbDeviceInfo> GetUsbDevices()
        {
            var devices = new List<UsbDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new UsbDeviceInfo(
                    (string)device.GetPropertyValue("DeviceID"),
                    (string)device.GetPropertyValue("PNPDeviceID"),
                    (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }

        private void LoadPorts()
        {
            var usbDevices = GetUsbDevices();
            for (var i = 0; i < usbDevices.Count; i++)
            {
                var item = new ComboboxItem
                {
                    Text = usbDevices[i].Description,
                    Value = i + 1
                };
                Ports.Items.Add(item);
            }
            Ports.SelectedIndex = 0;
            //MessageBox.Show((Ports.SelectedItem as ComboboxItem)?.Value.ToString());
        }
        private class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        private void RefreshPortsList_Click(object sender, EventArgs e)
        {
            LoadPorts();
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();

            var port = Convert.ToInt32((Ports.SelectedItem as ComboboxItem)?.Value.ToString());
            serialPort1.PortName = "COM" + port;
            serialPort1.BaudRate = Convert.ToInt32(BaudRates.Text);
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Open();

            if (serialPort1.IsOpen)
            {
                ConnectBtn.Enabled = false;
                DisconnectBtn.Enabled = true;
            }
            else
            {
                ConnectBtn.Enabled = true;
                DisconnectBtn.Enabled = false;
            }
            
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            if (serialPort1.IsOpen)
            {
                ConnectBtn.Enabled = false;
                DisconnectBtn.Enabled = true;
            }
            else
            {
                ConnectBtn.Enabled = true;
                DisconnectBtn.Enabled = false;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //serialPort1.WriteLine("M100030050\r\n");
            MessageBox.Show(serialPort1.ReadExisting());
        }

        private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }

        private void serialPort1_PinChanged(object sender, SerialPinChangedEventArgs e)
        {

        }
    }
}
