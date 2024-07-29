using System;
using RichardPieterse;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeyondThePines
{
    public class SceneTransition : MonoBehaviour
    {
       [SerializeField]  private SceneReference _sceneToLoad;
       [SerializeField]  private float _fadeDuration;

       private bool _triggered = false;

       private void Trigger2()
       {
           Debug.Log("Triggered");
       }

       public void Trigger()
       {
           if (_triggered == false)
           {
               _triggered = true;
               TransitionFadeToBlack.DoFadeTransition(_fadeDuration,
                   () => { SceneManager.LoadScene(_sceneToLoad.SceneName); }, null);
           }
       }
    }
}