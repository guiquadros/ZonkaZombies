using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Input;
using ZonkaZombies.UI.Data;
using ZonkaZombies.Util;

namespace ZonkaZombies.UI.Dialogues
{
    public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
    {
        [SerializeField]
        private GameObject _dialogueUIGameObject;

        [SerializeField]
        private Image _mugshot;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private float _timeBetweenDialogues = 0.5f;

        [SerializeField]
        private float _waitTimeStartDialogue = 1f;

        private int _currentDiallogTextIndex = 0;
        private int _currentDiallogDetailsIndex = 0;
        private bool _dialogueStarted = false;
        private bool _nextSentence;
        private Data.Dialogue _dialogue;
        private bool _isPlayer2Dialogue = false;

        public event Action<Data.Dialogue, bool> DialogueFinished;
        public event Action<Data.Dialogue, Transform, bool> DialogueStarted;

        public void Initialize(Data.Dialogue dialogue, bool waitStartDialogue = false, Transform interactableTransform = null, bool freezePlayer = true, bool isPlayer2Dialogue = false)
        {
            if (_dialogueStarted)
            {
                Debug.Log("A dialog is already in place. This dialog will not be initialized.");
                return;
            }

            _isPlayer2Dialogue = isPlayer2Dialogue;

            _dialogue = dialogue;
            _waitTimeStartDialogue = waitStartDialogue ? _waitTimeStartDialogue : 0f;

            _currentDiallogTextIndex = 0;
            _currentDiallogDetailsIndex = 0;

            StartDialogueCoroutine(_dialogue.DetailsOrdered, interactableTransform, freezePlayer);
        }

        private void Update()
        {
            if (_dialogueStarted)
            {
                VerifyInputForNextSentence();
            }
        }

        private void StartDialogueCoroutine(DialogueDetails[] dialogueDetailsListOrdered, Transform interactableTransform, bool freezePlayer)
        {
            StartCoroutine(DialogueCoroutine(dialogueDetailsListOrdered, interactableTransform, freezePlayer));
        }

        private IEnumerator DialogueCoroutine(DialogueDetails[] dialogueDetailsListOrdered, Transform interactableTransform, bool freezePlayer)
        {
            if (_waitTimeStartDialogue > 0f)
            {
                yield return new WaitForSeconds(_waitTimeStartDialogue);
            }

            _dialogueStarted = true;

            if (DialogueStarted != null)
            {
                DialogueStarted(_dialogue, interactableTransform, freezePlayer);
            }

            _dialogueUIGameObject.SetActive(true);

            SetDialogueTextAndImage(dialogueDetailsListOrdered);

            while (_currentDiallogDetailsIndex + 1 <= dialogueDetailsListOrdered.Length)
            {
                if (_nextSentence)
                {
                    if (dialogueDetailsListOrdered[_currentDiallogDetailsIndex].DialogueText.Length == _currentDiallogTextIndex + 1)
                    {
                        _currentDiallogDetailsIndex++;
                        _currentDiallogTextIndex = 0;

                        _dialogueUIGameObject.SetActive(false);

                        yield return new WaitForSeconds(_timeBetweenDialogues);

                        _dialogueUIGameObject.SetActive(true);
                    }
                    else
                    {
                        _currentDiallogTextIndex++;
                    }

                    if (_currentDiallogDetailsIndex + 1 > dialogueDetailsListOrdered.Length)
                    {
                        _dialogueStarted = false;
                        _dialogueUIGameObject.SetActive(false);

                        if (DialogueFinished != null)
                        {
                            DialogueFinished(_dialogue, freezePlayer);
                        }
                    }
                    else
                    {
                        SetDialogueTextAndImage(dialogueDetailsListOrdered);
                    }
                }

                yield return null;
            }
        }

        private void SetDialogueTextAndImage(DialogueDetails[] dialogueDetailsListOrdered)
        {
            DialogueDetails dialogueDetails = dialogueDetailsListOrdered[_currentDiallogDetailsIndex];
            
            _mugshot.sprite = _isPlayer2Dialogue && dialogueDetails.IsPlayerDialogue ? dialogueDetails.AlternativeMugshotImage : dialogueDetails.MugshotImage;
            _text.text = dialogueDetails.DialogueText[_currentDiallogTextIndex];
        }

        private void VerifyInputForNextSentence()
        {
            _nextSentence = PlayerInput.InputReaderController1.ADown() || PlayerInput.InputReaderController2.ADown() || PlayerInput.InputReaderKeyboard.ADown();
        }
    }
}
