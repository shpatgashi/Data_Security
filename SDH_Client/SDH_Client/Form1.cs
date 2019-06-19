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

            thread = new Thread(read);
            thread.Start();
        }


        public void read()
        {
            
            string key =  getRSAKey();
            
            string order = "1/ " + txtLogUsername.Text + "/" + txtLogPassword.Text;
            string toSend = cr.getIV() + "~" + cr.encryptDesKey(key) + "~" + cr.encryptMessage(order) + "~";
            byte[] byteText = Encoding.UTF8.GetBytes(toSend);
            ns.Write(byteText, 0, byteText.Length);



          }
            
        public string getRSAKey()
        {
            string filePath = "C:\\Users\\KobitPC\\Documents\\GitHub\\proj2_SDH19\\SDH_Server\\SDH_Server\\bin\\Debug\\publickey.xml";

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {

                    string key = sr.ReadToEnd();



                    return key;
                }

            }
            else return "Error !";
    }       
    
               

            
        

        private void btnLogin_Click(object sender, EventArgs e)
        {

            read();
           
            
            //ns.Write(Convert.FromBase64String(txtLogUsername.Text), 0, Convert.FromBase64String(txtLogUsername.Text).Length);




            


            //}
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
        //    string filePath = "";
        //    OpenFileDialog opf = new OpenFileDialog();
        //    if (opf.ShowDialog() == DialogResult.OK)
        //    {
        //        filePath = opf.FileName;
        //    }
        //    if (File.Exists(filePath))
        //    {
        //        using (StreamReader st = new StreamReader(filePath))
        //        {

        //            string order = "2/" + txtUsername.Text + "/" + txtPassword.Text + "/" + txtPosition.Text + "/" + txtSalary.Text
        //             + "/" + txtBonuses.Text + "/" + txtExperience.Text;

        //            string toSend = cr.getIV() + "~" + cr.encryptDesKey(filePath) + "~" + cr.encryptMessage(order);

        //            txtLogUsername.Text = toSend;
                    
        //       }
        //   }
        }
    }
}