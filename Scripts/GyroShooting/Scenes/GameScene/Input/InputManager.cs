using System;
using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 入力管理
    /// </summary>
    public sealed class InputManager : IDisposable
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 入力ハンドラリスト
        /// </summary>
        private List<IInputHandler> mInputHandlerList = new List<IInputHandler>();

        /// <summary>
        /// 入力が有効か
        /// </summary>
        private bool mEnableInput;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="onReqGyroRotatePlayer">        ジャイロによるプレイヤー回転リクエスト    </param>
        /// <param name="onReqKeyInputRotatePlayer">    キー入力によるプレイヤー回転リクエスト    </param>
        /// <param name="onReqFire">                    弾発射リクエスト                          </param>
        public void Setup(Action<Quaternion> onReqGyroRotatePlayer, Action<Def.Player.RotateDirection> onReqKeyInputRotatePlayer, Action onReqFire)
        {
            bool isDevice = false;

#if !(UNITY_EDITOR || UNITY_STANDALONE) && (UNITY_ANDROID || UNITY_IOS)
            isDevice = true;
#endif

            // 端末ではジャイロ入力を有効にする
            if (isDevice)
            {
                Input.gyro.enabled = true;

                var gyroInputHandler = new InputHandlerGyro(onReqGyroRotatePlayer);

                mInputHandlerList.Add(gyroInputHandler);
            }

            // PC ではキー入力を有効にする
            else
            {
                var keyInputHandler = new InputHandlerKeyboard(onReqKeyInputRotatePlayer, onReqFire);

                mInputHandlerList.Add(keyInputHandler);
            }

            var screenTapInputHandler = new InputHandlerScreenTap(onReqFire);

            // 共通で画面タップ入力を有効にする
            mInputHandlerList.Add(screenTapInputHandler);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (!mEnableInput) {
                return;
            }

            for (int i = 0; i < mInputHandlerList.Count; i++)
            {
                mInputHandlerList[i].DoUpdate();
            }
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < mInputHandlerList.Count; i++)
            {
                mInputHandlerList[i].Dispose();
            }
        }

        /// <summary>
        /// 入力有効制御
        /// </summary>
        /// <param name="enable"> 入力を有効にするか </param>
        public void SetEnableInput(bool enable)
        {
            mEnableInput = enable;
        }
    }
}

