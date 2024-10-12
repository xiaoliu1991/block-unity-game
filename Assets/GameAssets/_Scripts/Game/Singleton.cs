﻿public abstract class Singleton<T> where T: Singleton<T>, new()
{
    private static T _instance;
    protected Singleton() { }

    public static T Instance
    {
        get
        {
            if (_instance == null) 
            { 
                _instance = new T(); 
            }
            return _instance;
        }
    }

    public static void Dispose()
    {
        _instance = null;
    }
}
