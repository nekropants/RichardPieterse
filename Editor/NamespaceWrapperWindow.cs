using UnityEditor;
using UnityEngine;
using System.IO;

namespace RichardPieterse
{

    public class NamespaceWrapperWindow : EditorWindow
    {
        private string folderPath = "Assets/Scripts";
        private string namespaceName = "RichardPieterse";

        [MenuItem("Tools/Namespace Wrapper")]
        public static void ShowWindow()
        {
            GetWindow<NamespaceWrapperWindow>("Namespace Wrapper");
        }

        private void OnGUI()
        {
            GUILayout.Label("Wrap Scripts in Namespace", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Folder Path:", GUILayout.Width(70));
            folderPath = EditorGUILayout.TextField(folderPath);

            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", folderPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (selectedPath.StartsWith(Application.dataPath))
                    {
                        folderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    }
                    else
                    {
                        Debug.LogWarning("Folder must be inside the Assets folder!");
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Namespace:", GUILayout.Width(70));
            namespaceName = EditorGUILayout.TextField(namespaceName);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Wrap Scripts"))
            {
                WrapScriptsInNamespace(folderPath, namespaceName);
            }
        }

        private void WrapScriptsInNamespace(string folderPath, string namespaceName)
        {
            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                string[] lines = File.ReadAllLines(file);
                bool inNamespace = false;

                foreach (var line in lines)
                {
                    if (line.Trim().StartsWith("namespace"))
                    {
                        inNamespace = true;
                        break;
                    }
                }

                if (!inNamespace)
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine($"namespace {namespaceName}");
                        writer.WriteLine("{");
                        foreach (var line in lines)
                        {
                            writer.WriteLine($"    {line}");
                        }

                        writer.WriteLine("}");
                    }

                    Debug.Log($"Wrapped {file} in namespace {namespaceName}");
                }
                else
                {
                    Debug.Log($"Skipped {file} as it is already in a namespace");
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
