using UnityEngine;
using System.Collections;
using System;

namespace BrianTools.ManagersManager
{

    public class ManagerBase<T> : MonoBehaviour where T : Component
    {

        private static T _sInstance;

        public static T Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    GameObject prefab = ManagersManager.Instance.GetManagerPrefab<T>();
                    GameObject instance = GameObject.Instantiate(prefab);
                    _sInstance = instance.GetComponent<T>();
                    DontDestroyOnLoad(_sInstance.gameObject);
                }
                return _sInstance;
            }
        }

        public virtual void Awake()
        {
            if (_sInstance != null)
            {
                Debug.LogError("Error! Duplicate manager of type " + typeof(T).ToString() + " exists! Destroying new instance;");
                GameObject.Destroy(this);
            }
            else
            {
                _sInstance = (T)Convert.ChangeType(this, typeof(T));
				DontDestroyOnLoad(_sInstance.gameObject);
			}
        }

        void OnDestroy()
        {
            _sInstance = (T)Convert.ChangeType(null, typeof(T)); ;
        }
    }

}