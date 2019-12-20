using System;
using GameFramework;

public partial class ResourceManager : SingletonMono<ResourceManager>
{
    public delegate void InitResourcesCompleteCallback();
    public delegate void LoadAssetCompleteCallback(string assetName, object assetObject);
    public delegate void LoadAssetFailureCallback(string assetName, string errMessage);

    private class LoadResCallbacks
    {
        private readonly LoadAssetCompleteCallback m_LoadAssetCompleteCallback;
        private readonly LoadAssetFailureCallback m_LoadAssetFailureCallback;

        /// <summary>
        /// 初始化加载资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调函数。</param>
        public LoadResCallbacks(LoadAssetCompleteCallback loadAssetSuccessCallback)
            : this(loadAssetSuccessCallback, null)
        {
        }

        /// <summary>
        /// 初始化加载资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadAssetCompleteCallback">加载资源成功回调函数。</param>
        /// <param name="loadAssetFailureCallback">加载资源失败回调函数。</param>
        public LoadResCallbacks(LoadAssetCompleteCallback loadAssetCompleteCallback, LoadAssetFailureCallback loadAssetFailureCallback)
        {
            if (loadAssetCompleteCallback == null)
            {
                throw new GameFrameworkException("Load asset success callback is invalid.");
            }

            m_LoadAssetCompleteCallback = loadAssetCompleteCallback;
            m_LoadAssetFailureCallback = loadAssetFailureCallback;
        }

        /// <summary>
        /// 获取加载资源成功回调函数。
        /// </summary>
        public LoadAssetCompleteCallback LoadAssetCompleteCallback
        {
            get
            {
                return m_LoadAssetCompleteCallback;
            }
        }

        /// <summary>
        /// 获取加载资源失败回调函数。
        /// </summary>
        public LoadAssetFailureCallback LoadAssetFailureCallback
        {
            get
            {
                return m_LoadAssetFailureCallback;
            }
        }
    }
}