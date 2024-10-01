using System.Collections;
using UnityEngine;

namespace Hardgore
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;
        public static CoroutineRunner instance
        {
            get
            {
                if (_instance == null && Application.isPlaying)
                {
                    _instance = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
                    _instance.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            MonoBehaviour monoBehaviour = instance;
            return monoBehaviour.StartCoroutine(routine);
        }
        
        public void StopCoroutine(Coroutine routine)
        {
            MonoBehaviour monoBehaviour = instance;
            monoBehaviour.StopCoroutine(routine);
        }
    }
}