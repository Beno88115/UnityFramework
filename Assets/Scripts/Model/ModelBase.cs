using UnityEngine;

public abstract class ModelBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        OnSubscribe();
    }

    protected virtual void OnDisable()
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