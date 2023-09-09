using UnityEngine;

namespace JoyTeam.Core
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }

            Instance = this as T;

            Init();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public virtual void Init() { }
    }
}