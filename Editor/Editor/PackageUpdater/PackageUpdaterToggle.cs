using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RichardPieterse
{
    public static class PackageUpdaterToolbarButton
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            // UnityToolbarExtender.leftOfPlayButton.Add(OnToolbarGUI);
        }

        // private static void OnToolbarGUI()
        // {
        //     EditorGUI.BeginChangeCheck();
        //     
        //     if(GUILayout.Button( new GUIContent("L", "Switch to Git")))
        //     {
        //         Debug.Log("Switch to Git");
        //     }
        //     
        // }
    }
}
