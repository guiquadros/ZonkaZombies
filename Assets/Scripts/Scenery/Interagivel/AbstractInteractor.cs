using UnityEngine;

namespace Assets.Scripts.Cenario.Interagivel
{
    public class AbstractInteractor : MonoBehaviour
    {
        [SerializeField]
        private InteractorTypeEnum type;
        public InteractorTypeEnum Type { get { return type; } }

        public void SetType(InteractorTypeEnum type)
        {
            this.type = type;
        }

        //Utility methods
        /// <summary>
        /// Retorna o componente AbstractInteractor contido no GameObject, caso exista.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static AbstractInteractor GetInteractor(GameObject go)
        {
            return go.GetComponent<AbstractInteractor>();
        }
    }
}