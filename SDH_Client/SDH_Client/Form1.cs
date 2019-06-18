using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        Crypto cr = new Crypto();
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
        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                filePath = opf.FileName;
            }
            if (File.Exists(filePath))
            {
                using (StreamReader st = new StreamReader(filePath))
                {
                    string order = "1:" + txtLogUsername.Text + ":" + txtLogPassword;

                    txtUsername.Text = cr.encryptMessage(order);
                    txtPosition.Text =  cr.encryptDesKey(filePath);

                    string toSend = cr.getIV() + "~" + cr.encryptDesKey(filePath) + "~" + cr.encryptMessage(order);


                    txtExperience.Text = toSend;
                }


            }
        }

       
    }
}