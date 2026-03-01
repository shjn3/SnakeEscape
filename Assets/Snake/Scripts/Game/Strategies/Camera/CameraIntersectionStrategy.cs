namespace Shine.EscapeSnake.GamePlay.CameraStrategies
{
    using UnityEngine;
    public interface ICameraIntersectionStrategy
    {
        public Vector2 FindIntersectionPoint(Vector2 startPosition, Vector2 direction);
    }

    public class CameraIntersectionStrategy : ICameraIntersectionStrategy
    {
        public CameraController cameraController;

        public CameraIntersectionStrategy(CameraController camera)
        {
            this.cameraController = camera;
        }
        public Vector2 FindIntersectionPoint(Vector2 origin, Vector2 rayDirection)
        {
            if (FindVerticalIntersectionPoint(origin, rayDirection, out var result))
            {
                return result;
            }

            if (FindHorizontalIntersectionPoint(origin, rayDirection, out result))
            {
                return result;
            }

            return origin;
        }

        private bool FindHorizontalIntersectionPoint(Vector2 origin, Vector2 rayDirection, out Vector2 result)
        {
            var minX = -cameraController.HalfWidth * .5f;
            var maxX = cameraController.HalfWidth * .5f;

            float x = Mathf.Sign(minX - origin.x) == Mathf.Sign(rayDirection.x) ? minX : maxX;
            var y = rayDirection.x > 0 ? (rayDirection.y * (x - origin.x) / rayDirection.x) : 0 + origin.y;

            if (y >= -cameraController.HalfHeight * .5f && x <= cameraController.HalfHeight * .5f)
            {
                result = new Vector2(x, y);
                return true;
            }
            result = Vector2.zero;
            return false;
        }

        private bool FindVerticalIntersectionPoint(Vector2 origin, Vector2 rayDirection, out Vector2 result)
        {
            var minY = -cameraController.HalfHeight * .5f;
            var maxY = cameraController.HalfHeight * .5f;

            float y = Mathf.Sign(minY - origin.y) == Mathf.Sign(rayDirection.y) ? minY : maxY;
            float x = rayDirection.y > 0 ? (rayDirection.x * (y - origin.y) / rayDirection.y) : 0 + origin.x;

            if (x >= -cameraController.HalfWidth * .5f && x <= cameraController.HalfWidth * .5f)
            {
                result = new Vector2(x, y);
                return true;
            }
            result = Vector2.zero;
            return false;
        }
    }
}