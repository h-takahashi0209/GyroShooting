using System;
using UnityEngine;
using UnityEngine.UI;


namespace TakahashiH.GyroShooting.Scenes.ResultScene
{
    /// <summary>
    /// スコア加算アニメーション UI
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIScoreAnimation : ExMonoBehaviour
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// カウントアップ SE 再生時間（秒）
        /// </summary>
        private const float PlayCountUpSeTimeSec = 0.1f;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// スコアテキスト UI
        /// </summary>
        [SerializeField] private Text UIScoreText;

        /// <summary>
        /// カウントアップにかける時間（秒）
        /// </summary>
        [SerializeField] private float CountUpTimeSec = 0.5f;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 目標スコア
        /// </summary>
        private int mTargetScore;

        /// <summary>
        /// カウントアップ時間（秒）
        /// </summary>
        private float mCountUpTimeSec;

        /// <summary>
        /// カウントアップ再生時間（秒）
        /// </summary>
        private float mPlayCountUpSeTimeSec;

        /// <summary>
        /// 完了時コールバック
        /// </summary>
        private Action mOnComplete;


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            UIScoreText    = GetComponent<Text>();
            CountUpTimeSec = 0.5f;
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            mOnComplete = null;
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 再生
        /// </summary>
        /// <param name="score">         スコア                </param>
        /// <param name="onComplete">    完了時コールバック    </param>
        public void Play(int score, Action onComplete)
        {
            if (score <= 0)
            {
                SetScore(0);
                onComplete();
                return;
            }

            mTargetScore            = score;
            mOnComplete             = onComplete;
            mCountUpTimeSec         = 0f;
            mPlayCountUpSeTimeSec   = 0f;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (mTargetScore <= 0) {
                return;
            }

            mCountUpTimeSec += TimeManager.DeltaTime;

            var rate  = mCountUpTimeSec / CountUpTimeSec;
            int score = (int)(mTargetScore * rate);

            SetScore(score);

            mPlayCountUpSeTimeSec += TimeManager.DeltaTime;

            if (mPlayCountUpSeTimeSec >= PlayCountUpSeTimeSec)
            {
                SoundManager.PlaySe(SceneSoundDef.ResultScene.Se.CountUp.ToString());

                mPlayCountUpSeTimeSec = 0f;
            }

            if (mCountUpTimeSec >= CountUpTimeSec)
            {
                SetScore(mTargetScore);

                mOnComplete();

                mTargetScore = 0;
            }
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// スコア設定
        /// </summary>
        /// <param name="score"> スコア </param>
        private void SetScore(int score)
        {
            UIScoreText.text = string.Format("SCORE：{0, 8}", score);
        }
    }
}

