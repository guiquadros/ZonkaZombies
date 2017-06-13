namespace ZonkaZombies.Util.Security
{
    public sealed class XorAlgorithm : IEncryptAlgorithm
    {
        private readonly string _key;
        //private static readonly Encoding Encoding = Encoding.Unicode;

        public XorAlgorithm(string key)
        {
            _key = key;
        }

        public object Process(object value)
        {
            string strValue = value.ToString();
            char[] cipher = new char[strValue.Length];
            for (int i = 0; i < cipher.Length; i++)
            {
                cipher[i] = (char)(strValue[i] ^ _key[i % _key.Length]);
            }
            return new string(cipher);
        }
    }
}