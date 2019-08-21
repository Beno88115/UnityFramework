using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;

public partial class ResourceManager : SingletonMono<ResourceManager>
{
    /// <summary>
    /// 资源对象。
    /// </summary>
    private sealed class ResObject : ObjectBase
    {
        private readonly object m_AssetObject;
        private readonly IResourceHelper m_ResHelper;

        public ResObject(string name, object assetObject, IResourceHelper resHelper)
            : base(name, assetObject)
        {
            if (assetObject == null) {
                throw new GameFrameworkException("UI asset is invalid.");
            }
            if (resHelper == null) {
                throw new GameFrameworkException("UI resource helper is invalid.");
            }
            m_ResHelper = resHelper;
        }

        protected internal override void Release(bool isShutdown)
        {
            m_ResHelper.Release(Target);
        }
    }
}
