using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RichardPieterse.Audio
{
   public class PlaySoundLoop : MonoBehaviour
   {
      [SerializeField]  private float _currentSpeed;
      [SerializeField] private AudioSource _audioSource;
      [SerializeField] private SoundLoop _loopAsset;
      [SerializeField] private AnimationCurve _curve;
      private List<Keyframe> keys = new List<Keyframe>();

      private void OnValidate()
      {
         if (Application.isPlaying)
         {
            SetSpeed(_currentSpeed);
         }
      }

      public void Awake()
      {
         _audioSource.clip = _loopAsset.GetClip();
         _audioSource.Play(); 
         _audioSource.outputAudioMixerGroup = _loopAsset.mixer;
         _audioSource.loop = true;
      }
      
      public void SetSpeed(float speed)
      {

         _currentSpeed = Mathf.Lerp(_currentSpeed, speed, 0.5f);
         _audioSource.volume = _loopAsset.GetVolume(_currentSpeed);
         _audioSource.pitch = _loopAsset.GetPitch(_currentSpeed);
         AddKey(_currentSpeed);

      }
      
      
      public  void AddKey(float value)
      {
         if (Application.isEditor)
         {
            keys.Add(new Keyframe(Time.frameCount, value));
            _curve.keys = keys.ToArray();
         }
      }
   }
}
