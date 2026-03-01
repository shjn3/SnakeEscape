using Shine.EscapeSnake.GamePlay;
using UnityEngine;
namespace Shine.EscapeSnake
{
    [DefaultExecutionOrder((int)ExecuteOrder.LevelManager)]
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] LevelsDatabase _levelsDatabase;
        public LevelData LevelData { get; private set; }

        public int MaxLevel => _levelsDatabase.maxLevel;

        public void PrepareLevel()
        {
            LevelData = _levelsDatabase.GetLevel(GameManager.Instance.GameData.Level);
        }

    }
}