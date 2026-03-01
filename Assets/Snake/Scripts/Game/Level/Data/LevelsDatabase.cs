using UnityEngine;

namespace Shine.EscapeSnake
{
    [CreateAssetMenu(fileName = "Levels Database", menuName = "ShineSnake/Database/Levels")]
    public class LevelsDatabase : ScriptableObject
    {
        [UnityEngine.SerializeField] LevelData[] levels;

        public void SetLevels(LevelData[] levels)
        {
            this.levels = levels;
        }

        public int maxLevel => levels.Length;

        public LevelData GetLevel(int level)
        {
            if (level <= 0 || level > levels.Length) return default;
            return levels[level - 1];
        }
    }
}