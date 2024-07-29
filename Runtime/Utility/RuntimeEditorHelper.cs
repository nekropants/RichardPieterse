using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

using System.Collections;
using System.Collections.Generic;
using System;

namespace RichardPieterse
{

        public class CacheGuiEnabled : IDisposable
        {
                private bool cachedValue = false;

                public CacheGuiEnabled(bool enabled)
                {
                        cachedValue = GUI.enabled;
                        GUI.enabled = enabled;
                }

                public void Dispose()
                {
                        GUI.enabled = cachedValue;
                }

                ~CacheGuiEnabled()
                {
                }
        }
   

        public static class RuntimeEditorHelper
    {
            public static CacheGuiEnabled SetGuiEnabled(bool enabled)
            {
                    return new CacheGuiEnabled(enabled);
            }
            public static CacheGuiEnabled DisableGUI()
            {
                    return new CacheGuiEnabled(false);
            }
            public static CacheGuiEnabled EnableGUI()
            {
                    return new CacheGuiEnabled(true);
            }

            public static void RegisterCreatedObjectUndo(Object target, string name)
            {
#if UNITY_EDITOR
                    Undo.RegisterCreatedObjectUndo(target, name);
#endif
            }

            public static void RecordObjectUndo(Object target, string name)
            {
#if UNITY_EDITOR
                    if (target)
                            Undo.RecordObject(target, name);
#endif
            }

            public static void SetDirty(Object target)
        {
#if UNITY_EDITOR
            if (target)
                EditorUtility.SetDirty(target);
#endif
        }
        
        public static void ClearDirty(Object target)
        {
#if UNITY_EDITOR
            if (target)
                EditorUtility.ClearDirty(target);
#endif
        }
        
        public static void SelectAndFocus(Object selection)
        {
#if UNITY_EDITOR
                Selection.activeObject = selection;
                if (SceneView.sceneViews.Count > 0) SceneView.FrameLastActiveSceneView();
#endif
        }

        public static bool SelectionContains<T>() where T : MonoBehaviour
        {
#if UNITY_EDITOR
                foreach (var child in Selection.objects)
                        if (child is GameObject data)
                        {
                                var component = data.GetComponent<T>();
                                if(component)
                                        return true;
                        }
#endif
                return false;
        }


        public static void ValidateLayerDelayed(this GameObject gameObject, string layerName)
        {
                if (gameObject.layer != LayerMask.NameToLayer(layerName))
                {
#if UNITY_EDITOR

                         if (Application.isPlaying == false)
                        {
                                UnityEditor.EditorApplication.delayCall += () =>
                                {
                                        if (gameObject != null) 
                                                gameObject.layer = LayerMask.NameToLayer(layerName);
                                };
                        }
#endif
                }
        }


        public static bool ShouldValidatePrefabInstance(GameObject gameObject)
        {



#if UNITY_EDITOR

                return !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;


#else
                return IsPrefabAsset(gameObject) == false;
#endif

        }

        public static bool IsPrefabAsset(GameObject gameObject)
        {
                if (string.IsNullOrEmpty(gameObject.scene.path))
                {
                        return true;
                }

                return false;
        }

#if UNITY_EDITOR
    public static List<T> FindObjectsOfType<T>() where T : Component
    {
            List<T> results = new List<T>();
            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                    Scene scene = EditorSceneManager.GetSceneAt(i);
                    results.AddRange(scene.GetRootGameObjects().GetComponentsInChildren<T>());
            }

            return results;
    }
#endif

        public static IEnumerable<T> EnumerateSelection<T>() where T : MonoBehaviour
        {
#if UNITY_EDITOR
                foreach (var child in Selection.objects)
                        if (child is GameObject data)
                        {
                                var component = data.GetComponent<T>();
                                if(component)
                                        yield return component;
                        }

#else
                yield return null;
#endif
        }
        
        public static void SelectInProjectView(Object asset)
        {
#if UNITY_EDITOR
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
#endif
        }

        public static void PingSelection()
        {
#if UNITY_EDITOR
                for (var i = 0; i < Selection.count; i++) EditorGUIUtility.PingObject(Selection.objects[i]);
#endif
        }
        public static void Ping(Component component)
        {
#if UNITY_EDITOR
                EditorGUIUtility.PingObject(component);
#endif
        }
        
        public static void Ping(Object obj)
        {
#if UNITY_EDITOR
                EditorGUIUtility.PingObject(obj);
#endif
        }
        
        public static void Ping(List<GameObject> objects)
        {
#if UNITY_EDITOR
                for (var i = 0; i < objects.Count; i++) EditorGUIUtility.PingObject(objects[i]);
#endif
        }
        
        public static void Ping(List<Object> objects)
        {
#if UNITY_EDITOR
                for (var i = 0; i < objects.Count; i++) EditorGUIUtility.PingObject(objects[i]);
#endif
        }

        public static void Select<T>(T selection) where T : Object        
        {
#if UNITY_EDITOR
                        Selection.activeObject = selection;
#endif
        }
        

        public static void Select(List<GameObject> selection)
        {
#if UNITY_EDITOR
                Selection.objects = selection.ToArray();
#endif
        }
        
        public static void Select<T>(List<T> selection) where T : Component
        {
                var gos = new GameObject[selection.Count];
                for (var i = 0; i < selection.Count; i++)
                        if (selection[i])
                                gos[i] = selection[i].gameObject;

#if UNITY_EDITOR
                Selection.objects = gos;
#endif
        }
        
        public static void Select<T>(params T[] selection) where T : Component
        {
                var gos = new GameObject[selection.Length];
                for (var i = 0; i < selection.Length; i++)
                        if (selection[i])
                                gos[i] = selection[i].gameObject;

#if UNITY_EDITOR
                Selection.objects = gos;
#endif
        }   
        
 
        
        public static void SelectAndFocus<T>(List<T> selection) where T : Object
        {
                var gos = new Object[selection.Count];
                for (var i = 0; i < selection.Count; i++)
                        if (selection[i])
                                gos[i] = selection[i];

#if UNITY_EDITOR
                Selection.objects = gos;
                
                if (SceneView.lastActiveSceneView)
                        SceneView.lastActiveSceneView.FrameSelected();
#endif
        }
        
        public static void Focus<T>(List<T> selection) where T : Object
        {
#if UNITY_EDITOR
                var cache = Selection.objects;
                SelectAndFocus(selection);
                Selection.objects = cache;
#endif
        }
        
        public static void Focus<T>(T selection) where T : Object
        {
#if UNITY_EDITOR
                
                var cache = Selection.objects;
                Selection.activeObject = selection;
                if (SceneView.sceneViews.Count > 0) 
                        SceneView.FrameLastActiveSceneView();
                SelectAndFocus(selection);
                Selection.objects = cache;
#endif
        }

        public static void SelectAndFocus<T>(T[] selection) where T : Object
        {
                var gos  = new Object[selection.Length];
                for (var i = 0; i < selection.Length; i++) gos[i] = selection[i];

#if UNITY_EDITOR
                Selection.objects = gos;
                if ( SceneView.lastActiveSceneView) SceneView.lastActiveSceneView.FrameSelected();
#endif

        }
#if UNITY_EDITOR
        public static T GetSerializedValue<T>(this PropertyDrawer propertyDrawer, SerializedProperty property)
        {
                var @object = propertyDrawer.fieldInfo.GetValue(property.serializedObject.targetObject);

                // UnityEditor.PropertyDrawer.fieldInfo returns FieldInfo:
                // - about the array, if the serialized object of property is inside the array or list;
                // - about the object itself, if the object is not inside the array or list;

                // We need to handle both situations.
                if (((IList) @object.GetType().GetInterfaces()).Contains(typeof(IList<T>)))
                {
                        var propertyIndex = int.Parse(property.propertyPath[property.propertyPath.Length - 2].ToString());

                        return ((IList<T>) @object)[propertyIndex];
                }
                else
                {
                        return (T) @object;
                }
        }
#endif

        public static void EditorApplicationDelayCall(Action action)
        {
#if UNITY_EDITOR
  UnityEditor.EditorApplication.delayCall+=()=>
                {
                        action?.Invoke();
                };

#endif
        }
        public static void UndoDestroyObjectImmediate(GameObject gameObject)
        {
                if(gameObject == null)
                        return;

#if UNITY_EDITOR
  Undo.DestroyObjectImmediate(gameObject);
#endif

        }

        public static void SetTransformParent(Transform transform, Transform newParent, bool worldPositionStays,
                string name)
        {

#if UNITY_EDITOR
                Undo.SetTransformParent(transform, newParent, worldPositionStays, name);
#endif
        }

        public static List<T> FindAssetsOfType<T>() where T : Object
        {
                var list = new List<T>();
                
#if UNITY_EDITOR
                var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

                foreach (var guid in guids) 
                        list.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
#endif

                return list;
        }

        public static T FindAsset<T>() where T : Object
        {
#if UNITY_EDITOR
                var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

                if(guids.Length > 0)
                        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
#endif

                return null;
        }
     
                
        public static List<T> FindAssets<T>( ) where T : Object
        {
                var list = new List<T>();
#if UNITY_EDITOR
                var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
                foreach (var guid in guids) 
                        list.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
#endif

                return list;
        }
        
        

        public static Material GetDefaultMaterial()
        {
                Material material = null;
#if UNITY_EDITOR
                material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

#endif
                return material;
        }

        public static List<T> FindAssets<T>(string[] searchInFolders) where T : Object
        {
                var list = new List<T>();
#if UNITY_EDITOR
                var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", searchInFolders);
                foreach (var guid in guids) 
                        list.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
#endif

                return list;
        }

        public static T FindAsset<T>(string name, bool includeTypeInSearch = true) where T : Object
        {
#if UNITY_EDITOR
                string search = name;
                
                
                if(includeTypeInSearch)
                        search = $"t:{typeof(T).Name} {name}";
                
                var guids = AssetDatabase.FindAssets(search);


                if (guids.Length > 0)
                {
                        foreach (var guid in guids)
                        {
                                string path = AssetDatabase.GUIDToAssetPath(guid);
                                // find exact match
                                string nameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                                if (nameWithoutExtension.Equals(name))
                                {
                                        var assetAtPath = AssetDatabase.LoadAssetAtPath<T>(path);
                                        return assetAtPath;
                                }
                        }
                }
#endif
                return null;
        }
        public static T FindAsset2<T>(string name, bool includeTypeInSearch = true) where T : Object
        {
#if UNITY_EDITOR
                string search = name;
                
                if(includeTypeInSearch)
                        search = $"t:{typeof(T).Name} {name}";
                
                var guids = AssetDatabase.FindAssets(search);


#endif
                return null;
        }

        public static bool IsSelected(GameObject gameObject)
        {
#if UNITY_EDITOR
                for (int i = 0; i < UnityEditor.Selection.gameObjects.Length; i++)
                {
                        if(Selection.gameObjects[i] == gameObject)
                        {
                                return true;
                        }
                }
#endif

                return false;
        }

        public static T InstantiatePrefabAsset<T>(string prefabName, string undoName = "instantiate") where T : Object
        {
                T instantiatedPrefab = null;
#if UNITY_EDITOR
                GameObject asset = FindAsset<GameObject>(prefabName);
                GameObject instantiatedGO = UnityEditor.PrefabUtility.InstantiatePrefab(asset) as GameObject;

                if (instantiatedGO)
                {
                        instantiatedPrefab = instantiatedGO.GetComponent<T>();
                        UnityEditor.Undo.RegisterCreatedObjectUndo(instantiatedGO, undoName);
                }
#endif
                return instantiatedPrefab;
        }
            

        public static void MarkSceneDirty(GameObject gameObject)
        {
#if UNITY_EDITOR
                EditorApplication.delayCall += () => EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

        public static void SetGameObjectIcon(GameObject gameObject, string iconName)
        {
#if UNITY_EDITOR
                var texture2D = EditorGUIUtility.GetIconForObject(gameObject);
                string currentName = "";
                if (texture2D)
                {
                        currentName = texture2D.name;
                }

                if (currentName.Equals(iconName) == false)
                {
                        var iconContent = EditorGUIUtility.IconContent(iconName);
                        EditorGUIUtility.SetIconForObject(gameObject, (Texture2D) iconContent.image);     
                }
#endif
        }

        public static Camera GetSceneCamera()
        {
#if UNITY_EDITOR
                if (SceneView.lastActiveSceneView != null)
                        return SceneView.lastActiveSceneView.camera;
#endif

                return null;
        }

    }
}