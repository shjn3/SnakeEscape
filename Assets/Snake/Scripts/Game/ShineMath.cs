namespace Shine.EscapeSnake
{
    using UnityEngine;
    public partial class ShineMath
    {
        public static bool RaycastLine(Vector2 origin, Vector2 dir, Vector2 lineStart, Vector2 lineEnd, out Vector2 result)
        {
            result = Vector2.zero;

            Vector2 E = lineEnd - lineStart;
            Vector2 F = dir;
            Vector2 CA = origin - lineStart;
            Vector2 AC = lineStart - origin;

            float crossFE = Cross(F, E);
            if (crossFE == 0) return false;

            var r = Cross(E, CA) / crossFE;
            if (r < 0) return false;

            var s = Cross(AC, F) / crossFE;
            if (s < 0 || s > 1) return false;


            result = lineStart + s * E;
            return true;
        }
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
    }
}