using System.Collections;
using System.Collections.Generic;
using RichardPieterse;
using UnityEditor;
using UnityEngine;

namespace BeyondThePines
{
    [CustomEditor(typeof(TransitionWithAnimator))]
    public class TestTransitionButtonEditor : CustomEditorBase<TransitionWithAnimator>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Test"))
            {
                if (RuntimeEditorHelper.IsPrefabAsset(targetObject.gameObject))
                {
                    TransitionWithAnimator transitionWithAnimator = targetObject.Instantiate();
                    transitionWithAnimator.TestTransition();
                }
                else
                {
                    targetObject.TestTransition();
                }
            }
        }
    }
}
