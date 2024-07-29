using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BeyondThePines
{
    public class TransitionWithAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _inDuration;
        [SerializeField] private AnimationClip _outDuration;
        private static readonly int InTrigger = Animator.StringToHash("TransitionIn");
        private static readonly int OutTrigger = Animator.StringToHash("TransitionOut");

        private bool _doNotAutoDestroy;
        
        private IEnumerator TransitionIn(Action onComplete)
        {
            _animator.SetTrigger(InTrigger);
            _animator.ResetTrigger(OutTrigger);

            float lerp = 0;
            while (lerp < 1)
            {
                lerp += Time.deltaTime / _inDuration.length;
                lerp = Mathf.Clamp01(lerp);
                yield return null;
            }

            onComplete?.Invoke();
        }

        private IEnumerator TransitionOut(Action onComplete)
        {
            _animator.SetTrigger(OutTrigger);
            _animator.ResetTrigger(InTrigger);
            float lerp = 0;
            while (lerp < 1)
            {
                lerp += Time.deltaTime / _outDuration.length;
                lerp = Mathf.Clamp01(lerp);
                yield return null;
            }

            onComplete?.Invoke();
        }

        private IEnumerator IEDoFullTransition(Action onTransitionInComplete,
            Action onTransitionOutComplete)
        {
            if(_doNotAutoDestroy ==false)
                DontDestroyOnLoad(gameObject);
            
            yield return StartCoroutine(TransitionIn(null));
            yield return null;
            onTransitionInComplete?.Invoke();
            yield return null;
            yield return StartCoroutine(TransitionOut(null));
            yield return null;
            onTransitionOutComplete?.Invoke();
            yield return null;
            
            if(_doNotAutoDestroy == false)
                Destroy(this.gameObject);
        }

        [ContextMenu("Do Transition")]
        public void TestTransition()
        {
            _doNotAutoDestroy = true;
            DoFullTransition(null, null);
        }

        public Coroutine DoFullTransition(Action onTransitionInComplete, Action onTransitionOutComplete)
        {
            return StartCoroutine(IEDoFullTransition(onTransitionInComplete, onTransitionOutComplete));
        }

        public TransitionWithAnimator Instantiate()
        {
            GameObject newInstance = Instantiate(gameObject);
            TransitionWithAnimator transitionFadeToBlack = newInstance.GetComponent<TransitionWithAnimator>();
            return transitionFadeToBlack;
        }
    }
}
