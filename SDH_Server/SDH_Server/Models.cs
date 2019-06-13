using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Security;


namespace SDH_Server
{
    class Models
    {
        DBCONNECTION conn = new DBCONNECTION();

        private string username;
        private string userPassword;
        private string position;
        private double salary;
        private int bonuses;
        private int experience;



        public Models(string username, string userPassword, string position,double salary, int bonuses, int experience)
        {
            this.username = username;
            this.userPassword = userPassword;
            this.position = position;
            this.salary = salary;
            this.bonuses = bonuses;
            this.experience = experience;
        }

        // query per insertim te punetoreve
        //public bool InsertWorker(string username, string userPassword, string position, double salary, int bonuses, int experience)
        //{
        //    if (conn.OpenConn())
        //    {
        //        MySqlCommand query = new MySqlCommand(null, conn.getConnection());

        //        query.CommandText = "INSERT INTO worker(username, password, salt, position,salary,bonuses, experience) " +
        //            "VALUES (@user,@password, @salt, @positon, @salary, @bonuses, @experience)";


        //        try
        //        {
        //            // prepared statement binding values


        //            MySqlParameter userParameter = new MySqlParameter("@user", MySqlDbType.Text, 45);
        //            userParameter.Value = username;
        //            MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.Text, 45);
        //            passwordParameter.Value = generateSaltedHashPassword(userPassword, generateSalt()) ;
        //            MySqlParameter saltParameter = new MySqlParameter("@salt", MySqlDbType.Text);
        //            saltParameter.Value = generateSalt();
        //            MySqlParameter positionParameter = new MySqlParameter("@position", MySqlDbType.Text);
        //            positionParameter.Value = position;
        //            MySqlParameter salaryParameter = new MySqlParameter("@salary", MySqlDbType.Double);
        //            salaryParameter.Value = salary;
        //            MySqlParameter bonusesParameter = new MySqlParameter("@bonuses", MySqlDbType.Int16);
        //            bonusesParameter.Value = bonuses;
        //            MySqlParameter experienceParameter = new MySqlParameter("@experience", MySqlDbType.Int16);
        //            experienceParameter.Value = experience;


        //            query.Parameters.Add(userParameter);
        //            query.Parameters.Add(passwordParameter);
        //            query.Parameters.Add(saltParameter);
        //            query.Parameters.Add(positionParameter);
        //            query.Parameters.Add(salaryParameter);
        //            query.Parameters.Add(bonusesParameter);
        //            query.Parameters.Add(experienceParameter);

        //            query.Prepare();

        //            return (query.ExecuteNonQuery() > 0);

        //        }
        //        catch (MySqlException ex)
        //        {

        //            return false;
        //        }


        //    }
        //    return false;
        //}

        public string generateSaltedHashPassword(string Password, string salt)
        {
            string saltPassword = Password + salt;
            byte[] byteSaltPassword = Encoding.UTF8.GetBytes(saltPassword);

            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();

            byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltPassword);

            string base64SaltedHashPassword =  Convert.ToBase64String(byteSaltedHashPassword);


            return base64SaltedHashPassword;
        }

        public string generateSalt()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);


            return rnd.Next(100000, 1000000).ToString();

        }

    }


}

