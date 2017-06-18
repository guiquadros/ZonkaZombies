using UnityEngine;
using UnityEngine.UI;

namespace ZonkaZombies.UI.Data
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ZonkaZombies/Dialogues/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public DialogueDetails[] DetailsOrdered;
    }
}