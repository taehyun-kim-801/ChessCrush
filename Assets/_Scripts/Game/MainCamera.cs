using System.Collections;
using UnityEngine;

namespace ChessCrush.Game
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera: MonoBehaviour
    {
        private new Camera camera;
        private Vector3 initialPosition;

        private void Awake()
        {
            camera = gameObject.GetComponent<Camera>();
            initialPosition = camera.transform.position;
        }

        public void Vibrate(float amount, float time)
        {
            StartCoroutine(CoVibrate(amount, time));
        }

        private IEnumerator CoVibrate(float amount, float time)
        {
            float now = Time.time;
            while(Time.time - now < time)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Random.insideUnitSphere * amount + initialPosition;
            }

            transform.position = initialPosition;
        }
    }
}