using Assets.Scripts.Character;
using Assets.Scripts.Character.Attribute;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Character
{

    public class NPCHandler : MonoBehaviour
    {

        [Header("Dependencies")]
        [SerializeField]
        private NPCPrismasHandler prismasHandler;
        [SerializeField]
        private Image lifeSlider;
        [SerializeField]
        private Text nameText;

        [Header("Hack")]
        [SerializeField]
        private CharacterBehaviour nextSelected;
        [SerializeField]
        private bool selectNext;

        //Private Variables
        private CharacterBehaviour selectedCharacterBehaviour;

        private void Start()
        {
            Select(nextSelected);
        }

        /// <summary>
        /// Atualiza a UI do NPC com base no personagem selecionado. Este método de alto nível é reponsável por
        /// alterar a imagem, nome, barra de vida e de maldição e os prismas, da UI.
        /// </summary>
        /// <param name="character"></param>
        public void Select(CharacterBehaviour character)
        {
            if (character == null)
                return;

            DeselectLastCharacter();

            //Adiciona ouvintes aos eventos
            character.AttributePOOL.GetAttribute(AttributeType.Life).OnAttributeValueChanged += 
                OnLifeValueChanged;

            OnLifeValueChanged(character.AttributePOOL.GetAttribute(AttributeType.Life), 0);

            //Atualiza a quantidade de prismas
            prismasHandler.ShowPrismas();

            //
            selectedCharacterBehaviour = character;
        }

        /// <summary>
        /// Deixa de ouvir os eventos que estão sendo ouvidos pelo último NPC selecionado.
        /// </summary>
        private void DeselectLastCharacter()
        {
            if (selectedCharacterBehaviour == null)
                return;

            //Remove os ouvintes adicionados anteriormente
            selectedCharacterBehaviour.AttributePOOL.GetAttribute(AttributeType.Life).
                OnAttributeValueChanged -= OnLifeValueChanged;
        }

        /// <summary>
        /// Atualiza a barra de vida do personagem que está sendo observado.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnLifeValueChanged(Attribute attribute, float oldValue)
        {
            lifeSlider.DOFillAmount(attribute.Current / attribute.Max, .3f);
        }

        private void Update()
        {
            if (selectNext)
            {
                selectNext = false;

                if (nextSelected == null)
                    return;

                Select(nextSelected);
                nextSelected = null;
            }
        }
    }

}