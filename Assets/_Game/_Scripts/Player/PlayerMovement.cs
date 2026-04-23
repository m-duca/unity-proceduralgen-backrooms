using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Backrooms
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Walk")]
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;

        [Header("References")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private CameraHeadBob _cameraHeadBob;

        [Header("SFX")]
        [SerializeField] private EventReference _footstepsEventRef;
        [SerializeField] private float _footstepsInterval;

        // Not serialized
        private Vector2 _moveInput = Vector2.zero;
        private Vector3 _curSpeed = Vector3.zero;
        private bool _canPlayFootsteps = true;

        private void OnValidate() => _characterController = GetComponent<CharacterController>();

        private void OnEnable() => SetInputs(true);

        private void OnDisable() => SetInputs(false);

        private void Update()
        {
            ApplyMovement();
            HandleBob();
            HandleFootsteps();
        }

        #region Inputs

        private void SetInputs(bool value)
        {
            if (value)
            {
                _moveAction.action.performed += GetMoveInput;
                _moveAction.action.canceled += ResetMoveInput;
            }
            else
            {
                _moveAction.action.performed -= GetMoveInput;
                _moveAction.action.canceled -= ResetMoveInput;
            }
        }

        private void GetMoveInput(InputAction.CallbackContext contextValue) => _moveInput = contextValue.ReadValue<Vector2>();
        private void ResetMoveInput(InputAction.CallbackContext contextValue) => _moveInput = Vector2.zero;

        #endregion

        #region Physics

        private void ApplyMovement()
        {
            Vector3 targetHorizontalSpeed = (transform.right * _moveInput.x + transform.forward * _moveInput.y) * _maxSpeed;

            if (targetHorizontalSpeed.magnitude > 0.1f)
                _curSpeed = Vector3.Lerp(_curSpeed, targetHorizontalSpeed, _acceleration * Time.deltaTime);
            else
                _curSpeed = Vector3.Lerp(_curSpeed, Vector3.zero, _deceleration * Time.deltaTime);

            _characterController.Move(_curSpeed * Time.deltaTime);
        }

        public bool IsMoving()
        {
            return _curSpeed.magnitude > 0.01f;
        }

        private void HandleBob()
        {
            if (IsMoving())
                _cameraHeadBob.StartHeadBob();
            else
                _cameraHeadBob.StopHeadBob();
        }

        #endregion

        #region SFX

        private void HandleFootsteps()
        {
            if (IsMoving() && _canPlayFootsteps)
                PlayFootsteps();
        }

        private void PlayFootsteps()
        {
            AudioManager.Instance.PlayOneShot(_footstepsEventRef, transform.position);
            _canPlayFootsteps = false;
            Invoke(nameof(ResetCanPlayFootsteps), _footstepsInterval);
        }

        private void ResetCanPlayFootsteps() => _canPlayFootsteps = true;

        #endregion
    }
}
