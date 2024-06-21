using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace RichardPieterse.Audio
{
    [CreateAssetMenu(menuName = "Audio/Sound")]
    public class SoundAsset : ScriptableObject
    {
        [SerializeField] private AudioMixerGroup _mixer;
        [SerializeField] private AudioClip[] _clips;
        [Range(0,1 )]
        [SerializeField] private float _volumeMultiplier = 1;

        public AudioMixerGroup mixer => _mixer;

        public AudioClip GetClip()
        {
            int index = Random.Range(0, _clips.Length);
            return _clips[index];
        }

        public float GetVolume()
        {
            return _volumeMultiplier;
        }
    }

}