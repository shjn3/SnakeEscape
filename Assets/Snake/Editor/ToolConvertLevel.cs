namespace Shine.EscapeSnake.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    public class LevelArray
    {
        [System.Serializable]
        public class Level
        {
            [System.Serializable]
            public class Board
            {
                public int cols;
                public int rows;
            }

            [System.Serializable]
            public class Snake
            {
                public int id;
                public int color;

                public Vector2Int[] path;
            }
            public int id;
            public int version;
            public Board board;
            public Snake[] snakes;

        }
        public List<Level> levels;

    }
    public class ToolConvertLevel : EditorWindow
    {
        [MenuItem("Tools/Convert Level")]
        public static void ShowMenu()
        {
            GetWindow<ToolConvertLevel>();
        }

        public LevelsDatabase database;
        public TextAsset levelTextAsset;

        private SerializedObject _serializedObject;
        private SerializedProperty databaseProperty;
        private SerializedProperty levelTextProperty;



        public void OnGUI()
        {
            if (_serializedObject == null)
            {
                _serializedObject = new SerializedObject(this);
                databaseProperty = _serializedObject.FindProperty("database");
                levelTextProperty = _serializedObject.FindProperty("levelTextAsset");
            }


            EditorGUILayout.PropertyField(databaseProperty);
            EditorGUILayout.PropertyField(levelTextProperty);

            if (GUILayout.Button("Convert"))
            {
                Convert();
            }


            _serializedObject.ApplyModifiedProperties();
        }

        private void Convert()
        {
            LevelArray levelarray = JsonUtility.FromJson<LevelArray>(levelTextAsset.text);
            LevelData[] levels = new LevelData[levelarray.levels.Count];

            for (int i = 0; i < levels.Length; i++)
            {
                LevelData level = new LevelData()
                {
                    width = levelarray.levels[i].board.cols,
                    height = levelarray.levels[i].board.rows,
                    snakes = CopySnake(levelarray.levels[i].snakes)
                };
                levels[i] = level;
            }

            database.SetLevels(levels);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssetIfDirty(database);

            Debug.Log("Convert successfully!");
        }

        private SnakeData[] CopySnake(LevelArray.Level.Snake[] snake)
        {
            SnakeData[] result = new SnakeData[snake.Length];
            for (int i = 0; i < snake.Length; i++)
            {
                result[i] = new SnakeData()
                {
                    id = snake[i].id,
                    colorId = snake[i].color,
                    path = snake[i].path.ToArray(),
                };
            }

            return result;
        }
    }
}