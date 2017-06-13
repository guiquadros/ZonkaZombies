using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZonkaZombies.Util.Commands
{
    [Serializable]
    public class CommandList : BaseCommand
    {
        [SerializeField]
        private List<BaseCommand> _commands;

        public override void Execute()
        {
            _commands.ForEach(c => c.Execute());
        }
    }
}