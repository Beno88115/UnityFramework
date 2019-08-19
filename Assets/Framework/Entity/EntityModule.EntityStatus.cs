//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    internal sealed partial class EntityModule : GameFrameworkModule, IEntityModule
    {
        /// <summary>
        /// 实体状态。
        /// </summary>
        private enum EntityStatus
        {
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillHide,
            Hidden,
            WillRecycle,
            Recycled,
        }
    }
}
