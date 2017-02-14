using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Classe responsável por tornar visível/invisível o Renderer contido no gameObject.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class VisibilityHandler : IChangeVisibility {

        [Header("Hack")]
        [SerializeField]
        private bool visible = false;
        [SerializeField]
        private bool invisible = false;

        private Renderer rend;

        private void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        protected override void Update()
        {
            return;

            if (visible)
                TurnVisible();
            else if (invisible)
                TurnInvisible();
        }

        /// <summary>
        /// Torna o componente Renderer deste objeto visível.
        /// </summary>
        public override void TurnVisible()
        {
            visible = false;
            rend.enabled = true;
        }

        /// <summary>
        /// Torna o componente Renderer deste objeto invisível.
        /// </summary>
        public override void TurnInvisible()
        {
            invisible = false;
            rend.enabled = false;
        }

    }
}
