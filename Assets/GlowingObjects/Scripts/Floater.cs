using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField, Range(0, 3)]
    private float _maxYOffset = 1.5f;

    private float _baseYPos;

    private void Start()
    {
        _baseYPos = transform.position.y;
    }

    private void Update ()
    {
        Vector3 newPos = transform.position;
        newPos.y = _baseYPos + Mathf.Sin(Time.time) * _maxYOffset;
        transform.position = newPos;
    }
}
