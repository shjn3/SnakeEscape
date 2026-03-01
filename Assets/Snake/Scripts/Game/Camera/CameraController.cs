namespace Shine.EscapeSnake.GamePlay
{
    using Shine.EscapeSnake.GamePlay.CameraEffect;
    using Shine.EscapeSnake.GamePlay.CameraStrategies;
    using UnityEngine;
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera main;
        [SerializeField] ShakeEffect shakeEffect;

        public ShakeEffect ShakeEffect => shakeEffect;
        [SerializeField] Vector2 padding;
        [SerializeField] Vector2 cameraPadding;

        public float HalfHeight => cameraBounds.height + main.orthographicSize;
        public float HalfWidth => cameraBounds.width + main.orthographicSize * main.aspect;
        public Rect cameraBounds { get; private set; }
        private float defaultOrthorgraphicSize;

        public ICameraIntersectionStrategy intersectionStrategy { get; private set; }

        public void Init()
        {
            intersectionStrategy = new CameraIntersectionStrategy(this);
            GameManager.InputManager.RegisterDragEvent(OnDrag);
        }

        public void PrepareLevel()
        {
            var gridWidth = GameManager.Grid.Width;
            var gridHeight = GameManager.Grid.Height;

            cameraBounds = new Rect(-gridWidth * .5f - cameraPadding.x, -gridHeight * .5f - cameraPadding.y, gridWidth + cameraPadding.x * 2, gridHeight + cameraPadding.y * 2);

            if (GameManager.Grid.Width > GameManager.Grid.Height)
            {
                defaultOrthorgraphicSize = (GameManager.Grid.Width * GameManager.Grid.GridSettings.BodyPartsPerCell + padding.x) / (main.aspect * 2);
            }
            else
            {
                defaultOrthorgraphicSize = (GameManager.Grid.Height * GameManager.Grid.GridSettings.BodyPartsPerCell + padding.y) * .5f;
            }

            main.orthographicSize = defaultOrthorgraphicSize;
        }

        private void OnDrag(Vector3 delta)
        {
            // delta.z = 0;
            // Move(delta);
        }

        private void Move(Vector3 relativePos)
        {
            if (cameraBounds.Contains(transform.position + relativePos))
            {
                transform.position += relativePos;
            }
        }

        //Debugg
        public void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.yellow;
            var halfHeight = main.orthographicSize;
            var halfWidth = halfHeight * main.aspect;
            var bottomLeft = new Vector3(cameraBounds.xMin - halfWidth, cameraBounds.yMin - halfHeight);
            var topLeft = new Vector3(cameraBounds.xMin - halfWidth, cameraBounds.yMax + halfHeight);
            var topRight = new Vector3(cameraBounds.xMax + halfWidth, cameraBounds.yMax + halfHeight);
            var bottomRight = new Vector3(cameraBounds.xMax + halfWidth, cameraBounds.yMin - halfHeight);

            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }

        public void Shake()
        {

        }
    }
}
