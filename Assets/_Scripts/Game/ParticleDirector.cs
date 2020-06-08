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

        public void PlayFireVFX(Vector3 position)
        { 
            var result = objectPool.Use(nameof(FireVFX)).GetComponent<FireVFX>();
            result.PlayOn(position);
        }
    }
}