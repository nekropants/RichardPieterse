using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AddMeshColliders 
{
    [MenuItem("CONTEXT/Transform/Add Mesh Colliders To Children")]
    static void AddMeshCollidersToChildren(MenuCommand command)
    {
        Component component = command.context as Component;
        MeshFilter[] componentsInChildren = component.gameObject.GetComponentsInChildren<MeshFilter>();
        foreach (var filter in componentsInChildren)
        {
            MeshCollider meshCollider = filter.GetComponent<MeshCollider>();
            if (meshCollider)
            {
                GameObject.Destroy(meshCollider);
            }
            meshCollider = filter.gameObject.AddComponent<MeshCollider>();
        }
    }
}
