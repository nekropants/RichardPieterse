using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RichardPieterse.Audio
{
   public class PlaySound : MonoBehaviour
   {
      [SerializeField] private AudioSource _audioSource;
      [SerializeField] private SoundAsset _soundAsset;

      public void Play()
      {
         _audioSource.outputAudioMixerGroup = _soundAsset.mixer;
         _audioSource.PlayOneShot(_soundAsset.GetClip(), _soundAsset.GetVolume());
      }
   }
}
