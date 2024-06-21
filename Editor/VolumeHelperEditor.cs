using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VolumeHelper))]
public class VolumeHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VolumeHelper volumeHelper = target as VolumeHelper;
    }
}