using UnityEngine;

namespace Assets.Scripts.Cenario.Interagivel
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class HighlighterHandler : MonoBehaviour {

        private bool isHighlighted = false;
        public bool IsHighlighted() { return isHighlighted; }

        protected virtual void Awake()
        {
        
        }

        /// <summary>
        /// Realiza o highlight no objeto.
        /// </summary>
        public void Highlight()
        {
        
        }

        /// <summary>
        /// Remove o highlight do objeto.
        /// </summary>
        public void Dehilight()
        {

        }

    }
}
