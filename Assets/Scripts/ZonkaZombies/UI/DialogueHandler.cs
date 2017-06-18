using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Input;
using ZonkaZombies.UI.Data;

namespace ZonkaZombies.UI
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField]
        private Image _mugshot;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private DialogueDetails[] _dialogueDetailsListOrdered;

        [SerializeField]
        private float _timeBetweenDialogues = 0.5f;

        [SerializeField]
        private float _waitTimeStartDialogue = 1f;

        [SerializeField]
        private bool _nextSentence;

        [SerializeField]
        private GameObject _dialogueComponents;

        private int _currentDiallogTextIndex = 0;
        private int _currentDiallogDetailsIndex = 0;

        public event Action DialogueFinished;

        private void Start ()
        {
            StartCoroutine(DialogueCoroutine());
        }

        private void Update ()
        {
            NextSentence();
        }

        private IEnumerator DialogueCoroutine()
        {
            yield return new WaitForSeconds(_waitTimeStartDialogue);

            _dialogueComponents.SetActive(true);
            _mugshot.sprite = _dialogueDetailsListOrdered[_currentDiallogDetailsIndex].MugshotImage;
            _text.text = _dialogueDetailsListOrdered[_currentDiallogDetailsIndex].DialogueText[_currentDiallogTextIndex];

            while (_currentDiallogDetailsIndex + 1 <= _dialogueDetailsListOrdered.Length)
            {
                if (_nextSentence)
                {
                    if (_dialogueDetailsListOrdered[_currentDiallogDetailsIndex].DialogueText.Length == _currentDiallogTextIndex + 1)
                    {
                        _currentDiallogDetailsIndex++;
                        _currentDiallogTextIndex = 0;

                        _dialogueComponents.SetActive(false);

                        yield return new WaitForSeconds(_timeBetweenDialogues);

                        _dialogueComponents.SetActive(true);
                    }
                    else
                    {
                        _currentDiallogTextIndex++;
                    }

                    if (_currentDiallogDetailsIndex + 1 > _dialogueDetailsListOrdered.Length)
                    {
                        _dialogueComponents.SetActive(false);

                        if (DialogueFinished != null)
                        {
                            DialogueFinished();
                        }
                    }
                    else
                    {
                        _mugshot.sprite = _dialogueDetailsListOrdered[_currentDiallogDetailsIndex].MugshotImage;
                        _text.text = _dialogueDetailsListOrdered[_currentDiallogDetailsIndex].DialogueText[_currentDiallogTextIndex];
                    }
                }

                yield return null;
            }
        }

        public bool NextSentence()
        {
            _nextSentence = PlayerInput.InputReaderController1.ADown() || PlayerInput.InputReaderController2.ADown() || PlayerInput.InputReaderKeyboard.ADown();
            return _nextSentence;
        }
    }
}
