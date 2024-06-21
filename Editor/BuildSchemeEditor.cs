using Plugins.RichFramework.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildScheme))]
public class BuildSchemeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BuildScheme buildScheme = target as BuildScheme;
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Apply Settings"))
        {
            buildScheme.ApplyModeAndSdk();   
        }
        
        if (GUILayout.Button("Build"))
        {
            buildScheme.PerformBuild();   
        }
        
        if (GUILayout.Button("Open XCode"))
        {
            buildScheme.OpenXcode();   
        }
        
        if (GUILayout.Button("Show in Finder"))
        {
            buildScheme.OpenBuilderFolder();   
        }
        if (GUILayout.Button("Archive in XCode"))
        {
            buildScheme.DeleteBuild();   
        }
        
        if (GUILayout.Button("Delete Build"))
        {
            buildScheme.DeleteBuild();   
        }
    }
}