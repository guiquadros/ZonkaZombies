namespace ZonkaZombies.Util.Security
{
    public sealed class XorEncryptedData : EncryptedData
    {
        public XorEncryptedData(string key) : base(new XorAlgorithm(key)) { }
    }
}