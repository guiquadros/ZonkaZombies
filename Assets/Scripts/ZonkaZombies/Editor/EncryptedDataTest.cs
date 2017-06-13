using NUnit.Framework;
using ZonkaZombies.Util.Security;

namespace ZonkaZombies.Editor
{
    public class EncryptedDataTest
    {
        private const string Key = "41as6d468a1d38w48";
        private readonly EncryptedData _data = new XorEncryptedData(Key);

        [Test]
        public void EncryptInt()
        {
            for (int i = -1000; i < 1000; i++)
            {
                _data.Set(i);
                Assert.AreEqual(i, _data.GetInt());
            }
        }

        [Test]
        public void EncryptFloat()
        {
            for (float f = 0f; f < 0.001f; f += 0.000001f)
            {
                _data.Set(f);
                Assert.AreEqual(f, _data.GetFloat());
            }
        }

        [Test]
        public void EncryptLong()
        {
            for (long l = -100000000; l < 100000000; l += 10000000)
            {
                _data.Set(l);
                Assert.AreEqual(l, _data.GetLong());
            }
        }

        [Test]
        public void EncryptDecimal()
        {
            for (decimal d = -100M; d < 100M; d += 0.01M)
            {
                _data.Set(d);
                Assert.AreEqual(d, _data.GetDecimal());
            }
        }

        [Test]
        public void EncryptString()
        {
            string value = "ThisIsATest;HelloWorld!**&%*&!(4}}][f'.'132446518.}{_+=-";
            _data.Set(value);
            Assert.AreEqual(value, _data.GetString());
        }

        [Test]
        public void FloatCalculations()
        {
            float value = 5.0000536895f;
            _data.Set(value);
            float value2 = _data.GetFloat();
            Assert.AreEqual(value, value2);

            value2 -= 599465321.5483152f;
            _data.Set(value2);
            Assert.AreEqual(value2, _data.GetFloat());
        }
    }
}