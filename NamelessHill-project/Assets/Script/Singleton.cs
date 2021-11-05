using UnityEngine;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    protected Singleton()
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

    protected virtual void Init() { }

}
