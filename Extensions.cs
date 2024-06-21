using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RichardPieterse
{
    public static class Extensions
    {
        public static Vector3 To(this Vector3 from, Vector3 to)
        {
            return to - from;
        }
        
        public static Vector3 MidPoint(this Vector3 from, Vector3 to)
        {
            return (to + from)/2;
        }

        
        public static T GetOrAddComponent<T>(this GameObject uo) where T : Component
        {
            var component = uo.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            return uo.AddComponent<T>();
        }
    
        public static T GetOrAddComponent<T>(this Component uo) where T : Component
        {
            var component = uo.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            return uo.gameObject.AddComponent<T>();
        }

        
        public static float DistanceTo(this Vector3 from, Vector3 to)
        {
            return (to - from).magnitude;
        }
        
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        
        public static Vector3 WithX(this Vector3 vector, float value)
        {
            vector.x = value;
            return vector;
        }
        
        public static Vector3 WithY(this Vector3 vector, float value)
        {
            vector.y = value;
            return vector;
        }
        
        public static Vector3 WithZ(this Vector3 vector, float value)
        {
            vector.z = value;
            return vector;
        }
        
        
        public static JointDrive WithPositionSpring(this JointDrive drive, float spring)
        {
            drive.positionSpring = spring;
            return drive;
        }
        
        
        public static JointDrive WithPositionDamper(this JointDrive drive, float damper)
        {
            drive.positionDamper = damper;
            return drive;
        }
        
          
        public static void SetProperty(this Renderer meshRenderer, string name, float value)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat(name, value);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
        
        public static void SetProperty(this Renderer meshRenderer, string name, Vector4 value)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector(name, value);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        
        public static string GetHierarchyPath(this Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }
        
        public static   List<T> GetComponentsInChildren<T>(this GameObject[] gameObjects) where T : Component
        {
            List<T> results = new List<T>();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                T[] components = gameObjects[i].GetComponentsInChildren<T>();
                results.AddRange(components);
            }

            return results;
        }
        
        public static bool IsVisible(this Camera camera, Vector3 point,bool raycast =false, int layerMaskOfBlockingObjects = 0,  float margin = 0)
        {
            Vector3 viewPoint = camera.WorldToViewportPoint(point);

            if (viewPoint.x > 1 + margin || viewPoint.y > 1 + margin || viewPoint.z > camera.farClipPlane)
                return false;
            
            if (viewPoint.x < 0 - margin || viewPoint.y < 0 - margin || viewPoint.z <  camera.nearClipPlane)
                return false;

            Vector3 vector3 = camera.transform.position.To(point);
            if( raycast && Physics.Raycast(camera.transform.position, vector3.normalized, out RaycastHit hit, vector3.magnitude, layerMaskOfBlockingObjects) )
            {
                return false;
            }

            return true;
        }

        public static void SetEnabled(this Object obj, bool enabled)
        {
            if (obj is MonoBehaviour comp)
            {
                comp.enabled = enabled;
            }
            else if (obj is GameObject go)
            {
                go.SetActive(enabled);

            }
        }
        
        public static Transform GetPreviousSibling(this Transform transform)
        {
            if (transform.parent == null)
            {
                return null;
            }

            int index = transform.GetSiblingIndex();
            if (index == 0)
            {
                return null;
            }

            return transform.parent.GetChild(index - 1);
        }
    }
}
