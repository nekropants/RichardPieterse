    using System;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;


    namespace RichardPieterse
{
    [CreateAssetMenu(fileName = "PackageUpdater", menuName = MenuPaths.CREATE_MENU + "/PackageUpdater")]
    public class PackageUpdater : ScriptableObject
    {
        
        [SerializeField] private string _packageName = "com.nekropants.framework";
        [SerializeField] private string _localFilePath = "/Users/rpieterse/Documents/GitHub/RichardPieterse";
        [SerializeField] private string _gitRepoUrl = "https://github.com/nekropants/RichardPieterse.git";
   
      
         public string packageName
         {
             get => _packageName;
             set => _packageName = value;
         }

         public string localPackagePath
         {
             get => "file:"+localFilePath;
         }

         public string gitRepoUrl
         {
             get => _gitRepoUrl;
             set => _gitRepoUrl = value;
         }

         public string localFilePath
         {
             get => _localFilePath;
             set => _localFilePath = value;
         }
    }
}