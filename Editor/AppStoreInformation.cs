using System;
using UnityEngine;

namespace Plugins.RichFramework.Editor
{
    [Serializable]
    public class AppStoreInformation
    {
        [TextArea(3, 40)]
        [SerializeField] private string _description;
        [TextArea(2,40)]
        [SerializeField] private string _privacyPolicy;
        [SerializeField] private string _copyright = "2024 Nekropants. All rights reserved.";
    }
}