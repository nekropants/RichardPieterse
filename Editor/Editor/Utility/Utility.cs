using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


namespace RichardPieterse
{
    public  static class EditorExtensions 
    {
       
        [MenuItem("CONTEXT/Animator/Create Controller With Empty Idle")]
        public static void CreateTwoStateAnimator(MenuCommand command)
        {
            string directory = Application.dataPath + "/Silverlake/Animations";
            Debug.Log(directory);
            string filePath = EditorUtility.SaveFilePanel("Choose Location",
                directory, "Animator_TwoState_UniqueName", ".controller");
            // Debug.Log(filePath);
            
            if (filePath == "")
            {
                return;
            }
            string projectPath = "Assets/" + filePath.Replace(Application.dataPath, "") + ".asset";
    
            var controller = AnimatorController.CreateAnimatorControllerAtPath(projectPath);
            
            var idleState = controller.layers[0].stateMachine.AddState("Idle");
        
            var clip = new AnimationClip();
            clip.name = "Idle"; // set name


            string directoryName = Path.GetDirectoryName(projectPath);
            directoryName = directoryName.Replace(Application.dataPath, "Assets/");
            AssetDatabase.CreateAsset(clip, directoryName+ "/Anim_" +clip.name+".anim"); // to create asset
            
            idleState.motion = clip;

            RuntimeEditorHelper.Ping(controller);
            (command.context as Animator).runtimeAnimatorController = controller;
        }
    }
}