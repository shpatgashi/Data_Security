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


        //public string getKey(string path)


        //{


        //    StreamReader sr = new StreamReader(path);
        //    string key = sr.ReadToEnd();
        //    sr.Close();

        //    int ModFrom = key.IndexOf("<RSAKeyValue>") ;
        //    int ModTo = key.LastIndexOf("</RSAKeyValue>")+14;

        //    //int ExpFrom = key.LastIndexOf("<Exponent>") + 10;
        //    //int ExpTo = key.LastIndexOf("</Exponent>");

        //    //string exponent = key.Substring(ExpFrom, ExpTo - ExpFrom);
        //    string modulus = key.Substring(ModFrom, ModTo);

        //    return modulus ;
        //}

        public string encryptMessage(string text)
        {


            objDes.Padding = PaddingMode.Zeros;
            objDes.GenerateKey();
            objDes.GenerateIV();
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
            
            objRsa.FromXmlString(path);
            //RSAParameters rs = new RSAParameters();

            //string rsaKey = getKey(path);
            //string[] keys = rsaKey.Split(':');

            //rs.Modulus = Convert.FromBase64String(keys[0]);
            //rs.Exponent = Convert.FromBase64String(keys[1]);

            //objRsa.ImportParameters(rs);



            byte[] encryptedKey = objRsa.Encrypt(byteDesKey, true);


            return Convert.ToBase64String(encryptedKey);

        }

         public string getIV()
        {

            return Convert.ToBase64String(objDes.IV);

        }

        public string decryptMessage(byte[] message) 
        {

            MemoryStream ms = new MemoryStream(message);
            CryptoStream cs = new CryptoStream(ms, objDes.CreateDecryptor(), CryptoStreamMode.Read);


            byte[] bytePlaintext = new byte[ms.Length];
            cs.Read(bytePlaintext, 0, bytePlaintext.Length);
            cs.Close();
            return Encoding.UTF8.GetString(bytePlaintext);

            
        }
    
        public string getDesKey()
        {
            return Convert.ToBase64String(objDes.Key);
        }
    }


}
