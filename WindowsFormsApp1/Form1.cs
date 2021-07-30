using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kaenx.Konnect;
using Kaenx.Konnect.Connections;
using Kaenx.Konnect.Classes;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            knx();
        }

        private async Task knx()
        {
            try
            {
                Kaenx.Konnect.Connections.IKnxConnection _connIp = new Kaenx.Konnect.Connections.KnxIpTunneling("192.168.178.101", 3671);

                await _connIp.Connect();

                BusDevice dev = new BusDevice("1.1.2", _connIp);
                await dev.Connect();
                byte[] data = await dev.MemoryRead(0, 16);
                string mask = await dev.DeviceDescriptorRead(); //Returns Mask Version like MV-0701, MV-07B0, ...
                //byte[] serialbytes = await dev.PropertyRead("DeviceSerialNumber"); //Returns SerialNumber of Device as Byte Array
                await dev.Restart();
                await dev.Disconnect();
            }
            catch(System.Reflection.ReflectionTypeLoadException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
