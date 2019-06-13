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

namespace SDH_Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            listener = new System.Net.Sockets.TcpListener(2020);
            listener.Start();

            client = listener.AcceptTcpClient();
            ns = client.GetStream();
            // thread = new Thread();
            //thread.Start();

        }
        TcpListener listener;
        TcpClient client;
        NetworkStream ns;

        string request = "";
        string response = "";
        string encResponse = "";
        Thread thread = null;

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
    
        private void btnInsert_Click(object sender, EventArgs e)
        {
            Models models = new Models(txtUsername.Text, txtPass.Text, txtPosition.Text, Double.Parse(txtSalary.Text),
                int.Parse(txtBonuses.Text), int.Parse(txtExperience.Text));

            models.InsertWorker(txtUsername.Text, txtPass.Text, txtPosition.Text, Double.Parse(txtSalary.Text),
                int.Parse(txtBonuses.Text), int.Parse(txtExperience.Text));
            MessageBox.Show("OK");
        }
    }
}
