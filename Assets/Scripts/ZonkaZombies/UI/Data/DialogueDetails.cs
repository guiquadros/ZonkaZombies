using UnityEngine;
using UnityEngine.UI;

namespace ZonkaZombies.UI.Data
{
    [CreateAssetMenu(fileName = "DialogueDetails", menuName = "ZonkaZombies/Dialogues/DialogueDetails")]
    public class DialogueDetails : ScriptableObject
    {
        public Sprite MugshotImage;
        public string[] DialogueText;
    }
}