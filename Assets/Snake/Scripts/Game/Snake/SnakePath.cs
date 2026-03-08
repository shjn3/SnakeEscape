namespace Shine.EscapeSnake.GamePlay.Snake
{
    using System.Collections.Generic;
    using Shine.EscapeSnake.Utils;
    using Unity.VisualScripting;
    using UnityEngine;

    [System.Serializable]
    public class SnakePath
    {
        public List<Vector2Int> path = new();
        public int Length => path.Count;
        public SnakePath(Vector2Int[] dataPath)
        {
            path.Add(dataPath[0] + dataPath[0] - dataPath[1]);
            path.AddRange(dataPath);
            path.Add(dataPath[^1] + Utils.Normalized(dataPath[^1] - dataPath[^2]));
        }

        public Vector2Int this[int idx]
        {
            get
            {
                return path[idx];
            }
            set
            {
                path[idx] = value;
            }
        }

        public Vector2Int[] ToArray()
        {
            return path.ToArray();
        }

        public void GetPositionAndDirection(float percent, float distance, out Vector2 position, out Vector2 direction)
        {
            float start = percent * 1f * (this.path.Count - 2);
            var nearId = (int)(start + 1 - distance);
            if (nearId > 1)
            {
                var lerpValue = distance - (start - nearId);
                position = Vector2.Lerp(path[nearId + 1], path[nearId], lerpValue);
                direction = path[nearId + 1] - path[nearId];
            }
            else
            {
                var dt = distance - start;
                var headDirection = path[0] - path[1];
                position = path[1] + (Vector2)headDirection * dt;
                direction = -headDirection;
            }
        }
    }
}