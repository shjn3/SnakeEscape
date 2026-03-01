using UnityEngine;
namespace Shine.EscapeSnake
{
    [System.Serializable]
    public class SnakeData
    {
        public int id;
        public int colorId;
        public Vector2Int[] path;
    }

    [System.Serializable]
    public class LevelData
    {
        public int width;
        public int height;

        public SnakeData[] snakes;
    }
}