using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeMaterialTiling : MonoBehaviour
{
    public float xMultiplier = 1.0f;
    public float yMultiplier = 1.0f;

	private void Awake ()
    {
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(xMultiplier, yMultiplier);
    }
}
