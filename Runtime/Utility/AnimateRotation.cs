namespace RichardPieterse
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ProBuilder.MeshOperations;
    using UnityEngine.Serialization;
    
    public class AnimateRotation : MonoBehaviour
    {
        public enum Motion
        {
            Sin,
            Spin
        }
    
        [Range(0,1)]
        [SerializeField] public float lerp = 1f; 
        
        [SerializeField] private Motion _motion =0;
        [FormerlySerializedAs("_piOffset")] [SerializeField] private float _timeOffset =0;
        [SerializeField] private float _frequency =1;
        [SerializeField] private float _amplitude =20 ;
        [SerializeField] private Vector3 _euler = Vector3.up ;
         private Quaternion _offset = Quaternion.identity;
    
         
         
        public void Update()
        {
            transform.localRotation *= Quaternion.Inverse(_offset);
            
            if(_motion == Motion.Sin)
            {
                var amplitude = (Mathf.Sin(Time.time * _frequency + _timeOffset*Mathf.PI) * _amplitude);
                _offset = Quaternion.Euler(_euler.normalized * (amplitude * lerp));
            }
            else
            {
                _offset = Quaternion.Euler(_euler.normalized * ((Time.time*_frequency + _timeOffset) * lerp));
            }
      
            transform.localRotation *= _offset;
        }
    }
}
