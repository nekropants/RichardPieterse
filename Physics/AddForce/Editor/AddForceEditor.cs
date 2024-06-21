using UnityEditor;
using RichardPieterse.Editor;
using UnityEngine;


[CanEditMultipleObjects]
[CustomEditor(typeof(AddForce))]
public class AddForceEditor : CustomEditorBase<AddForce>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Apply Impulse"))
        {
            
            foreach (AddForce addForce in targetObjects)
            {
                addForce.ApplyForce();
            }
        }
    }
    
}
