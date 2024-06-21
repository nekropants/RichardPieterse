namespace RichardPieterse
{
    using UnityEngine;
    
    public class CenterOfMassGizmo : MonoBehaviour
    {
        [SerializeField] private float _scale =1f;
    
        private Rigidbody _rigidbody;
    
        void OnValidate()
        {
        }
    
      
    
        void OnDrawGizmos()
        {
    
            float width = 0.001f*_scale;
            float height = 0.01f*_scale;
    
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
    
            if (_rigidbody != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_rigidbody.worldCenterOfMass, new Vector3(height,width, width));
                Gizmos.DrawCube(_rigidbody.worldCenterOfMass, new Vector3(width,height, width));
                Gizmos.DrawCube(_rigidbody.worldCenterOfMass, new Vector3(width,width, height));
            }
            else
            {
                enabled = false;
            }
        }
    }
}
