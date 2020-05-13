using UnityEngine;

namespace Trudograd.NuclearEdition
{
    public static class The<T> where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance => Ensure();

        private static T Ensure()
        {
            if (_instance != null)
                return _instance;

            GameObject gameObject = new GameObject(typeof(T).Name);
            _instance = gameObject.AddComponent<T>();
            GameObject.DontDestroyOnLoad(gameObject);
            return _instance;
        }
    }
}