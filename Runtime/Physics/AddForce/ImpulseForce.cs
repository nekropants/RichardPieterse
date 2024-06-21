using UnityEngine;

[ExecuteInEditMode]
public class ImpulseForce : AddForceBase
{

    [SerializeField] private bool _disableOnTrigger;
    public void ApplyForce()
    {
        if (_rigidbody)
        {
            _rigidbody.AddForceAtPosition(force*_multiplier*lerp, transform.position, ForceMode.Impulse);

            if (_disableOnTrigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}