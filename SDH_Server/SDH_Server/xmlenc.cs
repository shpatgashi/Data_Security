using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SDH_Server
{
    class xmlenc
    {
        RSACryptoServiceProvider objRsa =
            new RSACryptoServiceProvider();
    
      
        public void exportPublicKey()
        {
            string strXmlParametrat = objRsa.ToXmlString(false);
            StreamWriter sw = new StreamWriter("publickey.xml");
            sw.Write(strXmlParametrat);
            sw.Close();
        }

        private void showKey()
        {
            StreamReader sr = new StreamReader("celesiPublik.xml");
            string strXmlParametrat = sr.ReadToEnd();
            sr.Close();

            objRsa.FromXmlString(strXmlParametrat);
        }
    }
}
