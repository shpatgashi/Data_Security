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
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace SDH_Server
{
    public partial class Server : Form
    {
        DBCONNECTION conn = new DBCONNECTION();

        private string username;
        private string userPassword;
        private string position;
        private double salary;
        private int bonuses;
        private int experience;



        public Server()
        {

            InitializeComponent();
            
        }
        TcpListener listener;
        TcpClient client;
        NetworkStream ns;

        string request = "";
        string response = "";
        string encResponse = "";
        Thread thread = null;
        
    
        
        private void InsertWorker(string username, string userPassword, string position, double salary, int bonuses, int experience)
        {
            string query = "INSERT INTO worker(username, password, salt, position,salary,bonuses, experience) " +
                 "VALUES (@user,@password, @salt, @position, @salary, @bonuses, @experience)";

            MySqlConnection getConn = conn.getConnection();


            try
            {
                getConn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn.getConnection());

                // prepared statement binding values


                MySqlParameter userParameter = new MySqlParameter("@user", MySqlDbType.Text, 45);
                userParameter.Value = username;
                MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.Text, 45);
                passwordParameter.Value = generateSaltedHashPassword(userPassword, generateSalt());
                MySqlParameter saltParameter = new MySqlParameter("@salt", MySqlDbType.Text);
                saltParameter.Value = generateSalt();
                MySqlParameter positionParameter = new MySqlParameter("@position", MySqlDbType.Text);
                positionParameter.Value = position;
                MySqlParameter salaryParameter = new MySqlParameter("@salary", MySqlDbType.Double);
                salaryParameter.Value = salary;
                MySqlParameter bonusesParameter = new MySqlParameter("@bonuses", MySqlDbType.Int16);
                bonusesParameter.Value = bonuses;
                MySqlParameter experienceParameter = new MySqlParameter("@experience", MySqlDbType.Int16);
                experienceParameter.Value = experience;


                cmd.Parameters.Add(userParameter);
                cmd.Parameters.Add(passwordParameter);
                cmd.Parameters.Add(saltParameter);
                cmd.Parameters.Add(positionParameter);
                cmd.Parameters.Add(salaryParameter);
                cmd.Parameters.Add(bonusesParameter);
                cmd.Parameters.Add(experienceParameter);

                cmd.Prepare();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Nice");

                }
                else { MessageBox.Show("FAIL"); }


                getConn.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private string generateSaltedHashPassword(string Password, string salt)
        {
            string saltPassword = Password + salt;
            byte[] byteSaltPassword = Encoding.UTF8.GetBytes(saltPassword);

            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();

            byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltPassword);

            string base64SaltedHashPassword = Convert.ToBase64String(byteSaltedHashPassword);


            return base64SaltedHashPassword;
        }

        private string generateSalt()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);


            return rnd.Next(100000, 1000000).ToString();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            listener = new System.Net.Sockets.TcpListener(2020);
            listener.Start();
            xmlenc rsakey = new xmlenc();
            try
            {
                rsakey.exportPublicKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            client = listener.AcceptTcpClient();
            ns = client.GetStream();
            // thread = new Thread();
            //thread.Start();


        }

        


        
        private void getUser(string username)
        {
            string query = "Select * from worker where username = @username";


            try
            {
                MySqlConnection getConn = conn.getConnection();

                getConn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn.getConnection());


                MySqlParameter userParameter = new MySqlParameter("@username", MySqlDbType.Text, 45);
                userParameter.Value = username;
                cmd.Parameters.Add(userParameter);

                cmd.Prepare();


                DataSet ds = new DataSet();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                mySqlDataAdapter.Fill(ds);

                getConn.Close();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("An error occured ...");
                }
                else
                {
                    MessageBox.Show("Your info : \nUsername: " + username + "\nPosition : " + ds.Tables[0].Rows[0]["position"].ToString()
                        + "\nSalary : " + ds.Tables[0].Rows[0]["salary"].ToString() + "\nBonuses : " + ds.Tables[0].Rows[0]["bonuses"].ToString()
                        + "\nExperience : " + ds.Tables[0].Rows[0]["experience"].ToString());

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error : " + ex);
            }

        }


        private bool isUser(string username, string password)
        {
            string query = "Select password, salt from worker where username = @username";


            try
            {
                MySqlConnection getConn = conn.getConnection();

                getConn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn.getConnection());


                MySqlParameter userParameter = new MySqlParameter("@username", MySqlDbType.Text, 45);
                userParameter.Value = username;
                cmd.Parameters.Add(userParameter);

                cmd.Prepare();


                DataSet ds = new DataSet();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                mySqlDataAdapter.Fill(ds);

                getConn.Close();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Username is incorrect !");
                    return false;
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["Password"].ToString() ==
                        generateSaltedHashPassword(password, ds.Tables[0].Rows[0]["salt"].ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password !!");
                        return false;
                    }



                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error ! " + ex);
                return false;
            }

        }

      
    }
}
