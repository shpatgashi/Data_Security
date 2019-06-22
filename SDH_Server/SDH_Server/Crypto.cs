using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace SDH_Server
{
    class Crypto
    {
        public Crypto()
        {
            exportKeys();
        }

        DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
        RSACryptoServiceProvider objRsa = new RSACryptoServiceProvider();
        string privatekey = null;

        public string decryptMessage(string IV, string key, string message)
        {
            byte[] decryptedKey = objRsa.Decrypt(Convert.FromBase64String(key), true);

            objDes.Padding = PaddingMode.Zeros;
            objDes.Mode = CipherMode.CBC;
            objDes.IV = Convert.FromBase64String(IV);
            objDes.Key = decryptedKey;

            MemoryStream ms = new MemoryStream(Convert.FromBase64String(message));
            CryptoStream cs = new CryptoStream(ms, objDes.CreateDecryptor(), CryptoStreamMode.Read);


            byte[] bytePlaintext = new byte[ms.Length];
            cs.Read(bytePlaintext, 0, bytePlaintext.Length);
            cs.Close();

            return Encoding.UTF8.GetString(bytePlaintext);
            
        }
        
        public string showPrivateKey()
        {
            return objRsa.ToXmlString(true);
        }

        
        public void exportKeys()
        {

            string strXmlParametrat = objRsa.ToXmlString(false);
            StreamWriter sw = new StreamWriter("publickey.xml");
            sw.Write(strXmlParametrat);
            sw.Close();
        }

        public string encrypt(string message)
        {
            objDes.Padding = PaddingMode.Zeros;
            objDes.Mode = CipherMode.CBC;

            byte[] bytePlainText = Encoding.UTF8.GetBytes(message);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(
            ms, objDes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytePlainText, 0, bytePlainText.Length);
            cs.Close();

            byte[] byteCiphertext = ms.ToArray();

            return Convert.ToBase64String(byteCiphertext);
        }
    }
}
