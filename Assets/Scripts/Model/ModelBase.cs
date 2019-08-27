using UnityEngine;

public abstract class ModelBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        OnSubscribe();
    }

    protected virtual void OnDestroy()
    {
        OnUnSubscribe();
    }

    /// <summary>
    /// 注册消息事件。
    /// </summary>
    protected abstract void OnSubscribe();

    /// <summary>
    /// 注销消息事件。
    /// </summary>
    protected abstract void OnUnSubscribe();
}