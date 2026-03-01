using UnityEngine;
namespace Shine.EscapeSnake.Utils
{
    public partial class Utils
    {
        public static Quaternion GetSnakeParkQuaternion(Vector2 direction)
        {
            return Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90);

        }

        public static Vector2Int Normalized(Vector2 source)
        {
            if (Mathf.Abs(source.x) > Mathf.Abs(source.y))
            {
                return new Vector2Int((int)Mathf.Sign(source.x), 0);
            }

            return new Vector2Int(0, (int)Mathf.Sign(source.y));
        }

    }
}