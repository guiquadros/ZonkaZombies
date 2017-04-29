using UnityEngine;

namespace ZonkaZombies.Scenery.Interaction
{
    public interface IInteractable
    {
        void OnAwake();
        void OnBegin(IInteractor interactor);
        void OnFinish();
        void OnSleep();
        GameObject GetGameObject();
    }
}