using System;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 入力ハンドラインターフェース
    /// </summary>
    interface IInputHandler : IDisposable
    {
        /// <summary>
        /// 更新
        /// </summary>
        void DoUpdate();
    }
}

