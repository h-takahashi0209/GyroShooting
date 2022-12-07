using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// ジャイロ入力ハンドラ
    /// </summary>
    public sealed class InputHandlerGyro : IInputHandler, IDisposable
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// プレイヤー回転リクエスト
        /// </summary>
        private Action<Quaternion> mOnReqRotatePlayer;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="onReqRotatePlayer"> プレイヤー回転リクエスト </param>
        public InputHandlerGyro(Action<Quaternion> onReqRotatePlayer)
        {
            mOnReqRotatePlayer = quaternion => onReqRotatePlayer(quaternion);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            mOnReqRotatePlayer(Input.gyro.attitude);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            mOnReqRotatePlayer = null;
        }
    }
}

