using UnityEngine;
using UnityEngine.EventSystems;
namespace Shine.EscapeSnake.GamePlay.Snake
{
    public class SnakeSegment : MonoBehaviour, IPointerDownHandler
    {
        [HideInInspector] public int id;
        [SerializeField] Collider2D _collider;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public event System.Action _onClickedCallback;
        public void UpdateVisuals(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void UpdatePosition(Vector2 position, Vector2 direction)
        {

            var eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90);
            Quaternion rotation = Quaternion.Euler(eulerAngles);
            transform.SetPositionAndRotation(position, rotation);
        }

        public void AddListener(System.Action listener)
        {
            _onClickedCallback += listener;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onClickedCallback?.Invoke();
        }
    }
}