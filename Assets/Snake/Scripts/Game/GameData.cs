namespace Shine.EscapeSnake.GamePlay
{
    public class GameData
    {
        public int Level = 9;
        public int Score = 0;
        public int MaxScore;

        public void Reset()
        {
            Score = 0;
            MaxScore = 0;
        }
    }
}