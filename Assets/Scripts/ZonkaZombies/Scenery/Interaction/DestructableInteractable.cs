namespace ZonkaZombies.Scenery.Interaction
{
    public class DestructableInteractable : InteractableGlowable
    {
        public override void OnBegin(IInteractor interactor)
        {
            OnFinish(interactor);
        }

        public override void OnFinish(IInteractor interactor)
        {
            Destroy(gameObject);
        }
    }
}