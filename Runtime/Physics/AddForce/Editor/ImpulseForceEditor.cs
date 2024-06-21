namespace RichardPieterse
{
    using UnityEditor;
    using RichardPieterse.Editor;
    using UnityEngine;
    
    
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ImpulseForce))]
    public class ImpulseForceEditor : CustomEditorBase<ImpulseForce>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
    
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Apply Impulse"))
            {
                
                foreach (ImpulseForce addForce in targetObjects)
                {
                    addForce.ApplyForce();
                }
            }
        }
    }
}
