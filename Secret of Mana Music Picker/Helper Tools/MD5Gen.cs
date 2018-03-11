using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace MD5Gen
{
    public static class MD5Generator
    {
        //Function to return hexadecimal MD5 hash
        static public string GetMD5(string strInputPath)
        {
            try
            {
                //"using" here lets the compiler definitively know to dispose of the objects used when out of scope.
                //FileShare.ReadWrite is very important so other programs are not blocked from what they were doing with their files.
                using (FileStream streamFile = File.Open(strInputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (MD5 md5 = MD5.Create())
                using (CryptoStream cs = new CryptoStream(streamFile, md5, CryptoStreamMode.Read))
                {
                    int byteCount;
                    byte[] data = new byte[4096];


                    while ((byteCount = cs.Read(data, 0, data.Length)) > 0)
                    {
                                               
                    }

                    byte[] bytMD5 = md5.Hash;

                    return BitConverter.ToString(bytMD5).Replace("-", String.Empty).ToLower();
                }
            }
            catch (Exception Ex)
            {
                return "UnableToRead - Exception: " + Ex.Message.Replace('\r', ' ').Replace('\n', ' ');
            }
        }
    }
}
