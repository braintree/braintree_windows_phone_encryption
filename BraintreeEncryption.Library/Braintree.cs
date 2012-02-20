using System;

namespace BraintreeEncryption.Library
{
    public class Braintree
    {
        private const String Version = "1.0.0";
        private readonly String _publicKey;

        public Braintree(String publicKey)
        {
            _publicKey = publicKey;
        }

        public String GetPublicKey()
        {
            return _publicKey;
        }

        public String GetVersion()
        {
            return Version;
        }

        public String Encrypt(string dataToEncrypt)
        {
            var aes = new Aes();
            var rsa = new Rsa(_publicKey);
            var aesKey = aes.GenerateKey();
            var encryptedData = aes.Encrypt(dataToEncrypt, aesKey);
            var encryptedAesKey = rsa.Encrypt(aesKey);
            return GetPrefix() + encryptedAesKey + "$" + encryptedData;
        }

        private String GetPrefix()
        {
            return "$bt3|wp7_" + Version.Replace(".", "_") + "$";
        }
    }
}
