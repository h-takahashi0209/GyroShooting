using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// キーボード入力ハンドラ
    /// </summary>
    public sealed class InputHandlerKeyboard : IInputHandler, IDisposable
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// プレイヤー回転リクエスト
        /// </summary>
        private Action<Def.Player.RotateDirection> mOnReqRotatePlayer;

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
        /// <param name="onReqRotatePlayer">    プレイヤー回転リクエスト    </param>
        /// <param name="onReqFire">            弾発射リクエスト            </param>
        public InputHandlerKeyboard(Action<Def.Player.RotateDirection> onReqRotatePlayer, Action onReqFire)
        {
            mOnReqRotatePlayer  = direction => onReqRotatePlayer(direction);
            mOnReqFire          = ()        => onReqFire();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            CheckRotatePlayer();
            CheckFire();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            mOnReqRotatePlayer = null;
            mOnReqFire         = null;
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// プレイヤー回転チェック
        /// </summary>
        private void CheckRotatePlayer()
        {
            if (TakahashiH.InputManager.IsKeyPress(KeyCode.LeftArrow))
            {
                mOnReqRotatePlayer(Def.Player.RotateDirection.Left);
            }

            if (TakahashiH.InputManager.IsKeyPress(KeyCode.RightArrow))
            {
                mOnReqRotatePlayer(Def.Player.RotateDirection.Right);
            }

            if (TakahashiH.InputManager.IsKeyPress(KeyCode.UpArrow))
            {
                mOnReqRotatePlayer(Def.Player.RotateDirection.Up);
            }

            if (TakahashiH.InputManager.IsKeyPress(KeyCode.DownArrow))
            {
                mOnReqRotatePlayer(Def.Player.RotateDirection.Down);
            }
        }

        /// <summary>
        /// 弾発射チェック
        /// </summary>
        private void CheckFire()
        {
            if (TakahashiH.InputManager.IsKeyDown(KeyCode.Space))
            {
                mOnReqFire();
            }
        }
    }
}

