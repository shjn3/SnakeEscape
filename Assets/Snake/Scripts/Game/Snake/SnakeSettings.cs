
namespace Shine.EscapeSnake.GamePlay.Snake.Settings
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Snake settings", menuName = "ShineSnake/Settings/Snake")]
    public class SnakeSettings : ScriptableObject
    {
        [SerializeField] float speed = 2;
        public float Speed { get => speed <= 0 ? 1 : speed; }
    }
}