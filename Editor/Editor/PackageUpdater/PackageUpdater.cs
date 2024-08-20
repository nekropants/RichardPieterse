using System.IO;
using UnityEngine;


namespace RichardPieterse
{
    [CreateAssetMenu(fileName = "PackageUpdater", menuName = MenuPaths.CREATE_MENU + "/PackageUpdater")]
    public class PackageUpdater : ScriptableObject
    {
        
        [SerializeField] private string _packageName = "com.nekropants.framework";

        private Preference<string> _localFilePath = null;
        // [SerializeField] private string _localFilePath = "/Users/rpieterse/Documents/GitHub/RichardPieterse";
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
             get
             {
                 LazyInitilizeLocalFilePath();
                 return _localFilePath.value;
             }
             set
             {
                 LazyInitilizeLocalFilePath();
                 _localFilePath.value = value;
             }
         }

         private void LazyInitilizeLocalFilePath()
         {
             if (_localFilePath == null)
             {
                 string defaultPath =
                     Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
                         "Repositories");
                 _localFilePath = new Preference<string>(this.name+"_localFilePath", defaultPath);
             }
         }
         
    }
}