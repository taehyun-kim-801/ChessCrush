using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ChessCrush.VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class VFX: MonoBehaviour
    {
        private new ParticleSystem particleSystem;
        private bool isPlaying;

        private void Awake()
        {
            particleSystem = gameObject.GetComponent<ParticleSystem>();
            gameObject.UpdateAsObservable().Where(_ => isPlaying && particleSystem.isStopped).Subscribe(_ => gameObject.SetActive(isPlaying = !isPlaying)).AddTo(gameObject);
        }

        public void PlayOn(Vector3 position)
        {
            transform.position = position;
            particleSystem.Play();
            isPlaying = true;
        }
    }
}