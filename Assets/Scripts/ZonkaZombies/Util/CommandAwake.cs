using UnityEngine;

namespace ZonkaZombies.Util
{
    public class CommandAwake : MonoBehaviour, ICommand
    {
        private void Awake()
        {
            Execute();
        }

        public void Execute()
        {
            //TODO
        }
    }
}