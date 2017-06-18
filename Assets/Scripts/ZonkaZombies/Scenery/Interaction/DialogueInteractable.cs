using GlowingObjects.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Managers;
using ZonkaZombies.UI;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenery.Interaction
{
    [RequireComponent(typeof(DialogueHandler))]
    public class DialogueInteractable : InteractableBase
    {
        private DialogueHandler _dialogueHandler;

        private void Awake()
        {
            _dialogueHandler = GetComponent<DialogueHandler>();
        }

        public override void OnBegin(IInteractor interactor)
        {
            _dialogueHandler.StartDialogue();
        }

        public override void OnFinish(IInteractor interactor)
        {
            
        }
    }
}