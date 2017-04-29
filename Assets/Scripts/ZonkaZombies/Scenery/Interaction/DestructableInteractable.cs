namespace ZonkaZombies.Scenery.Interaction
{
    public class DestructableInteractable : InteractableGlowable
    {
        public override void OnBegin(IInteractor interactor)
        {
            OnFinish();
        }

        public override void OnFinish()
        {
            Destroy(gameObject);
        }
    }
}