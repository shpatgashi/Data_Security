using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace SDH_Client
{
    class Crypto
    {
        RSACryptoServiceProvider objRsa = new RSACryptoServiceProvider();
        DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();


        public string getKey(string path)


        {


            StreamReader sr = new StreamReader(path);
            string key = sr.ReadToEnd();
            sr.Close();

            int ModFrom = key.LastIndexOf("<Modulus>") + 9;
            int ModTo = key.IndexOf("</Modulus>");

            int ExpFrom = key.LastIndexOf("<Exponent>") + 10;
            int ExpTo = key.LastIndexOf("</Exponent>");

            string exponent = key.Substring(ExpFrom, ExpTo - ExpFrom);
            string modulus = key.Substring(ModFrom, ModTo - ModFrom);

            return modulus + ":" + exponent;
        }

        public string encryptMessage(string text)
        {

            
            objDes.Padding = PaddingMode.Zeros;
            objDes.GenerateKey();
            objDes.Mode = CipherMode.CBC;

            byte[] bytePlainText = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(
            ms, objDes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytePlainText, 0, bytePlainText.Length);
            cs.Close();


            byte[] byteCiphertext = ms.ToArray();


            return Convert.ToBase64String(byteCiphertext);
        }


        public string encryptDesKey(string path)
        {
            byte[] byteDesKey = objDes.Key;


            RSAParameters rs = new RSAParameters();

            string rsaKey = getKey(path);
            string[] keys = rsaKey.Split(':');

            rs.Modulus = Convert.FromBase64String(keys[0]);
            rs.Exponent = Convert.FromBase64String(keys[1]);

            objRsa.ImportParameters(rs);



            byte[] encryptedKey = objRsa.Encrypt(byteDesKey, true);


            return Convert.ToBase64String(encryptedKey);

        }

           public string getIV()
        {

            return Convert.ToBase64String(objDes.IV);

        }


    }


}
