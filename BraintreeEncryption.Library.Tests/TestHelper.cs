using System;
using System.Security.Cryptography;
using System.Text;
using BraintreeEncryption.Library.BouncyCastle.Asn1;
using BraintreeEncryption.Library.BouncyCastle.Asn1.Pkcs;
using BraintreeEncryption.Library.BouncyCastle.Crypto.Encodings;
using BraintreeEncryption.Library.BouncyCastle.Crypto.Engines;
using BraintreeEncryption.Library.BouncyCastle.Crypto.Parameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BraintreeEncryption.Library.Tests
{
    public class TestHelper
    {
        private const int IVLength = 16;

        public static void IsNotEmpty(string value)
        {
            Assert.IsFalse(value.Equals(""));
        }

        public static string DecryptAes(string encrypted, byte[] aesKey)
        {
            var decoded = Convert.FromBase64String(encrypted);

            var iv = new byte[IVLength];
            Buffer.BlockCopy(decoded, 0, iv, 0, iv.Length);

            var encryptedData = new byte[decoded.Length - iv.Length];
            Buffer.BlockCopy(decoded, iv.Length, encryptedData, 0, encryptedData.Length);

            var decryptor = new AesManaged {KeySize = 256, BlockSize = 128}.CreateDecryptor(aesKey, iv);

            var decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return new UTF8Encoding().GetString(decryptedBytes, 0, decryptedBytes.Length);
        }

        public static byte[] DecryptRsa(string encrypted, string privateKey)
        {
            var rsaKeyParameters = GetRsaKeyParameters(privateKey);
            var rsaEngine = new Pkcs1Encoding(new RsaEngine());
            rsaEngine.Init(false, rsaKeyParameters);
            var decoded = Convert.FromBase64String(encrypted);
            var decryptedEncodedBytes = rsaEngine.ProcessBlock(decoded, 0, decoded.Length);
            var decryptedEncodedString = new UTF8Encoding().GetString(decryptedEncodedBytes, 0, decryptedEncodedBytes.Length);
            return Convert.FromBase64String(decryptedEncodedString);
        }

        private static RsaKeyParameters GetRsaKeyParameters(string privateKey)
        {
            var decodedPublicKeyString = Convert.FromBase64String(privateKey);
            var inputStream = new Asn1InputStream(decodedPublicKeyString);
            var derObject = inputStream.ReadObject();
            var asn1Sequence = Asn1Sequence.GetInstance(derObject);
            var keyStruct = new RsaPrivateKeyStructure(asn1Sequence);
            return new RsaKeyParameters(true, keyStruct.Modulus, keyStruct.PrivateExponent);
        }
    }
}
