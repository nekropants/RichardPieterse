using UnityEditor;
using UnityEngine;
using System.IO;

namespace RichardPieterse
{

    public class NamespaceWrapperEditor : EditorWindow
    {
        private string folderPath = "Assets/Scripts";
        private string namespaceName = "MyNamespace";

        [MenuItem("Tools/Namespace Wrapper")]
        public static void ShowWindow()
        {
            GetWindow<NamespaceWrapperEditor>("Namespace Wrapper");
        }

        private void OnGUI()
        {
            GUILayout.Label("Wrap Scripts in Namespace", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Folder Path:", GUILayout.Width(70));
            folderPath = EditorGUILayout.TextField(folderPath);

            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", projectPath, "");

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
                int lastUsingIndex = -1;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Trim().StartsWith("namespace"))
                    {
                        inNamespace = true;
                        break;
                    }

                    if (lines[i].Trim().StartsWith("using"))
                    {
                        lastUsingIndex = i;
                    }
                }

                if (!inNamespace)
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        for (int i = 0; i <= lastUsingIndex; i++)
                        {
                            writer.WriteLine(lines[i]);
                        }

                        writer.WriteLine($"\nnamespace {namespaceName}");
                        writer.WriteLine("{");

                        for (int i = lastUsingIndex + 1; i < lines.Length; i++)
                        {
                            writer.WriteLine($"    {lines[i]}");
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