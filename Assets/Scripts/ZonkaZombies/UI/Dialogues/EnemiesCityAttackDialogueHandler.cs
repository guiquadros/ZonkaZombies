using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;

namespace ZonkaZombies.UI.Dialogues
{
    public class EnemiesCityAttackDialogueHandler : DialogueHandler
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            MessageRouter.AddListener<ForceEnemyPursuitMode>(OnForceEnemyPursuitMode);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            MessageRouter.RemoveListener<ForceEnemyPursuitMode>(OnForceEnemyPursuitMode);
        }

        private void OnForceEnemyPursuitMode(ForceEnemyPursuitMode obj)
        {
            DialogueManager.Instance.Initialize(Dialogue, freezePlayer: false);
        }
    }
}
