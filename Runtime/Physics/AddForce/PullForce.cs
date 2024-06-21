using System;
using System.Collections;
using System.Collections.Generic;
using RichardPieterse;
using UnityEngine;

[ExecuteInEditMode]
public class PullForce : AddForceBase
{
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    [SerializeField] private bool _applyForce = true;

    void FixedUpdate()
    {
        if (_applyForce && Application.isPlaying )
        {
            Pull();
            
        }
    }

    public void Pull()
    {

        if (_rigidbody)
        {
            _rigidbody.AddForceAtPosition(force*_multiplier*lerp, transform.position, _forceMode);
        }
    }
}