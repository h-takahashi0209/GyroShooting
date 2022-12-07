using UnityEngine;
using UnityEngine.UI;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// スコア UI
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIScore : ExMonoBehaviour
    {
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
        /// 開始スコア
        /// </summary>
        private int mBeginScore;

        /// <summary>
        /// 目標スコア
        /// </summary>
        private int mEndScore;

        /// <summary>
        /// 現在のスコア
        /// </summary>
        private int mNowScore;

        /// <summary>
        /// カウントアップ時間（秒）
        /// </summary>
        private float mCountUpTimeSec;

        /// <summary>
        /// カウントアップ中か
        /// </summary>
        private bool mIsRunning;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 累計スコア
        /// </summary>
        public int TotalScore => mEndScore;


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


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            SetScore(0);
        }

        /// <summary>
        /// スコア加算
        /// </summary>
        /// <param name="score"> スコア </param>
        public void Add(int score)
        {
            mBeginScore     = mNowScore;
            mEndScore       = mEndScore + score;
            mCountUpTimeSec = 0f;
            mIsRunning      = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (!mIsRunning) {
                return;
            }

            mCountUpTimeSec += TimeManager.DeltaTime;

            var rate = mCountUpTimeSec / CountUpTimeSec;

            mNowScore = mBeginScore + (int)(((float)mEndScore - mBeginScore) * rate);

            SetScore(mNowScore);

            if (mCountUpTimeSec >= CountUpTimeSec)
            {
                SetScore(mEndScore);

                mNowScore  = mEndScore;
                mIsRunning = false;
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
            UIScoreText.text = string.Format("SCORE:{0, 8}", score);
        }
    }
}

