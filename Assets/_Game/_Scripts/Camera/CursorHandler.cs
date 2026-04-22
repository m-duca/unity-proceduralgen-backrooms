using UnityEngine;

namespace Backrooms
{
    public class CursorHandler : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private bool _isShowing;

        private void OnValidate()
        {
            if (_isShowing) ShowCursor();
            else HideCursor();
        }

        private void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;    
        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
