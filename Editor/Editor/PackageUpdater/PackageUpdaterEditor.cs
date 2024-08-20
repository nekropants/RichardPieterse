using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RichardPieterse
{
    [CustomEditor(typeof(PackageUpdater))]
    public class PackageUpdaterEditor : CustomEditorBase<PackageUpdater>
    {
        public enum PackageStatus
        {
            Local,
            Git,
            NotInstalled
        }

        private AddRequest addRequest;
        private RemoveRequest removeRequest;
        private ListRequest listRequest;
        private string targetPackagePath;
        private PackageStatus _packageStatus;
        private string _version;

        private void OnEnable()
        {
            CheckPackageStatus();
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = EnableGUI();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_packageName"));
            
            GUILayout.BeginHorizontal();
            

            GUI.enabled = false;
            EditorGUILayout.TextField("Local Path", targetObject.localFilePath);
            GUI.enabled = true;
            if (GUILayout.Button("...", GUILayout.Width(16)))
            {
                var packageJsonFilePath =
                    EditorUtility.OpenFilePanel("Local package.json", targetObject.localFilePath, "json");
                
                Debug.Log(packageJsonFilePath);
                targetObject.localFilePath = Path.GetDirectoryName(packageJsonFilePath);
            }
            GUI.enabled = EnableGUI();

            GUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_gitRepoUrl"));
            
            GUILayout.Space(10);
            GUILayout.Label("Status: " + _packageStatus);
            GUILayout.Label("Version: " + _version);
            GUILayout.Space(10);

            GUI.enabled = true;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Switch to Local"))
            {
                targetPackagePath = targetObject.localPackagePath;
                SwitchToPackage();
            }

            if (GUILayout.Button("Switch to Git"))
            {
                targetPackagePath = targetObject.gitRepoUrl;
                SwitchToPackage();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Refresh Status"))
            {
                CheckPackageStatus();
            }
            
            GUILayout.Space(10);

            GUI.enabled = EnableGUI();
            if (GUILayout.Button("Increment Version"))
            {
                IncrementVersion();
            }

            if (GUILayout.Button("Publish New Version"))
            {
                PublishNewVersion();
            }

            GUILayout.EndHorizontal();
        }

        private bool EnableGUI()
        {
            return _packageStatus == PackageStatus.Local;
        }

        private void SwitchToPackage()
        {
            listRequest = Client.List(true); // List all packages including local ones
            EditorApplication.update += ListPackagesProgress;
        }

        private void ListPackagesProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    bool packageFound = false;
                    foreach (var package in listRequest.Result)
                    {
                        if (package.name == targetObject.packageName)
                        {
                            packageFound = true;
                            removeRequest = Client.Remove(targetObject.packageName);
                            EditorApplication.update += RemovePackageProgress;
                            break;
                        }
                    }

                    if (!packageFound)
                    {
                        AddPackage(targetPackagePath);
                    }
                }
                else if (listRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError(listRequest.Error.message);
                }

                EditorApplication.update -= ListPackagesProgress;
            }
        }

        private void RemovePackageProgress()
        {
            if (removeRequest.IsCompleted)
            {
                if (removeRequest.Status == StatusCode.Success)
                {
                    AddPackage(targetPackagePath);
                }
                else if (removeRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError(removeRequest.Error.message);
                }

                EditorApplication.update -= RemovePackageProgress;
            }
        }

        private void AddPackage(string packagePath)
        {
            addRequest = Client.Add(packagePath);
            EditorApplication.update += AddPackageProgress;
        }

        private void AddPackageProgress()
        {
            if (addRequest.IsCompleted)
            {
                if (addRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Package successfully added: " + addRequest.Result.packageId);
                }
                else if (addRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError(addRequest.Error.message);
                }

                EditorApplication.update -= AddPackageProgress;
                CheckPackageStatus(); // Refresh package status after adding
            }
        }

        private void CheckPackageStatus()
        {
            listRequest = Client.List(true);
            EditorApplication.update += CheckPackageStatusProgress;
        }

        private void CheckPackageStatusProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    foreach (var package in listRequest.Result)
                    {
                        if (package.name == targetObject.packageName)
                        {
                            _version = package.version;
                            if (package.source == PackageSource.Local)
                            {
                                _packageStatus = PackageStatus.Local;
                            }
                            else if (package.source == PackageSource.Git)
                            {
                                _packageStatus = PackageStatus.Git;
                            }
                            return;
                        }
                    }
                    _packageStatus = PackageStatus.NotInstalled;
                }
                else if (listRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError(listRequest.Error.message);
                }

                EditorApplication.update -= CheckPackageStatusProgress;
            }
        }

        private void IncrementVersion()
        {
            string packageJsonPath = Path.Combine(targetObject.localFilePath, "package.json");
            if (File.Exists(packageJsonPath))
            {
                string json = File.ReadAllText(packageJsonPath);
                var packageData = JsonUtility.FromJson<PackageData>(json);

                Version version;
                if (Version.TryParse(packageData.version, out version))
                {
                    version = new Version(version.Major, version.Minor, version.Build + 1);
                    packageData.version = version.ToString();
                    _version = version.ToString();
                    json = JsonUtility.ToJson(packageData, true);
                    File.WriteAllText(packageJsonPath, json);
                    Debug.Log("Version incremented to: " + _version);

                    // Trigger reimport of the package
                    AssetDatabase.ImportAsset(packageJsonPath, ImportAssetOptions.ForceUpdate);
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogError("Invalid version format.");
                }
            }
            else
            {
                Debug.LogError("package.json not found at " + packageJsonPath);
            }
        }

        private void PublishNewVersion()
        {
            string packageJsonPath = Path.Combine(targetObject.localFilePath, "package.json");
            if (File.Exists(packageJsonPath))
            {
                // Execute git commands
                // RunGitCommand("add " + packageJsonPath);
                RunGitCommand("add -all" );
                RunGitCommand("commit -m \"Update package version to " + _version + "\"");
                RunGitCommand("push");
                Debug.Log("New version published to the Git repository.");
            }
            else
            {
                Debug.LogError("package.json not found at " + packageJsonPath);
            }
        }

        private void RunGitCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git")
            {
                WorkingDirectory = targetObject.localFilePath,
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (sender, args) => Debug.Log(args.Data);
                process.ErrorDataReceived += (sender, args) => Debug.LogError(args.Data);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        [Serializable]
        private class PackageData
        {
            public string name;
            public string version;
            public string displayName;
            public string description;
            public string unity;
            public Author author;
        }

        [Serializable]
        private class Author
        {
            public string name;
            public string email;
            public string url;
        }
    }
}
