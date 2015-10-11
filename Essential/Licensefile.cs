using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Essential
{
    public class LicenseFile
    {
        public string Username;
        public string Password;
        public string LicenseNumber;
        public LicenseFile(string fileContent)
        {
            string decrypted = Decrypt(fileContent, "Essential", "Essential041715", "SHA1", 3, "@1B2c3D4e5F6g7H8", 256);
            string Value;
            Console.WriteLine(decrypted);
            foreach (string s in decrypted.Split('|'))
            {
                Value = s.Split('=')[1];
                switch (s.Split('=')[0].ToLower())
                {
                    case "name":
                        Username = Value;
                        break;
                    case "password":
                        Password = Value;
                        break;
                    case "licensenumber":
                        LicenseNumber = Value;
                        break;
                }
            }
        }
        #region "AES"
        public string Encrypt(string passtext, string passPhrase, string saltV, string hashstring, int Iterations, string initVect, int keysize)
        {
            string functionReturnValue = null;
            byte[] initVectorBytes = null;
            initVectorBytes = Encoding.ASCII.GetBytes(initVect);
            byte[] saltValueBytes = null;
            saltValueBytes = Encoding.ASCII.GetBytes(saltV);
            byte[] plainTextBytes = null;
            plainTextBytes = Encoding.UTF8.GetBytes(passtext);
            PasswordDeriveBytes password = default(PasswordDeriveBytes);
            password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashstring, Iterations);
            byte[] keyBytes = null;
            keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = default(RijndaelManaged);
            symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = default(ICryptoTransform);
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = default(MemoryStream);
            memoryStream = new MemoryStream();
            CryptoStream cryptoStream = default(CryptoStream);
            cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = null;
            cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = null;
            cipherText = Convert.ToBase64String(cipherTextBytes);
            functionReturnValue = cipherText;
            return functionReturnValue;
        }
        public string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            string functionReturnValue = null;
            byte[] initVectorBytes = null;
            initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = null;
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = null;
            cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = default(PasswordDeriveBytes);
            password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = null;
            keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = default(RijndaelManaged);
            symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = default(ICryptoTransform);
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = default(MemoryStream);
            memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = default(CryptoStream);
            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = null;
            plainTextBytes = new byte[cipherTextBytes.Length + 1];
            int decryptedByteCount = 0;
            decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = null;
            plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            functionReturnValue = plainText;
            return functionReturnValue;
        }
        public void Authenticate()
        {
            WebClient wc = new WebClient();

        }
        #endregion
    }
}
