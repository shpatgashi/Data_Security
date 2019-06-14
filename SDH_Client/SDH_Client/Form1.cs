using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDH_Client
{
    public partial class Client : Form
    {
        private const int portNum = 2020;
        private const string hostName = "localhost";
        TcpClient client;
        NetworkStream ns;
        Thread thread = null;

        string encResponse = "";
        string encRequest = string.Empty;
        public Client()
        {
            InitializeComponent();
            client = new TcpClient(hostName, portNum);
            ns = client.GetStream();

            //thread = new Thread();
            //thread.Start();
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }
    }
}
