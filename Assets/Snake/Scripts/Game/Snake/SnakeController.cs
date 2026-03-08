
namespace Shine.EscapeSnake.GamePlay.Snake
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using Shine.EscapeSnake;
    using Shine.EscapeSnake.Utils;
    using UnityEngine;
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] SnakeSegment bodyPrefab;
        [Space]
        [SerializeField] SnakeSegment head;
        //
        public bool IsActive { get; private set; }
        public SnakeData Data { get; private set; }
        public int ID => Data.id;
        public int ColorId => Data.colorId;
        //
        private SnakeSegment[] _segements;
        [SerializeField]
        private SnakePath path;
        private bool isMoving;
        private int BodyPartsPerCell => GameManager.Grid.GridSettings.BodyPartsPerCell;
        // private int BodyPartsPerCell => 3;
        public void Activate(SnakeData snakeData)
        {
            Data = snakeData;
            SnakeSkinData snakeSkinData = GameManager.SnakeSkinsDatabase.GetSkin(snakeData.colorId);
            if (snakeSkinData == null)
            {
                Debug.LogError("skin is null: " + snakeData.colorId);
                IsActive = false;
                gameObject.SetActive(false);
                return;
            }
            Create(snakeSkinData);
            IsActive = true;
        }

        public void Activate(SnakeData snakeData, SnakeSkinData snakeSkinData)
        {
            Data = snakeData;
            if (snakeSkinData == null)
            {
                Debug.LogError("skin is null: " + snakeData.colorId);
                IsActive = false;
                gameObject.SetActive(false);
                return;
            }
            Create(snakeSkinData);
            IsActive = true;
        }

        private void Create(SnakeSkinData snakeSkinData)
        {
            var pathLength = Data.path.Length + 2;
            path = new SnakePath(Data.path);

            _segements = new SnakeSegment[Data.path.Length * BodyPartsPerCell - 1];
            _segements[0] = head;
            head.id = 0;
            head.AddListener(OnClicked);
            for (int i = 1; i < _segements.Length; i++)
            {
                _segements[i] = Instantiate(bodyPrefab, transform);
                _segements[i].transform.SetAsFirstSibling();
                _segements[i].id = i;
                _segements[i].AddListener(OnClicked);
            }

            UpdateVisuals(snakeSkinData);
            UpdateSegments();
        }

        private void OnClicked()
        {
            PlayMoveAnimation();
        }

        private void UpdateVisuals(SnakeSkinData snakeSkinData)
        {
            _segements[0].UpdateVisuals(snakeSkinData.head);

            for (int i = 1; i < _segements.Length - 2; i++)
            {
                _segements[i].UpdateVisuals(snakeSkinData.body);
            }

            _segements[^2].UpdateVisuals(snakeSkinData.tailBase);
            _segements[^1].UpdateVisuals(snakeSkinData.tailTip);
        }

        private Vector2 GetWorldPosition(Vector2 position)
        {
            return GameManager.Grid.GetWorldPosition(position);
        }

        private void UpdateSegments()
        {
            int pathLength = path.Length;

            Vector2 tailDirection = Vector2.zero;
            for (int i = 1; i < pathLength - 2; i++)
            {
                tailDirection = Utils.Normalized(path[i + 1] - path[i]);
                for (int j = 0; j < BodyPartsPerCell; j++)
                {
                    _segements[(i - 1) * BodyPartsPerCell + j].UpdatePosition(GetWorldPosition(Vector2.Lerp(path[i], path[i + 1], j * 1f / BodyPartsPerCell)), tailDirection);
                }
            }
            _segements[^2].UpdatePosition(GetWorldPosition(path[^2]), tailDirection);
            _segements[^1].UpdatePosition(GetWorldPosition(Vector2.Lerp(path[^2], path[^1], 1f / BodyPartsPerCell)), tailDirection);
        }

        public void PlayMoveAnimation()
        {
            if (isMoving) return;
            isMoving = true;

            Vector2Int headDirection = path[0] - path[1];
            var headPos = GameManager.Grid.GetWorldPosition(path[1]);
            bool isCollided = GameManager.Grid.FindCollisionPoint(path[0], headDirection, out var collisionPoint);

            float distance;
            if (isCollided)
            {
                distance = (int)Vector2Int.Distance(collisionPoint, path[0]) + .5f;
            }
            else
            {
                Vector2 intersectionPoint = GameManager.Camera.intersectionStrategy.FindIntersectionPoint(headPos, headDirection);
                var extraDistance = Vector2.Distance(intersectionPoint, headPos) * 1f / BodyPartsPerCell;
                distance = path.Length - 2 + Mathf.FloorToInt(extraDistance);

                for (int i = 1; i < path.Length - 1; i++)
                {
                    GameManager.Grid.RemoveCell(path[i]);
                }

                GameManager.Instance.IncreaseScore();
            }

            var duration = distance * 1f / GameManager.Grid.SnakeSettings.Speed;
            var _segementLength = _segements.Length;
            DOVirtual.Float(0, distance, duration, (v) =>
            {
                Move(v);
            }).OnComplete(() =>
            {
                isMoving = false;
                Move(distance);
                if (isCollided)
                {
                    PlayReverseMoveAnimation(distance);
                }
            });
        }

        public void PlayReverseMoveAnimation(float distance)
        {
            isMoving = true;
            GameManager.Camera.ShakeEffect.Shake(0.05f);
            if (distance == 0)
            {
                isMoving = false;
                UpdateSegments();
                return;
            }
            var duration = distance * 1f / GameManager.Grid.SnakeSettings.Speed;
            DOVirtual.Float(distance, 0, duration, (v) =>
            {
                Move(v);
            }).OnComplete(() =>
            {
                isMoving = false;
                UpdateSegments();
            });
        }

        public void Move(float distance)
        {
            var _segementLength = _segements.Length;

            for (int i = 0; i < _segementLength; i++)
            {
                path.GetPositionAndDirection(i * 1f / (_segementLength + 1), distance, out var position, out var direction);
                _segements[i].UpdatePosition(GetWorldPosition(position), direction);
            }
        }

        public void Deactivate()
        {
            Destroy(this.gameObject);
        }
    }
}