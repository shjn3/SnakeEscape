using UnityEngine;

namespace Shine.EscapeSnake.GamePlay
{
    [System.Serializable]
    public class SnakeSkinData
    {
        public Sprite head;
        public Sprite body;
        public Sprite tailTip;
        public Sprite tailBase;
    }
    [CreateAssetMenu(fileName = "SnakeSkinsDatabase", menuName = "ShineSnake/Database/SnakeSkins")]
    public class SnakeSkinsDatabase : ScriptableObject
    {
        [SerializeField] SnakeSkinData[] skins;

        public SnakeSkinData GetSkin(int id)
        {
            
            if (id < 0 || id > skins.Length) return default;
            return skins[id];
        }
    }
}