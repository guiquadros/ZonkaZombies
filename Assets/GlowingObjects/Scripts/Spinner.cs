using UnityEngine;

namespace GlowingObjects.Scripts
{
    public class Spinner : MonoBehaviour
    {
        public Vector3 EulersPerSecond;

        void Update()
        {
            transform.Rotate(EulersPerSecond * Time.deltaTime);
        }
    }
}
