namespace Shine.EscapeSnake.GamePlay.UI
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class GameUIView
    {
        public static class GameUIElement
        {
            public const string HEADER = "Header";
            public const string LEVEL_TEXT = "LevelLabel";
            public const string SCORE_TEXT = "ScoreLabel";
            public const string REFRESH_BUTTON = "RefreshButton";


            public const string FOOTER = "Footer";
            public const string PREVIOUS_LEVEL_BUTTON = "PreviousLevelButton";

            public const string NEXT_LEVEL_BUTTON = "NextLevelButton";
        }

        //Header
        private VisualElement header;
        public Label levelLabel;
        public Label scoreLabel;

        public Button refreshButton;

        //Footer
        private VisualElement footer;
        public Button nextLevelButton;

        public Button previousLevelButton;


        public GameUIView(VisualElement root)
        {
            header = root.Q(GameUIElement.HEADER);
            levelLabel = header.Q<Label>(GameUIElement.LEVEL_TEXT);
            scoreLabel = header.Q<Label>(GameUIElement.SCORE_TEXT);
            refreshButton = header.Q<Button>(GameUIElement.REFRESH_BUTTON);
            //
            footer = root.Q(GameUIElement.FOOTER);
            nextLevelButton = footer.Q<Button>(GameUIElement.NEXT_LEVEL_BUTTON);
            previousLevelButton = footer.Q<Button>(GameUIElement.PREVIOUS_LEVEL_BUTTON);
        }

        public void UpdateLevelText(int level)
        {

            levelLabel.text = "Level " + level;
        }

        public void UpdateScoreText(int score, int maxScore)
        {
            scoreLabel.text = $"Score: {score}/{maxScore}";
        }

        public void SetEnabledButtons(int currentLevel, int maxLevel)
        {
            nextLevelButton.SetEnabled(currentLevel < maxLevel);
            previousLevelButton.SetEnabled(currentLevel > 1);
        }
    }
}