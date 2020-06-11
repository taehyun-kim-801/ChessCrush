using ChessCrush.VFX;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ParticleDirector : SubDirector
    {
        private ObjectPool objectPool;

        private void Awake()
        {
            objectPool = gameObject.GetComponent<ObjectPool>();
        }

        public void PlayVFX<T>(Vector3 position) where T: VFX.VFX
        {
            var result = objectPool.Use(typeof(T).Name).GetComponent<T>();
            result.PlayOn(position);
        }
    }
}