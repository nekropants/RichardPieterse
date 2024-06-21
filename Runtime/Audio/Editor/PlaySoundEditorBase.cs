using RichardPieterse.Editor;
using UnityEditor;
using UnityEngine;

namespace RichardPieterse.Audio
{
    [CustomEditor(typeof(PlaySound))]
    public class PlaySoundEditorBase : CustomEditorBase<PlaySound>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Play"))
            {
                foreach (PlaySound playSound in targetObjects)
                {
                    playSound.Play();
                }
            }
        }
    }
}