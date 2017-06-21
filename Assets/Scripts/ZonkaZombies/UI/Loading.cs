using DG.Tweening;
using UnityEngine;

namespace ZonkaZombies.UI
{
    public class Loading : MonoBehaviour
    {
        private bool _doLoading = false;

        private void OnEnable()
        {
            _doLoading = true;
        }

        private void OnDisable()
        {
            _doLoading = false;
        }

        private void Update()
        {
            if (_doLoading)
            {
                transform.DORotate(new Vector3(0f, 0f, 540f), 3f, RotateMode.FastBeyond360);
            }
        }
    }
}
