using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RichardPieterse
{
    public class Preference<T>
    {
        private static List<string> __keyRegistry = new List<string>();
        private string _key;
        private bool _initialized = false;
        private T _value;
        private T _default;
        public Action<T> _onChanged;


        public Preference(string key, T defaultValue)
        {
            _key = key;
            _value = defaultValue;
            _default = defaultValue;
            _onChanged = null;

            if (__keyRegistry.Contains(key) == false)
            {
                __keyRegistry.Add(_key);
            }
            else
            {
                Debug.LogError("Duplicate Preference Key " + key);
            }
        }

        private void LazyInitialize()
        {
            if(_initialized)
                return;

            _initialized = true;
#if UNITY_EDITOR
            if (EditorPrefs.HasKey(_key) == false)
            {
                Set(_default, true);
            }
            else
            {
                Set(LoadFromEditorPrefs());
            }
#endif
        }

        public static implicit operator bool(Preference<T> pref)
        {
#if UNITY_EDITOR
            pref.LazyInitialize();
            return EditorPrefs.GetBool(pref._key);
#else
            return false;
#endif
        }

        public static implicit operator string(Preference<T> pref)
        {

#if UNITY_EDITOR
            pref.LazyInitialize();
            return EditorPrefs.GetString(pref._key);
#else
            return "";
#endif
        }

        public static implicit operator int(Preference<T> pref)
        {
#if UNITY_EDITOR
            pref.LazyInitialize();
            return EditorPrefs.GetInt(pref._key);
#else
            return 0;
#endif
        }

        public static implicit operator float(Preference<T> pref)
        {
#if UNITY_EDITOR
            pref.LazyInitialize();
            return EditorPrefs.GetFloat(pref._key);
#else
            return 0;
#endif
        }

        public static implicit operator Object(Preference<T> pref)
        {
#if UNITY_EDITOR
            pref.LazyInitialize();
            var assetPath = EditorPrefs.GetString(pref._key);
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            return asset;
#else
            return null;
#endif
        }

        public T value
        {
            set => Set(value);
            get
            {
                LazyInitialize();
                return _value;
            }
        }

        public Action<T> onChanged
        {
            get => _onChanged;
            set => _onChanged = value;
        }

        private void Set(T value, bool force = false)
        {

            if (force == false && _value != null && _value.Equals(value))
            {
                return;
            }

            _value = value;

#if UNITY_EDITOR
            if (typeof(T) == typeof(string))
                EditorPrefs.SetString(_key, Convert.ToString(value));

            else if (typeof(T) == typeof(float))
            {
                EditorPrefs.SetFloat(_key, Convert.ToSingle(value));

            }
            else if (typeof(T) == typeof(int))
            {
                EditorPrefs.SetInt(_key, Convert.ToInt32(value));
            }

            else if (typeof(T) == typeof(bool))
                EditorPrefs.SetBool(_key, Convert.ToBoolean(value));

            else if (value == null)
                EditorPrefs.SetString(_key, string.Empty);

            else if (value is UnityEngine.Object)
            {
                var assetPath = AssetDatabase.GetAssetPath(value as UnityEngine.Object);

                EditorPrefs.SetString(_key, assetPath);
            }
#endif

            _onChanged?.Invoke(_value);
        }

        private T LoadFromEditorPrefs()
        {
#if UNITY_EDITOR
            LazyInitialize();

            if (typeof(T) == typeof(float))
                return (T) Convert.ChangeType(EditorPrefs.GetFloat(this._key), typeof(T));

            if (typeof(T) == typeof(int))
                return (T) Convert.ChangeType(EditorPrefs.GetInt(this._key), typeof(T));

            if (typeof(T) == typeof(bool))
                return (T) Convert.ChangeType(EditorPrefs.GetBool(this._key), typeof(T));

            if (typeof(T) == typeof(string))
                return (T) Convert.ChangeType(EditorPrefs.GetString(this._key), typeof(T));

            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                string str = EditorPrefs.GetString(this._key);

                Object asset = AssetDatabase.LoadAssetAtPath(str, typeof(T));

                return (T) Convert.ChangeType(asset, typeof(T));
            }

            return (T) Convert.ChangeType((bool) this, typeof(T));
#else
            return default;
#endif
        }

        public void DrawGUI(string label = "")
        {
#if UNITY_EDITOR
            
            if (label == "")
                label = _key;
            
            
            if (this is Preference<bool> boolPref)
            {
                boolPref.value = EditorGUILayout.Toggle(boolPref.value, label);
                return;
            }
            
            if (this is Preference<int> intPref)
            {
                intPref.value = EditorGUILayout.IntField(intPref.value, label);
                return;
            }
            
            
            if (this is Preference<float> floatPref)
            {
                floatPref.value = EditorGUILayout.FloatField(floatPref.value, label);
                return;
            }
            
            
            if (this is Preference<string> stringPref)
            {
                stringPref.value = EditorGUILayout.TextField(stringPref.value, label);
                return;
            }
            
            if (this is Preference<Object> objPref)
            {
                objPref.value = EditorGUILayout.ObjectField(label, objPref.value, typeof(Object), false);
                return;
            }

            throw new NotImplementedException();
            
#endif
        }
    }

    public static class PreferenceExtensions
    {
        public static void Toggle(this Preference<bool> pref)
        {
#if UNITY_EDITOR
            pref.value = !pref.value;
#endif
        }
    }
}
