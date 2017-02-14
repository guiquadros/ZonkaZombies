using Assets.Scripts.Character;
using Assets.Scripts.Character.Attribute;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Character
{

    [RequireComponent(typeof(RectTransform))]
    public class PC_UIController : MonoBehaviour
    {

        [Header("Dependences")]
        [SerializeField]
        private RectTransform maldicaoRectTransform;
        [SerializeField]
        private Image lifeImage, maldicaoImage;

        [Header("Hack")]
        [SerializeField]
        private CharacterBehaviour behavior;
        [SerializeField]
        private bool selectBehavior;

        private void Start()
        {
            ListenToCharacter(behavior);
        }

        private void ListenToCharacter(CharacterBehaviour behaviour)
        {
            if (behavior == null)
                return;

            Attribute lifeAttribute = behaviour.AttributePOOL.GetAttribute(AttributeType.Life);
            lifeAttribute.OnAttributeValueChanged += OnLifeChanged;

            Attribute maldicaoAttribute = behaviour.AttributePOOL.GetAttribute(AttributeType.Maldicao);
            maldicaoAttribute.OnAttributeValueChanged += OnMaldicaoChanged;

            //Inicializa os valores na UI
            lifeImage.fillAmount = lifeAttribute.Current / lifeAttribute.Max;
            maldicaoImage.fillAmount = maldicaoAttribute.Current / maldicaoAttribute.Max;

            Vector3 euler = maldicaoRectTransform.eulerAngles;
            euler.z = -(lifeImage.fillAmount * 360);
            maldicaoRectTransform.DOLocalRotate(euler, 0);

        }

        private void OnLifeChanged(Attribute attribute, float oldValue)
        {
            //Atualiza a rotação no eixo Z da barra de maldição
            Vector3 euler = maldicaoRectTransform.eulerAngles;
            euler.z = -(attribute.Current / attribute.Max * 360);
            maldicaoRectTransform.DOLocalRotate(euler, .3f);

            //Atualiza o tamanho da barra de vida
            lifeImage.DOFillAmount(attribute.Current / attribute.Max, .3f);
        }

        private void OnMaldicaoChanged(Attribute attribute, float oldValue)
        {
            //Atualiza o valor tendo como base o valor atual da barra de vida
            float newValue = Mathf.Clamp(attribute.Current / attribute.Max, 0, lifeImage.fillAmount);
            attribute.Current = newValue;

            //Atualiza o tamanho da barra de maldição
            maldicaoImage.DOFillAmount(newValue, .3f);
        }

        private void Update()
        {
            if (selectBehavior)
            {
                selectBehavior = false;

                ListenToCharacter(behavior);
            }
        }
    }

}