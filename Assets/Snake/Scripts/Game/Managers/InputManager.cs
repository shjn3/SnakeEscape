namespace Shine.EscapeSnake.GamePlay
{

    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;
    public interface IInputManager
    {
        public void RegisterDragEvent(System.Action<Vector3> onDrag);
        public void UnregisterDragEvent(System.Action<Vector3> onDrag);

    }
    public class InputManager : MonoBehaviour, IInputManager
    {
        private bool isPressed;
        private Vector2 currentPosition;
        private Vector2 delta;

        private event Action OnBeginDrag;
        private event Action<Vector3> OnDrag;
        private event Action OnEndDrag;

        public void RegisterDragEvent(Action<Vector3> _onDrag)
        {
            OnDrag += _onDrag;
        }

        public void UnregisterDragEvent(Action<Vector3> _onDrag)
        {
            OnDrag -= _onDrag;
        }

        public void Update()
        {
            var _pointer = Pointer.current;
            if (!isPressed)
            {
                if (_pointer.press.wasPressedThisFrame)
                {
                    isPressed = true;
                    currentPosition = _pointer.position.ReadValue();
                    delta = Vector2.zero;
                    OnBeginDrag?.Invoke();
                }
            }
            else
            {
                if (_pointer.press.wasReleasedThisFrame)
                {
                    isPressed = false;
                    currentPosition = Vector2.zero;
                    delta = Vector2.zero;
                    OnEndDrag?.Invoke();
                }
                else
                {
                    var nextPostion = _pointer.position.ReadValue();
                    var worldDelta = Camera.main.ScreenToWorldPoint(nextPostion) - Camera.main.ScreenToWorldPoint(currentPosition);
                    delta = nextPostion - currentPosition;
                    currentPosition = nextPostion;
                    OnDrag?.Invoke(worldDelta);
                }
            }
        }
    }
}