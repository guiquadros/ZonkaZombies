using System.Collections.Generic;
using UnityEngine;

namespace ZonkaZombies.Util
{
    public class CommandList : ICommand
    {
        [SerializeField]
        private List<ICommand> _commands;

        public void Execute()
        {
            _commands.ForEach(c => c.Execute());
        }
    }
}