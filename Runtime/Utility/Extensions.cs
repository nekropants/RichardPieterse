using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RichardPieterse
{
    public static class Extensions
    {
        
        public static Vector3 MultiplyComponentWise(this Vector3 vector, Vector3 operand)
        {
            return new Vector3(vector.x * operand.x, vector.y * operand.y, vector.z * operand.z);
        }

    public static Vector3 MultiplyComponentWise(this Vector3 vector, float x, float y, float z)
    {
        return new Vector3(vector.x * x, vector.y * y, vector.z * z);
    }

    public static Vector2 MultiplyComponentWise(this Vector2 vector, Vector2 operand)
    {
        return new Vector2(vector.x * operand.x, vector.y * operand.y);
    }

    public static Vector2 MultiplyComponentWise(this Vector2 vector, float x, float y)
    {
        return new Vector2(vector.x * x, vector.y * y);
    }

    public static Vector3 DivideComponentWise(this Vector3 vector, Vector3 operand)
    {
        return new Vector3(vector.x / operand.x, vector.y / operand.y, vector.z / operand.z);
    }

    public static Vector2 DivideComponentWise(this Vector2 vector, Vector2 operand)
    {
        return new Vector2(vector.x / operand.x, vector.y / operand.y);
    }

    public static Vector3 Rounded(this Vector3 vector)
    {
        return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }
    
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
        
        public static Vector3 WithXZ(this Vector3 vector, Vector3 value)
        {
            vector.x = value.x;
            vector.z = value.z;
            return vector;
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

        public static void DestroyChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
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
        
         public static string ElementsToString<T>( this T[] _array  )
        {
            string s = "";
            if (_array != null)
            {
                for (int i = 0; i < _array.Length; i++)
                {
                    s += _array[i].ToString() + ", ";
                }
            }

            return s;
        }


         #if Editor
        public static T GetComponent<T>(this Scene scene) where T : Component
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                T result = rootGameObjects[i].GetComponentInChildren<T>();

                if (result)
                    return result;
            }

            return null;
        }

        public static List<T> GetComponents<T>(this Scene scene) where T : Component
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            List<T> list = new List<T>();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                T[] result = rootGameObjects[i].GetComponentsInChildren<T>();
                list.AddRange(result);
            }

            return list;
        }
        
        public static GameObject FindGameObjectByName(this Scene scene, string name)
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {

                GameObject result = FindGameObjectByName(rootGameObjects[i], name);
                if (result)
                    return result;
            }

            return null;
        }
#endif

        public static GameObject FindGameObjectByName(this GameObject parent, string name)
        {
            if (parent.name.Equals(name))
            {
                return parent;
            }

            foreach (Transform child in parent.transform)
            {
                GameObject result = child.gameObject.FindGameObjectByName(name);
                if (result)
                    return result;
            }

            return null;
        }
    }
}
