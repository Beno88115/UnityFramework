//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    internal sealed partial class ConfigModule : GameFrameworkModule, IConfigModule
    {
        private sealed class LoadConfigTableInfo : IReference
        {
            private LoadType m_LoadType;
            private object m_UserData;

            public LoadConfigTableInfo()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
            }

            public LoadType LoadType
            {
                get
                {
                    return m_LoadType;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static LoadConfigTableInfo Create(LoadType loadType, object userData)
            {
                LoadConfigTableInfo loadConfigTableInfo = ReferencePool.Acquire<LoadConfigTableInfo>();
                loadConfigTableInfo.m_LoadType = loadType;
                loadConfigTableInfo.m_UserData = userData;
                return loadConfigTableInfo;
            }

            public void Clear()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
            }
        }
    }
}
