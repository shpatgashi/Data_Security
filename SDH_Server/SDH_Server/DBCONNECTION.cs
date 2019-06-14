using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDH_Server
{
    public class DBCONNECTION
    {

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public MySqlConnection getConnection()
        {
            return connection;
        }

        //Constructor
        public DBCONNECTION()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "workers";
            uid = "root";
            password = "itsfuntostayatymca";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        // Hapim connection
        public bool OpenConn()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }


        // mbyllim connection
        public bool CloseConn()
        {
            if (OpenConn())
            {
                try
                {
                         connection.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    return false;
                }

            }
            else return false;

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


    }
}