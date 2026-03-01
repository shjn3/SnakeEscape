using UnityEngine;

namespace Shine.EscapeSnake
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "ShineSnake/Settings/Grid")]
    public class GridSettings : ScriptableObject
    {
        [SerializeField] float gridSize = 1;

        public float GridSize => gridSize;

        [SerializeField] int bodyPartsPerCell = 3;
        public int BodyPartsPerCell => bodyPartsPerCell;
    }
}