namespace Shine.EscapeSnake
{
    using System;
    using System.Collections.Generic;
    using Shine.EscapeSnake.GamePlay;
    using Shine.EscapeSnake.GamePlay.Snake;
    using Shine.EscapeSnake.GamePlay.Snake.Settings;
    using UnityEngine;

    [DefaultExecutionOrder((int)ExecuteOrder.GridManager)]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] SnakeController snakePrefab;
        [SerializeField] GridSettings gridSettings;
        [SerializeField] SnakeSettings snakeSettings;

        public GridSettings GridSettings => gridSettings;
        public SnakeSettings SnakeSettings => snakeSettings;
        //
        private LevelData levelData;
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<SnakeController> snakes = new();
        public Vector2 worldBottomleft { get; private set; }
        RectInt gridBounds;

        private Dictionary<Vector2Int, SnakeController> occupiedCells = new();

        public void PrepareLevel()
        {
            levelData = GameManager.LevelManger.LevelData;

            CalculateBounds();
            SpawnSnakes();
        }

        private void CalculateBounds()
        {

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;


            for (int i = 0; i < levelData.snakes.Length; i++)
            {
                foreach (var point in levelData.snakes[i].path)
                {
                    minX = Math.Min(minX, point.x);
                    minY = Math.Min(minY, point.y);

                    maxX = Math.Max(maxX, point.x);
                    maxY = Math.Max(maxY, point.y);
                }
            }
            gridBounds = new RectInt(new Vector2Int(minX, minY), new Vector2Int(maxX - minX, maxY - minY));

            Width = maxX - minX;
            Height = maxY - minY;

            worldBottomleft = new Vector2(minX + Width * .5f, minY + Height * .5f);
        }

        private void SpawnSnakes()
        {
            occupiedCells.Clear();

            for (int i = 0; i < levelData.snakes.Length; i++)
            {
                var snakeData = levelData.snakes[i];
                SnakeController snake = Instantiate(snakePrefab);
                snake.Activate(snakeData);
                snakes.Add(snake);

                foreach (var point in snakeData.path)
                {
                    occupiedCells.Add(point, snake);
                }
            }
        }

        public Vector2 GetWorldPosition(Vector2 position)
        {
            return (position - worldBottomleft) * GameManager.Grid.GridSettings.BodyPartsPerCell;
        }

        public bool IsCellOccupied(Vector2Int nextPosition)
        {
            return occupiedCells.ContainsKey(nextPosition);
        }

        public SnakeController GetSnakeAtCell(Vector2Int position)
        {
            occupiedCells.TryGetValue(position, out var result);
            return result;
        }

        public void RemoveCell(Vector2Int position)
        {
            occupiedCells.Remove(position);
        }

        public void Clear()
        {
            foreach (var snake in snakes)
            {
                snake.Deactivate();
            }

            snakes.Clear();
        }

        public bool FindCollisionPoint(Vector2Int start, Vector2Int direction, out Vector2Int result)
        {
            Vector2Int point = start;
            result = start;

            while (gridBounds.Contains(point))
            {

                if (IsCellOccupied(point))
                {
                    result = point;
                    return true;
                }


                point += direction;
            }
            return false;
        }
    }
}