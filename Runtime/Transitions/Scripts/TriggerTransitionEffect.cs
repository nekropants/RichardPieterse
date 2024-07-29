using BeyondThePines;
using UnityEngine;

namespace RichardPieterse
{
    public class TriggerTransitionEffect : MonoBehaviour
    {
        [SerializeField] private TransitionWithAnimator _transitionPrefab;

        public void Trigger()
        {
            TransitionWithAnimator transitionWithAnimator = _transitionPrefab.Instantiate();
            transitionWithAnimator.DoFullTransition(null, null);
        }
    }
}