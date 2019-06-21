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
        DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
        RSACryptoServiceProvider objRsa = new RSACryptoServiceProvider();
        RSACryptoServiceProvider objRsa1 = new RSACryptoServiceProvider();
        public string decryptMessage(string key, string message)
        {

            objDes.Padding = PaddingMode.Zeros;
            objDes.Mode = CipherMode.CBC;

            objDes.Key = decryptDesKey(key);

            MemoryStream ms = new MemoryStream(Convert.FromBase64String(message));
            CryptoStream cs = new CryptoStream(ms, objDes.CreateDecryptor(), CryptoStreamMode.Read);


            byte[] bytePlaintext = new byte[ms.Length];
            cs.Read(bytePlaintext, 0, bytePlaintext.Length);
            cs.Close();
            return Convert.ToBase64String(bytePlaintext);

            //objDes.IV = Convert.FromBase64String(parts[0]);

            //byte[] nice = objRsa.Decrypt(Convert.FromBase64String(message), true);

            //byte[] byteText = Convert.FromBase64String(parts[2]);
            //MemoryStream ms = new MemoryStream(byteText);
            //CryptoStream cs = new CryptoStream(ms, objDes.CreateDecryptor(), CryptoStreamMode.Read);


            //byte[] bytePlaintext = new byte[ms.Length];
            //cs.Read(bytePlaintext, 0, bytePlaintext.Length);
            //cs.Close();
            //return Convert.ToBase64String(nice);

        }
        public byte[] decryptDesKey(string encKey)
        {
           
           
            
            byte[] decryptedKey = objRsa1.Decrypt(Convert.FromBase64String(encKey), true);

            return decryptedKey;

        }


 

        public void exportKeys()
        {

            //  string privatexml = objRsa.ToXmlString(true);
            RSAParameters param = objRsa.ExportParameters(true);
            objRsa1.ImportParameters(param);
                string strXmlParametrat = objRsa.ToXmlString(false);
                StreamWriter sw = new StreamWriter("publickey.xml");
                sw.Write(strXmlParametrat);
                sw.Close();
            
        }

        //public void showKey()
        //{
        //    StreamReader sr = new StreamReader("privatekey.xml");
        //    string strXmlParametrat = sr.ReadToEnd();
        //    sr.Close();

        //    objRsa.FromXmlString(strXmlParametrat);
        //}

        

        public void setIV(byte[] IV)
        {
            objDes.IV = IV;
        }
    }
}
