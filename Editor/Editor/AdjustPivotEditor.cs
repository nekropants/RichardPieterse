using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RichardPieterse
{
    [CustomEditor(typeof(AdjustPivot))]
    public class AdjustPivotEditor : UnityEditor.Editor
    {
        private Vector3 position;
        
        private void Awake()
        {
            AdjustPivot transform = target as AdjustPivot;
            position = transform.transform.position;
        }

        private void OnSceneGUI()
        {
            AdjustPivot pivot = target as AdjustPivot;
            Transform transform = pivot.transform;

            
            EditorGUI.BeginChangeCheck();
            position = Handles.PositionHandle(position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                List<Vector3> childPositions = new List<Vector3>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    childPositions.Add(transform.GetChild(i).position);

                }
               
                // Undo.RecordObject(transform, "Change Look At Target Position");
                // example.targetPosition = newTargetPosition;
                // example.Update();

                transform.position = position;

                for (int i = 0; i < childPositions.Count; i++)
                {
                    transform.GetChild(i).position = childPositions[i];
                }
                
            }
        }
    }
}