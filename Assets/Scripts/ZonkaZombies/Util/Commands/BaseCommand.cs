using System;
using UnityEngine;

namespace ZonkaZombies.Util.Commands
{
    [Serializable]
    public class BaseCommand : MonoBehaviour
    {
        public virtual void Execute() { }
    }
}