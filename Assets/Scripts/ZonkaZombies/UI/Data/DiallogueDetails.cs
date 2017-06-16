using UnityEngine;
using UnityEngine.UI;

namespace ZonkaZombies.UI.Data
{
    [CreateAssetMenu(fileName = "DiallogueDetails", menuName = "ZonkaZombies/Diallogue/DiallogueDetails")]
    public class DiallogueDetails : ScriptableObject
    {
        public Sprite MugshotImage;
        public string[] DiallogueText;
    }
}