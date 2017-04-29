using UnityEngine;

namespace ZonkaZombies.Util.Commands
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