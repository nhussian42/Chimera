using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
 
        public static T Instance
        {
            get { return _instance; }
        }
 
        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Second instance of " + typeof(T) + " created. Automatic self-destruct triggered. KABOOM!");
                Destroy(this.gameObject);
                return;
            }
            
            _instance = this as T;

            Init();
        }

        protected virtual void Init() { }
    }
