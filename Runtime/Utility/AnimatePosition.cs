namespace RichardPieterse
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;
    
    public class AnimatePosition : MonoBehaviour
    {
        public enum Motion
        {
            Sin,
        }
    
        [Range(0,1)]
        [SerializeField] public float lerp = 1f; 
        
        [SerializeField] private Motion _motion =0; 
        [SerializeField] private float _timeOffset =0;
        [SerializeField] private float _frequency =1;
        [SerializeField] private float _amplitude =1 ;
        [SerializeField] private Vector3 direction = Vector3.up ;
         private Vector3 _offset = Vector3.zero;
    
         
        public void Update()
        {
            transform.localPosition -= _offset;
            
            if(_motion == Motion.Sin)
            {
                var amplitude = (Mathf.Sin(Time.time * _frequency + _timeOffset*Mathf.PI) * _amplitude);
                _offset = direction.normalized * (amplitude * lerp);
            }
          
            transform.localPosition += _offset;
        }
    }
}
