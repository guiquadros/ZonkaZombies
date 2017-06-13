using UnityEngine;

namespace ZonkaZombies.Scenery.Interaction
{
    public interface IInteractable
    {
        /// <summary>
        /// Called when a character ENTER the trigger.
        /// </summary>
        void OnAwake();
        /// <summary>
        /// Called when a player wants to interact with this interactable.
        /// </summary>
        /// <param name="interactor">The character that wants to interact with this Interactable.</param>
        void OnBegin(IInteractor interactor);
        /// <summary>
        /// Called when a character gets out from the trigger after the interaction starts OR when the player cancells the interaction.
        /// </summary>
        /// <param name="interactor">The character that wants to interact with this Interactable.</param>
        void OnFinish(IInteractor interactor);
        /// <summary>
        /// Called when a character EXIT the trigger.
        /// </summary>
        void OnSleep();
        GameObject GetGameObject();
    }
}