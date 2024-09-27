using System.Collections;
using System.Collections.Generic;
using RichardPieterse;
using UnityEditor;
using UnityEngine;

namespace BeyondThePines
{
    [CustomEditor(typeof(TransitionWithAnimation))]
    public class TestTransitionButtonEditor : CustomEditorBase<TransitionWithAnimation>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Test"))
            {
                if (RuntimeEditorHelper.IsPrefabAsset(targetObject.gameObject))
                {
                    TransitionWithAnimation transitionWithAnimation = targetObject.Instantiate();
                    transitionWithAnimation.TestTransition();
                }
                else
                {
                    targetObject.TestTransition();
                }
            }
        }
    }
}
