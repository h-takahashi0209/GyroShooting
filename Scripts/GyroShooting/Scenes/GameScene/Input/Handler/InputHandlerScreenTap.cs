using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// スクリーンタップ入力ハンドラ
    /// </summary>
    public sealed class InputHandlerScreenTap : IInputHandler, IDisposable
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 弾発射リクエスト
        /// </summary>
        private Action mOnReqFire;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="onReqFire"> 弾発射リクエスト </param>
        public InputHandlerScreenTap(Action onReqFire)
        {
            mOnReqFire = () => onReqFire();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            CheckFire();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            mOnReqFire = null;
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// 弾発射チェック
        /// </summary>
        private void CheckFire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mOnReqFire();
            }
        }
    }
}

