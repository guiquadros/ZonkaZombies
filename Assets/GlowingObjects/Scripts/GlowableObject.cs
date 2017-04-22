using UnityEngine;
using System.Collections.Generic;

public class GlowableObject : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

	private readonly List<Material> _materials = new List<Material>();
	private Color _currentColor;
	private Color _targetColor;

	private void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in Renderers)
        {
			_materials.AddRange(renderer.materials);
		}
	}

	/// <summary>
	/// Loop over all cached materials and update their color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

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
        _targetColor = state ? GlowColor : Color.black;
        enabled = true;
    }
}
