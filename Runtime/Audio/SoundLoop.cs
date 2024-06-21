using RichardPieterse.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace RichardPieterse.Audio
{
    [CreateAssetMenu(menuName = "Audio/Sound Loop")]
    public class SoundLoop : ScriptableObject
    {
        [SerializeField] private AudioMixerGroup _mixer;
        [SerializeField] private AudioClip _clip;
        [Range(0,1 )]
        [SerializeField] private float _volumeMultiplier = 1;
        [SerializeField] private SmartCurve _speedVolumeCurve = new SmartCurve(1) ;
        [SerializeField] private SmartCurve _speedPitchCurve = new SmartCurve(1) ;

        
        public AudioMixerGroup mixer => _mixer;

        public AudioClip GetClip()
        {
            return _clip;
        }

        public float GetVolume(float speed = 1)
        {
            return _volumeMultiplier*_speedVolumeCurve.Evaluate(speed);
        }
            
        
        public float GetPitch(float speed = 1)
        {
            return _volumeMultiplier*_speedPitchCurve.Evaluate(speed);
        }
    }
}