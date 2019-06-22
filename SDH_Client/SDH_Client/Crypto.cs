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


        public string encryptMessage(string text)
        {


            objDes.GenerateKey();
            objDes.GenerateIV();
            objDes.Padding = PaddingMode.Zeros;
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


            byte[] encryptedKey = objRsa.Encrypt(byteDesKey, true);


            return Convert.ToBase64String(encryptedKey);

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

        public string getIV()
        {
            return Convert.ToBase64String(objDes.IV);
        }

        public string getDesKey()
        {
            return Convert.ToBase64String(objDes.Key);
        }
    }


}
