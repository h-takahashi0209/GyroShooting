using System;
using UnityEngine;
using UnityEngine.UI;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 残り時間 UI
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIRemainingTime : ExMonoBehaviour
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 残り時間テキスト UI
        /// </summary>
        [SerializeField] private Text UIRemainingTimeText;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 残り時間（秒）
        /// </summary>
        private float mRemainingTimeSec;

        /// <summary>
        /// 計測中か
        /// </summary>
        private bool mIsRunning;

        /// <summary>
        /// 時間切れ時コールバック
        /// </summary>
        private Action mOnTimeOut;


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            mOnTimeOut = null;
        }

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            UIRemainingTimeText = GetComponent<Text>();
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="timeSec">      時間（秒）                </param>
        /// <param name="onTimeOut">    時間切れ時コールバック    </param>
        public void Setup(float timeSec, Action onTimeOut)
        {
            mRemainingTimeSec = timeSec;
            mOnTimeOut        = onTimeOut;

            SetRemainingTime(mRemainingTimeSec);
        }

        /// <summary>
        /// 計測開始
        /// </summary>
        public void Begin()
        {
            mIsRunning = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (!mIsRunning) {
                return;
            }

            mRemainingTimeSec -= TimeManager.DeltaTime;

            SetRemainingTime(mRemainingTimeSec + 1f);

            if (mRemainingTimeSec <= 0f)
            {
                mIsRunning = false;

                SetRemainingTime(0f);
                mOnTimeOut();
            }
        }

        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="timeSec"> 時間（秒） </param>
        public void Add(float timeSec)
        {
            mRemainingTimeSec += timeSec;
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// 残り時間設定
        /// </summary>
        /// <param name="timeSec"> 時間（秒）</param>
        private void SetRemainingTime(float timeSec)
        {
            UIRemainingTimeText.text = string.Format("TIME:{0:D2}", (int)timeSec);
        }
    }
}

