using UnityEngine;

namespace Assets.Scripts.Cenario.Interagivel
{
    public class IChangeMaterialColor : MonoBehaviour {

        private new Renderer renderer;
        private Color defaultColor;
        private bool isHighlighted = false;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            defaultColor = renderer.material.color;
        }

        private void OnMouseDown()
        {
            if (isHighlighted)
                DeHighlight();
            else
                Highlight();

            isHighlighted = !isHighlighted;
        }

        private void Highlight()
        {
            renderer.material.color = Color.yellow;
        }

        private void DeHighlight()
        {
            renderer.material.color = defaultColor;
        }

    }
}
