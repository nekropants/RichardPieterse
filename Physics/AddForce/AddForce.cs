using System;
using System.Collections;
using System.Collections.Generic;
using RichardPieterse;
using UnityEngine;


[ExecuteInEditMode]
public class AddForce : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _force = Vector3.zero;
    [SerializeField] private float _multiplier = 1f;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    [SerializeField] private bool _applyForce;
    [SerializeField] private bool _momentary;
    [SerializeField] private bool _localSpace = false;
    [Space]
    [SerializeField] private bool _drawArrow = true;

    [SerializeField] private float _intendedForceRange = 1;
    [SerializeField] private Vector3 _arrowOffset;
    [SerializeField] private Arrow _debugArrow;
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

    
    private void LateUpdate()
    {
        UpdateArrow();
    }

    void UpdateArrow()
    {
        if (_drawArrow)
        {
            if (_debugArrow == null)
            {
                _debugArrow = RuntimeEditorHelper.InstantiatePrefabAsset<Arrow>("Prefab_Arrow");
                _debugArrow.gameObject.hideFlags |= HideFlags.DontSave;
                _debugArrow.transform.SetParent( transform);
            }

            _debugArrow.transform.forward = force*Mathf.Sign(force.magnitude);
            _debugArrow.length =  Mathf.Abs(force.magnitude) * _multiplier/_intendedForceRange;
            _debugArrow.gameObject.SetActive(_drawArrow);
            _debugArrow.transform.position = transform.position + _arrowOffset;
            
        }
    }

    void FixedUpdate()
    {
        if (_applyForce && Application.isPlaying )
        {
            ApplyForce();
            
            if (_momentary)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ApplyForce()
    {
        _rigidbody.AddForceAtPosition(force*_multiplier, transform.position, _forceMode);
    }
}
