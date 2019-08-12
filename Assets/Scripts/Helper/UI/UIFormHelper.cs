using GameFramework.UI;

public class UIFormHelper : IUIFormHelper 
{
    /// <summary>
    /// 实例化界面。
    /// </summary>
    /// <param name="uiFormAsset">要实例化的界面资源。</param>
    /// <returns>实例化后的界面。</returns>
    public object InstantiateUIForm(object uiFormAsset)
    {
        return null;
    }

    /// <summary>
    /// 创建界面。
    /// </summary>
    /// <param name="uiFormInstance">界面实例。</param>
    /// <param name="uiGroup">界面所属的界面组。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>界面。</returns>
    public IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
    {
        return null;
    }

    /// <summary>
    /// 释放界面。
    /// </summary>
    /// <param name="uiFormAsset">要释放的界面资源。</param>
    /// <param name="uiFormInstance">要释放的界面实例。</param>
    public void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
    {
    }
}
