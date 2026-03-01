using System;
using Shine.EscapeSnake.GamePlay.UI;
using UnityEngine;
namespace Shine.EscapeSnake.GamePlay
{
    [DefaultExecutionOrder((int)ExecuteOrder.GameManager)]
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;
        [SerializeField] LevelManager levelManager;
        public static LevelManager LevelManger => instance.levelManager;

        [SerializeField] GridManager gridManager;
        public static GridManager Grid => instance.gridManager;
        [SerializeField] CameraController cameraController;
        public static CameraController Camera => instance.cameraController;
        [SerializeField] InputManager inputManager;
        public static InputManager InputManager => instance.inputManager;

        [SerializeField] SnakeSkinsDatabase snakeSkinsDatabase;
        public static SnakeSkinsDatabase SnakeSkinsDatabase => instance.snakeSkinsDatabase;
        [SerializeField] GameUIController gameUIController;

        public event System.Action OnUpdatedScore;

        public GameData GameData { get; private set; }

        private void Awake()
        {
            instance = this;
            GameData = new();
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            cameraController.Init();
            gameUIController.Init();
            PrepareLevel();
        }

        private void PrepareLevel()
        {
            levelManager.PrepareLevel();

            GameData.MaxScore = levelManager.LevelData.snakes.Length;
            UpdateScore(0);

            gridManager.PrepareLevel();
            cameraController.PrepareLevel();
            gameUIController.PrepareLevel();
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void Refresh()
        {
            Debug.Log("Refresh");
            gridManager.Clear();
            gridManager.PrepareLevel();
        }

        public void NextLevel()
        {
            if (GameData.Level == levelManager.MaxLevel)
            {
                Debug.LogWarning("The game is already at the maximum level.");
                return;
            }
            GameData.Reset();
            GameData.Level += 1;
            gridManager.Clear();

            PrepareLevel();
        }

        public void PreviousLevel()
        {
            if (GameData.Level == 1)
            {
                Debug.LogWarning("You are already at the first level.");
                return;
            }
            gridManager.Clear();
            GameData.Reset();
            GameData.Level -= 1;
            PrepareLevel();
        }

        public void IncreaseScore()
        {
            UpdateScore(GameData.Score + 1);
        }

        public void DecreaseScore()
        {
            UpdateScore(GameData.Score - 1);
        }

        private void UpdateScore(int score)
        {
            GameData.Score = Math.Clamp(score, 0, GameData.MaxScore);
            OnUpdatedScore?.Invoke();
        }
    }
}