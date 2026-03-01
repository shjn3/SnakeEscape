namespace Shine.EscapeSnake.GamePlay.UI
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class GameUIController : MonoBehaviour
    {
        [SerializeField] UIDocument uiDocument;

        private GameUIView gameView;

        public void Init()
        {
            gameView = new GameUIView(uiDocument.rootVisualElement);
            gameView.refreshButton.clicked += GameManager.Instance.Refresh;
            gameView.nextLevelButton.clicked += GameManager.Instance.NextLevel;
            gameView.previousLevelButton.clicked += GameManager.Instance.PreviousLevel;
            GameManager.Instance.OnUpdatedScore += UpdateScoreText;
        }

        public void PrepareLevel()
        {
            UpdateLevelText();
            UpdateScoreText();
            gameView.SetEnabledButtons(GameManager.Instance.GameData.Level, GameManager.LevelManger.MaxLevel);
        }

        public void OnRefreshButtonClicked()
        {
            GameManager.Instance.Refresh();
        }

        public void UpdateLevelText()
        {
            gameView.UpdateLevelText(GameManager.Instance.GameData.Level);
        }

        public void UpdateScoreText()
        {
            gameView.UpdateScoreText(GameManager.Instance.GameData.Score, GameManager.Instance.GameData.MaxScore);
        }
    }
}