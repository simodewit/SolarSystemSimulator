using UnityEngine;

/// <summary>
/// A class that ensures there will be only one instance of the specified type of class in the project.
/// </summary>
/// <typeparam name="T"> The type of class </typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    /// <summary>
    /// This getter will either reference the existing instance or create a new one.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            _instance = FindFirstObjectByType<T>();

            if (_instance != null)
            {
                return _instance;
            }
            
            var singletonObject = new GameObject(typeof(T).Name);
            _instance = singletonObject.AddComponent<T>();
            
            return _instance;
        }
    }

    /// <summary>
    /// Gets called before the first rendered frame.
    /// </summary>
    protected virtual void Awake()
    {
        HandleMultipleInstances();
    }

    /// <summary>
    /// This method will make sure there will only be one instance.
    /// </summary>
    private void HandleMultipleInstances()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// A method created to ensure the instance code has been called.
    /// </summary>
    public void SpawnInstance()
    {
        // The instance code can run without this as well but this is a nice illusion it actually spawns in by calling this method.
    }
}