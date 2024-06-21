using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiallyDisableColliders : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private float _delay;
    private IEnumerator Start()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_delay);
        _collider.enabled = true;
    }

    void Update()
    {
        
    }
}
