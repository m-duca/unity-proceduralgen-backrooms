using DG.Tweening;
using UnityEngine;

namespace Backrooms
{
    public class CameraHeadBob : MonoBehaviour
    {
        // Inspector
        [Header("Parameters")]
        [SerializeField] private float _headBobIntensity;
        [SerializeField] private float _headBobSpeed;
        [SerializeField] private float _headBobStopDuration;

        // Not serialized
        private Tween _headBobTween;
        private Vector3 _initialLocalPos;
        private bool _stoppedHeadBob = false;

        private void Start() => _initialLocalPos = transform.localPosition;

        public void StartHeadBob()
        {
            if (_headBobTween != null && _headBobTween.IsActive()) return;

            _headBobTween = transform
                .DOLocalMoveY(_initialLocalPos.y + _headBobIntensity, 1f / _headBobSpeed)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            _stoppedHeadBob = false;
        }

        public void StopHeadBob()
        {
            if (_stoppedHeadBob) return;

            if (_headBobTween != null)
            {
                _headBobTween.Kill();
                _headBobTween = null;
            }

            transform.DOLocalMove(_initialLocalPos, _headBobStopDuration);

            _stoppedHeadBob = true;
        }
    }
}
