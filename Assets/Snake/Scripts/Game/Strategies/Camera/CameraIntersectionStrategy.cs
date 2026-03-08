namespace Shine.EscapeSnake.GamePlay.CameraStrategies
{
    using Shine.EscapeSnake;
    using UnityEngine;
    public interface ICameraIntersectionStrategy
    {
        public Vector2 FindIntersectionPoint(Vector2 startPosition, Vector2 direction);
        public void UpdateLines();
    }

    public class CameraIntersectionStrategy : ICameraIntersectionStrategy
    {
        public CameraController cameraController;
        Vector2[][] lines;
        public CameraIntersectionStrategy(CameraController camera)
        {
            this.cameraController = camera;
        }

        public void UpdateLines()
        {
            float left = -cameraController.HalfWidth;
            float right = cameraController.HalfWidth;
            float top = cameraController.HalfHeight;
            float bottom = -cameraController.HalfHeight;
            var bottomLeft = new Vector2(left, bottom);
            var bottomRight = new Vector2(right, bottom);
            var topLeft = new Vector2(left, top);
            var topRight = new Vector2(right, top);

            lines = new Vector2[][]{
                new Vector2[]{bottomLeft,topLeft},
                new Vector2[]{ topLeft,topRight},
                new Vector2[]{topRight,bottomRight},
                new Vector2[]{ bottomRight,bottomLeft}
            };
        }

        public Vector2 FindIntersectionPoint(Vector2 origin, Vector2 rayDirection)
        {
            foreach (var line in lines)
            {
                if (ShineMath.RaycastLine(origin, rayDirection, line[0], line[1], out var result))
                {
                    return result;
                }
            }
            return origin;
        }
    }
}