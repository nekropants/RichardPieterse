using UnityEngine;

public class AddForceBase : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Vector3 _force = Vector3.forward;
    [SerializeField] protected float _multiplier = 1f;
    [SerializeField] protected bool _localSpace = false;

    [Space]
    [SerializeField] protected bool _drawArrow = true;
    [SerializeField] protected float _intendedForceRange = 1;
    [SerializeField] protected Vector3 _arrowOffset;
    [SerializeField] protected Arrow _debugArrow;
    private float _lerp = 1;

    public Vector3 force
    {
        set => _force = value;
        get
        {
            if (_localSpace)
            {
                return transform.TransformDirection(_force);
            }

            return _force;
        }
    }
   
    public float lerp
    {
        get => _lerp;
        set => _lerp = value;
    }

    protected virtual void LateUpdate()
    {
        UpdateArrow();
    }

    private void UpdateArrow()
    {
        if (_debugArrow == null)
        {
            _debugArrow = GizmoUtility.CreateArrowGizmo(this);
            _debugArrow.transform.SetParent(transform);
        }

        if (force.magnitude != 0)
        {
            _debugArrow.transform.forward = force * Mathf.Sign(force.magnitude);
        }
        _debugArrow.length = Mathf.Abs(force.magnitude) * _multiplier / _intendedForceRange;
        _debugArrow.transform.position = transform.position + _arrowOffset;

        _debugArrow.gameObject.SetActive(_drawArrow);
    }
}