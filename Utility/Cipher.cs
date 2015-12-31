using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Cipher
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "769y7a6z3b968sug";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        // Default passPhrase
        private const string defaultPassPhrase = "139hgsdnfv190u2tyjsdfp0v8";

        public static string Encrypt(string plainText, string passPhrase = defaultPassPhrase)
        {
            // Local values
            var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var password = new PasswordDeriveBytes(passPhrase, null);
            var keyBytes = password.GetBytes(keysize / 8);
            
            // Cipher
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            
            // Get encryptor
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            
            // Encrypt text-bytes
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            
            // Get cipher in bytes
            var cipherTextBytes = memoryStream.ToArray();
            
            // Close streams
            memoryStream.Close();
            cryptoStream.Close();

            // Get string based on bytes and return value
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string cipherText, string passPhrase = defaultPassPhrase)
        {
            // Local values
            var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var password = new PasswordDeriveBytes(passPhrase, null);
            var keyBytes = password.GetBytes(keysize / 8);
            
            // Cipher
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };

            // Get decryptor
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            
            // Decrypt text-bytes
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            // Close streams
            memoryStream.Close();
            cryptoStream.Close();
            
            // Parse decrypted value and return string
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

    }
}
