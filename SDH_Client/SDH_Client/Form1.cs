using JWT;
using JWT.Serializers;
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
	private bool login=false;
	private bool signup=false;

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
 
            string key = getRSAKey();
 
            if (login)
            {
                string order = "1/" + txtLogUsername.Text + "/" + txtLogPassword.Text;
                string toSend = cr.getIV() + "~" + cr.encryptDesKey(key) + "~" + cr.encryptMessage(order) + "~";
                byte[] byteText = Encoding.ASCII.GetBytes(toSend);
                ns.Write(byteText, 0, byteText.Length);
                login = false;
                MessageBox.Show("Client side : " + toSend);
 
            }
            else if (signup)
            {
                string order = "2/ " + txtUsername.Text + "/" + txtPassword.Text + "/" + txtPosition.Text + "/"
                    + txtSalary.Text + "/" + txtBonuses.Text + "/" + txtExperience.Text;
                string toSend = cr.getIV() + "~" + cr.encryptDesKey(key) + "~" + cr.encryptMessage(order) + "~";
                byte[] byteText = Encoding.ASCII.GetBytes(toSend);
                ns.Write(byteText, 0, byteText.Length);
                signup = false;
                MessageBox.Show("Client side : "+cr.getDesKey());
 
            }
 
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
		login=true;
            read();
               

        }


        private void btnSignup_Click(object sender, EventArgs e)
        {
		signup=true;
		read();
        }


        private string verifyToken(string token)
        {
 
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
 
                var json = decoder.Decode(token, getRSAKey(), verify: true);
 
                return json;
            }
            catch (TokenExpiredException)
            {
                MessageBox.Show("Token has expired !!!!");
                return null;
            }
 
            catch (SignatureVerificationException)
            {
                MessageBox.Show("Token had invalid signature !");
                return null;
            }
 
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            MessageBox.Show(verifyToken(rtVerify.Text));
        }
    }
}