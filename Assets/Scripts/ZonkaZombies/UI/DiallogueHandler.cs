using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZonkaZombies.Input;
using ZonkaZombies.UI.Data;

namespace ZonkaZombies.UI
{
    public class DiallogueHandler : MonoBehaviour
    {
        [SerializeField]
        private Image _mugshot;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private DiallogueDetails[] _diallogueDetailsListOrdered;

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

        public event Action DiallogueFinished;

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
            _mugshot.sprite = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].MugshotImage;
            _text.text = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].DiallogueText[_currentDiallogTextIndex];

            while (_currentDiallogDetailsIndex + 1 <= _diallogueDetailsListOrdered.Length)
            {
                if (_nextSentence)
                {
                    if (_diallogueDetailsListOrdered[_currentDiallogDetailsIndex].DiallogueText.Length == _currentDiallogTextIndex + 1)
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

                    if (_currentDiallogDetailsIndex + 1 > _diallogueDetailsListOrdered.Length)
                    {
                        _dialogueComponents.SetActive(false);

                        if (DiallogueFinished != null)
                        {
                            DiallogueFinished();
                        }
                    }
                    else
                    {
                        _mugshot.sprite = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].MugshotImage;
                        _text.text = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].DiallogueText[_currentDiallogTextIndex];
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
