namespace RichardPieterse
{
    using System;
    using System.Numerics;
    using RichardPieterse;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;
    
    public class AlignAxis : MonoBehaviour
    {
        private Rigidbody _rigidbody;
            
        [SerializeField] private float _uprightJointSpringStrength = 1;
        [Range(0,1)]
        [SerializeField] private float _uprightJointSpringDamper = 0.1f;
        [SerializeField] private Vector3 _localAxis = Vector3.up;
        [SerializeField] private Vector3 _worldAxisTarget = Vector3.up;
        [Space]
        [Min(0.001f)] 
        [SerializeField] private float _intendedForceRange = 10;
        private Vector3 _torque;
    
        public Vector3 worldAxisTarget
        {
            get => _worldAxisTarget;
            set => _worldAxisTarget = value;
        }
    
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    
        void FixedUpdate()
        {
            RotateTowardsTarget();
        }
    
        /// <summary>
        /// Rotates the object towards a target world axis by applying a torque calculated using spring physics.
        /// </summary>
        void RotateTowardsTarget()
        {
            // Convert the local axis to a world axis
            Vector3 worldAxis = transform.TransformDirection(_localAxis);
    
            // Calculate the axis of rotation needed to align the current world axis with the target world axis
            Vector3 rotationAxis = Vector3.Cross(worldAxis, worldAxisTarget);
            float angle = Vector3.Angle(worldAxis, worldAxisTarget);
    
            // Calculate the spring force (torque) using Hooke's law
            // The torque is proportional to the angle difference and the spring strength
            _torque = rotationAxis * (angle * _uprightJointSpringStrength);
    
            // Calculate the damping torque to reduce oscillations
            // This torque is proportional to the negative of the current angular velocity and the damping coefficient
            Vector3 dampingTorque = -_rigidbody.angularVelocity * _uprightJointSpringDamper;
    
            // Apply the calculated torques (spring torque + damping torque) to the rigidbody
            _rigidbody.AddTorque(_torque + dampingTorque);
        }
    
    
        private void OnDrawGizmos()
        {
            if (enabled == false)
            {
                return;
            }
            
            Gizmos.color = Color.blue;
            Vector3 worldAxis = transform.TransformDirection(_localAxis);
            Gizmos.DrawRay(transform.position, worldAxis * 0.05f);
            Gizmos.DrawRay(transform.position, worldAxisTarget * 0.05f);
            Gizmos.DrawRay(transform.position, _torque / _intendedForceRange);
            Gizmos.DrawSphere(transform.position + _torque / _intendedForceRange, 0.02f);
            GizmoUtility.DrawArrowFromPosition(transform.position, worldAxisTarget * 0.1f, 0.01f);
        }
    
        public void LookAt(Vector3 lookTargetPosition)
        {
            _worldAxisTarget = transform.position.To(lookTargetPosition).normalized;    
        }
    }
}
