using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RichardPieterse
{
    public static class DeveloperPreferences
    {
        public class SettingDrawer
        {
            public Action onGUI;
            public string[] keywords;
        }
        
        private static List<SettingDrawer> _settingsDrawers = new List<SettingDrawer>(); 
        
        
        public static void RegisterSettingDrawer(SettingDrawer drawer)
        {
            _settingsDrawers.Add(drawer);
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            HashSet<string> keywords = new System.Collections.Generic.HashSet<string>();
            foreach (SettingDrawer settingsDrawer in _settingsDrawers)
            {
                foreach (string keyword in settingsDrawer.keywords)
                {
                    keywords.Add(keyword);
                }
            }

            var provider = new SettingsProvider($"Preferences/{Application.companyName}", SettingsScope.User)
            {
                label = Application.companyName,
                guiHandler = (searchContext) =>
                {
                    foreach (SettingDrawer settingsDrawer in _settingsDrawers)
                    {
                        settingsDrawer.onGUI.Invoke();
                    }
                    
                },
                
                // Optional keyword search
                keywords = keywords
            };

            return provider;
        }
    }
}