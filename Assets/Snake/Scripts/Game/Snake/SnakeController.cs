
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
        private Vector2Int[] path;
        private Stack<Vector2Int[]> histories = new();
        private bool isMoving;

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

        private void Create(SnakeSkinData snakeSkinData)
        {
            var pathLength = Data.path.Length + 2;
            path = new Vector2Int[pathLength];
            for (int i = 1; i < pathLength - 1; i++)
            {
                path[i] = Data.path[i - 1];
            }
            path[0] = Data.path[0] + Data.path[0] - Data.path[1];
            path[^1] = Data.path[^1] + Utils.Normalized(Data.path[^1] - Data.path[^2]);

            _segements = new SnakeSegment[Data.path.Length * GameManager.Grid.GridSettings.BodyPartsPerCell - 1];
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

            UpdatePartsPosition();
        }

        private void OnClicked()
        {
            Move();
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

        private void UpdatePartsPosition()
        {
            int pathLength = path.Length;

            Vector2 tailDirection = Vector2.zero;
            for (int i = 1; i < pathLength - 2; i++)
            {
                tailDirection = Utils.Normalized(path[i + 1] - path[i]);
                for (int j = 0; j < GameManager.Grid.GridSettings.BodyPartsPerCell; j++)
                {
                    _segements[(i - 1) * GameManager.Grid.GridSettings.BodyPartsPerCell + j].UpdatePosition(GameManager.Grid.GetWorldPosition(Vector2.Lerp(path[i], path[i + 1], j * 1f / GameManager.Grid.GridSettings.BodyPartsPerCell)), tailDirection);
                }
            }
            _segements[^2].UpdatePosition(GameManager.Grid.GetWorldPosition(path[^2]), tailDirection);
            _segements[^1].UpdatePosition(GameManager.Grid.GetWorldPosition(Vector2.Lerp(path[^2], path[^1], 1f / GameManager.Grid.GridSettings.BodyPartsPerCell)), tailDirection);
        }

        public void Move()
        {
            if (isMoving) return;
            isMoving = true;

            Vector2Int headDirection = path[0] - path[1];
            var headPos = GameManager.Grid.GetWorldPosition(path[1]);

            bool isCollided = GameManager.Grid.FindCollisionPoint(path[0], headDirection, out var collisionPoint);

            int previousValue = 0;
            int pathLength = path.Length;


            int endValue;
            if (isCollided)
            {
                endValue = (int)Vector2Int.Distance(collisionPoint, path[0]) + 1;
            }
            else
            {
                Vector2 intersectionPoint = GameManager.Camera.intersectionStrategy.FindIntersectionPoint(headPos, headDirection);
                // endValue = (path.Length - 2) + Mathf.CeilToInt(Vector2.Distance(intersectionPoint, headPos) / GameManager.Grid.GridSettings.BodyPartsPerCell);
                endValue = (path.Length - 2);


                for (int i = 1; i < path.Length - 1; i++)
                {
                    GameManager.Grid.RemoveCell(path[i]);
                }
            }
            if (endValue == 0)
            {
                isMoving = false;
                Debug.Log("collision");
                return;
            }


            Vector2Int[] previousPath = new Vector2Int[path.Length];
            histories.Clear();

            var duration = endValue * 1f / GameManager.Grid.SnakeSettings.Speed;
            DOVirtual.Float(0, endValue, duration, (v) =>
            {
                if (previousValue != (int)v + 1 && v != endValue)
                {
                    previousValue = (int)v + 1;
                    previousPath = path.ToArray();
                    histories.Push(previousPath);

                    for (int i = path.Length - 1; i > 0; i--)
                    {
                        path[i] = path[i - 1];
                    }
                    path[0] += headDirection;
                }

                float miniValue = v % 1;
                for (int i = 0; i < _segements.Length; i++)
                {
                    float percent = pathLength - (i * 1f / GameManager.Grid.GridSettings.BodyPartsPerCell - miniValue);
                    int startPathId = pathLength - (int)percent + 1;
                    Vector2 position = Vector2.Lerp(previousPath[startPathId], path[startPathId], percent % 1);
                    _segements[i].UpdatePosition(GameManager.Grid.GetWorldPosition(position), previousPath[startPathId] - path[startPathId]);
                }
            }).OnComplete(() =>
            {
                isMoving = false;
                UpdatePartsPosition();
                if (isCollided)
                {
                    Undo();
                }
            });
        }


        public void Undo()
        {
            isMoving = true;
            GameManager.Camera.ShakeEffect.Shake(0.05f);
            int count = histories.Count;
            if (count == 0)
            {
                isMoving = false;
                UpdatePartsPosition();
                return;
            }
            int previousValue = 0;
            Vector2Int[] previousPath = path;
            var duration = count * 1f / GameManager.Grid.SnakeSettings.Speed;
            DOVirtual.Float(0, count, duration, (v) =>
            {
                if (previousValue != (int)v + 1 && v != count)
                {
                    previousValue = (int)v + 1;
                    if (histories.Count > 0)
                    {
                        previousPath = path;
                        path = histories.Pop();
                    }
                }
                float miniValue = v % 1;

                for (int i = 0; i < _segements.Length; i++)
                {
                    float id = i * 1f / GameManager.Grid.GridSettings.BodyPartsPerCell + miniValue;
                    int startPathId = (int)id + 1;
                    Vector2 position = Vector2.Lerp(previousPath[startPathId], path[startPathId], id % 1);
                    _segements[i].UpdatePosition(GameManager.Grid.GetWorldPosition(position), path[startPathId] - previousPath[startPathId]);
                }
            }).OnComplete(() =>
            {
                isMoving = false;
                UpdatePartsPosition();
            });
        }

        public void Deactivate()
        {
            Destroy(this.gameObject);
        }
    }
}