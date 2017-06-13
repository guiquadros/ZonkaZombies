using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeMaterialOffset : MonoBehaviour
{
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;

	private void Awake ()
    {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xOffset, yOffset);
    }
}
