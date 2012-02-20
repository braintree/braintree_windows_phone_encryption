using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BraintreeEncryption.Library.Tests
{
    [TestClass]
    [Tag("AesTests")]
    public class AesTest
    {
        private Aes _aes;

        [TestInitialize]
        public void SetUp()
        {
            _aes = new Aes();
        }

        [TestMethod]
        public void AesKeyIs32BytesInLength()
        {
            var aesKey = _aes.GenerateKey();
            Assert.AreEqual(32, aesKey.Length);
        }

        [TestMethod]
        public void AesKeysAreUnique()
        {
            var aesKey1 = _aes.GenerateKey();
            var aesKey2 = _aes.GenerateKey();
            Assert.AreNotEqual(aesKey1, aesKey2);
        }

        [TestMethod]
        public void IVIs16BytesInLength()
        {
            var iv = _aes.GenerateIV();
            Assert.AreEqual(16, iv.Length);
        }

        [TestMethod]
        public void IVsAreUnique()
        {
            var iv1 = _aes.GenerateIV();
            var iv2 = _aes.GenerateIV();
            Assert.AreNotEqual(iv1, iv2);
        }

        [TestMethod]
        public void AesEncryptedSize()
        {
            var aesKey = _aes.GenerateKey();
            var encryptedData = _aes.Encrypt("test data", aesKey);
            Assert.AreEqual(44, encryptedData.Length);
            Assert.AreEqual("=", encryptedData.Substring(43, 1));
            Assert.AreNotEqual("=", encryptedData.Substring(42, 1));
        }

        [TestMethod]
        public void AesEncryptionWithKnownIVAndKey()
        {
            var aesKey     = Convert.FromBase64String("iz5DQzn/XpwXvZ7wY3OGQRVBZTFeVMrEIUljWrIr2Pg=");
            var initVector = Convert.FromBase64String("AAAAAQAAAAIAAAADAAAABA==");
            var encrypted  = _aes.EncryptWithIv("test data", aesKey, initVector);
            Assert.AreEqual("AAAAAQAAAAIAAAADAAAABJcSo857BMv+cJtJfpF5Pak=", encrypted);
        }

        [TestMethod]
        public void AesRoundTrip()
        {
            var aesKey = _aes.GenerateKey();
            var encrypted = _aes.Encrypt("test data", aesKey);
            Assert.AreEqual("test data", TestHelper.DecryptAes(encrypted, aesKey));
        }
    }
}
