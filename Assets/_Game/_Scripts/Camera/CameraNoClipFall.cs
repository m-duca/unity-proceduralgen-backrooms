using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Backrooms
{
    public class CameraNoClipFall : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _fallDistance;
        [SerializeField] private float _fallDuration;
        [SerializeField] private float _recoverDuration;

        [Header("References")]
        [SerializeField] private CameraRotation _cameraRotation;

        // Not serialized
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Sequence _fallSequence;

        private void Awake()
        {
            _originalPosition = transform.localPosition;
            _originalRotation = transform.localRotation;
        }

        private void Start() => PlayNoclipFall();

        public void PlayNoclipFall()
        {
            _fallSequence?.Kill();

            _cameraRotation.enabled = false;

            Transform camTransform = transform;

            camTransform.localPosition = _originalPosition;
            camTransform.localRotation = _originalRotation;

            _fallSequence = DOTween.Sequence();

            // queda + rotação brusca
            _fallSequence.Append(
                camTransform.DOLocalMoveY(
                    _originalPosition.y - _fallDistance,
                    _fallDuration
                ).SetEase(Ease.InQuad)
            );

            _fallSequence.Join(
                camTransform.DOLocalRotate(
                    new Vector3(360f, 0f, 0f),
                    _fallDuration,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.OutExpo)
            );

            // impacto
            _fallSequence.Append(
                camTransform.DOShakePosition(0.12f, 0.06f, 10)
            );

            // volta instantaneamente para a posição original
            _fallSequence.AppendCallback(() =>
            {
                camTransform.localPosition = _originalPosition;
                camTransform.localRotation = _originalRotation;

                // reativa a câmera logo após o roll
                _cameraRotation.enabled = true;
            });
        }
    }
}