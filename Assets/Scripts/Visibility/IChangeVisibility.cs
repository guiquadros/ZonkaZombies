using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Interface que possuir os métodos básicos necessários para realizar a troca de visibilidade em objetos.
    /// </summary>
    public class IChangeVisibility : MonoBehaviour {

        protected virtual void Update() {}

        /// <summary>
        /// Torna o componente Renderer deste objeto visível.
        /// </summary>
        public virtual void TurnVisible() {}

        /// <summary>
        /// Torna o componente Renderer deste objeto invisível.
        /// </summary>
        public virtual void TurnInvisible() {}

    }
}
