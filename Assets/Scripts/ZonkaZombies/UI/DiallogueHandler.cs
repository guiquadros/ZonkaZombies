﻿using System;
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

        private int _currentDiallogTextIndex = 0;
        private int _currentDiallogDetailsIndex = 0;

        public event Action DiallogueFinished;

        private void Start ()
        {
            StartDiallogue();
        }

        public void StartDiallogue()
        {
            gameObject.SetActive(true);
            _mugshot.sprite = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].MugshotImage;
            _text.text = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].DiallogueText[_currentDiallogTextIndex];
        }
        
        private void Update ()
        {
            if (!NextSentence() || _currentDiallogDetailsIndex + 1 > _diallogueDetailsListOrdered.Length) return;

            if (_diallogueDetailsListOrdered[_currentDiallogDetailsIndex].DiallogueText.Length == _currentDiallogTextIndex + 1)
            {
                _currentDiallogDetailsIndex++;
                _currentDiallogTextIndex = 0;
            }
            else
            {
                _currentDiallogTextIndex++;
            }

            if (_currentDiallogDetailsIndex + 1 > _diallogueDetailsListOrdered.Length)
            {
                gameObject.SetActive(false);

                if (DiallogueFinished != null)
                {
                    DiallogueFinished();
                }

                return;
            }

            _mugshot.sprite = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].MugshotImage;
            _text.text = _diallogueDetailsListOrdered[_currentDiallogDetailsIndex].DiallogueText[_currentDiallogTextIndex];
        }

        public bool NextSentence()
        {
            return PlayerInput.InputReaderController1.ADown() || PlayerInput.InputReaderController2.ADown() || PlayerInput.InputReaderKeyboard.ADown();
        }
    }
}
