using UnityEngine;

public abstract class SingletonMono<T>: MonoBehaviour where T : SingletonMono<T>, new()
{
    protected SingletonMono()
    {
        Init();
    }

    protected static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    protected virtual void Awake() {
        instance = this as T;
    }

    protected virtual void Init() { }

}
