namespace ZonkaZombies.Util.Security
{
    public interface IEncryptAlgorithm
    {
        object Process(object value);
    }
}