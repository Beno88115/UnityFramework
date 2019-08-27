using System.Collections.Generic;

public class ModelManager : SingletonMono<ModelManager>
{
    private Dictionary<string, ModelBase> m_Models;

    protected override void Awake()
    {
        base.Awake();
        m_Models = new Dictionary<string, ModelBase>();
    }

    public T Get<T>() where T : ModelBase
    {
        string typeName = typeof(T).Name;
        if (m_Models.ContainsKey(typeName)) 
        {
            return (T)m_Models[typeName];
        }
        T model = this.gameObject.AddComponent<T>();
        m_Models[typeName] = model;
        return model;
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}