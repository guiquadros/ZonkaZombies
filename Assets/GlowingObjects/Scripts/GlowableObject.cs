using System.Collections.Generic;
using UnityEngine;

namespace GlowingObjects.Scripts
{
    public class GlowableObject : MonoBehaviour
    {
        [SerializeField]
        private Color _glowColor = Color.yellow;
        [SerializeField, Range(1, 20)]
        private float _lerpFactor = 8;

        private readonly List<Material> _materials = new List<Material>();
        private Color _currentColor;
        private Color _targetColor;

        private void Start()
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                _materials.AddRange(renderer.materials);
            }
        }

        /// <summary>
        /// Loop over all cached materials and update their color, disable self if we reach our target color.
        /// </summary>
        private void Update()
        {
            _currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * _lerpFactor);

            foreach (Material t in _materials)
            {
                t.SetColor("_GlowColor", _currentColor);
            }

            if (_currentColor.Equals(_targetColor))
            {
                enabled = false;
            }
        }

        public void Glow(bool state)
        {
            _targetColor = state ? _glowColor : Color.black;
            enabled = true;
        }
    }
}
