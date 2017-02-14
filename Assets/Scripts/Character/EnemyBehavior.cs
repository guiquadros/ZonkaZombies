using Assets.Scripts.Cenario.Room.Node;

namespace Assets.Scripts.Character
{

    public class EnemyBehavior : CharacterBehaviour
    {
        public enum EnemyState
        {
            Sleeping,
            Player_Spotted
        }

        public EnemyState myCurrentState = EnemyState.Sleeping;

        private Node currentObjective;

        private void Update()
        {
            switch (myCurrentState)
            {
                case EnemyState.Sleeping:

                    break;

                case EnemyState.Player_Spotted:
                    if (currentObjective == null)
                    {
                        //CharacterMovementController.Handler.MoveTo(currentObjective);
                    }

                    break;
            }
        }
    }
}