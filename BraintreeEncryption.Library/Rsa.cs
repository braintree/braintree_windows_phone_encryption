using System;
using System.Text;
using BraintreeEncryption.Library.BouncyCastle.Asn1;
using BraintreeEncryption.Library.BouncyCastle.Asn1.X509;
using BraintreeEncryption.Library.BouncyCastle.Crypto.Encodings;
using BraintreeEncryption.Library.BouncyCastle.Crypto.Engines;
using BraintreeEncryption.Library.BouncyCastle.Crypto.Parameters;

namespace BraintreeEncryption.Library
{
    public class Rsa
    {
        private readonly String _publicKeyString;

        public Rsa(String publicKeyString)
        {
            _publicKeyString = publicKeyString;
        }

        public string Encrypt(byte[] dataToEncrypt)
        {
            var rsaKeyParameters = GetRsaKeyParameters();
            var rsaEngine = new Pkcs1Encoding(new RsaEngine());
            rsaEngine.Init(true, rsaKeyParameters);
            var encodedDataToEncrypt = new UTF8Encoding().GetBytes(Convert.ToBase64String(dataToEncrypt));
            return Convert.ToBase64String(rsaEngine.ProcessBlock(encodedDataToEncrypt, 0, encodedDataToEncrypt.Length));
        }

        private RsaKeyParameters GetRsaKeyParameters()
        {
            var decodedPublicKeyString = Convert.FromBase64String(_publicKeyString);
            var inputStream = new Asn1InputStream(decodedPublicKeyString);
            var derObject = inputStream.ReadObject();
            var keyStruct = RsaPublicKeyStructure.GetInstance(derObject);
            return new RsaKeyParameters(false, keyStruct.Modulus, keyStruct.PublicExponent);
        }
    }
}
