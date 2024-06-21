using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PolySpatial;
using UnityEngine;


[RequireComponent(typeof(VolumeCamera))]
public class VolumeHelper : MonoBehaviour
{
   [SerializeField] public float scale = 1;
   public VolumeCamera volumeCamera => GetComponent<VolumeCamera>();

   private void OnValidate()
   {
      volumeCamera.Dimensions = volumeCamera.WindowConfiguration.Dimensions/scale;
      transform.position = Vector3.up*volumeCamera.Dimensions.y/2f;
   }

   private void OnDrawGizmos()
   {
      OnValidate();
   }
   //  192.168.1.72
}
