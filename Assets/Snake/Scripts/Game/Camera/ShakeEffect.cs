namespace Shine.EscapeSnake.GamePlay.CameraEffect
{
    using DG.Tweening;
    using UnityEngine;
    public class ShakeEffect : MonoBehaviour
    {
        [SerializeField] Transform target;
        Tweener shakeTween;
        public void Shake(float strenth, float duration = 0.3f)
        {
            if (shakeTween != null) return;
            Vector3 origin = target.transform.position;

            shakeTween = DOVirtual.Float(0, 1, duration, (v) =>
             {
                 target.transform.position = origin + new Vector3(Random.Range(-5, 5) * strenth, Random.Range(-5, 5) * strenth);
             }).OnComplete(() =>
             {
                 shakeTween = null;
                 target.transform.position = origin;
             });
        }
    }

}