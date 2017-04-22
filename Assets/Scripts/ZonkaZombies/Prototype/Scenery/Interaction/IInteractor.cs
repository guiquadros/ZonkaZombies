using ZonkaZombies.Prototype.Characters;

namespace ZonkaZombies.Prototype.Scenery.Interaction
{
    public interface IInteractor
    {
        void OnEnter(IInteractable interactable);
        void OnExit(IInteractable interactable);
        void OnFinish();
        void OnBegin();
        Character GetCharacter();
    }
}