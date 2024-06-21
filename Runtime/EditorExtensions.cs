using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RichardPieterse
{
    public static class EditorExtensions
    {
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