using ZonkaZombies.Characters;

namespace ZonkaZombies.Scenery.Interaction
{
    public interface IInteractor
    {
        void SetUp(object obj);
        void OnEnter(IInteractable interactable);
        void OnExit(IInteractable interactable);
        void OnBegin();
        void OnFinish();
        Character GetCharacter();
        void ForceFinish();
    }
}