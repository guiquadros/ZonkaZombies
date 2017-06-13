using System;

namespace ZonkaZombies.Util.Security
{
    public class EncryptedData
    {
        private object _value;
        private readonly IEncryptAlgorithm _encryptAlgorithm;

        public EncryptedData(IEncryptAlgorithm algorithm)
        {
            _encryptAlgorithm = algorithm;
        }

        public void Set(object value)
        {
            string valueAsString = value is float ? ((float)value).ToString("R") : value.ToString();
            _value = _encryptAlgorithm.Process(valueAsString);
        }

        public object Get()
        {
            return _value;
        }

        public string GetString()
        {
            return (string)_encryptAlgorithm.Process(_value);
        }

        public int GetInt()
        {
            return Convert.ToInt32(GetString());
        }

        public float GetFloat()
        {
            return Convert.ToSingle(GetString());
        }

        public long GetLong()
        {
            return Convert.ToInt64(GetString());
        }

        public short GetShort()
        {
            return Convert.ToInt16(GetString());
        }

        public decimal GetDecimal()
        {
            return Convert.ToDecimal(GetString());
        }
    }
}