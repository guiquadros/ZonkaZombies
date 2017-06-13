using UnityEngine;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenery.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class TransparentObject : MonoBehaviour
    {
        [SerializeField]
        private Material _transparentMaterial;

        private Material _originalMaterial;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _originalMaterial = _renderer.material;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagConstants.PLAYER))
            {
                _renderer.material = _transparentMaterial;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagConstants.PLAYER))
            {
                _renderer.material = _originalMaterial;
            }
        }
    }
}
