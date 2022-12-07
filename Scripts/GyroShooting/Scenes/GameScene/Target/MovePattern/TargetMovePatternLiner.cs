using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的移動パターン：直線
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TargetMovePatternLiner : TargetMovePatternBase
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 移動時間（秒）
        /// </summary>
        private float mMoveTimeSec;

        /// <summary>
        /// 最大移動時間（秒）
        /// </summary>
        private float mMaxMoveTimeSec;

        /// <summary>
        /// 開始座標
        /// </summary>
        private Vector3 mBeginPos;

        /// <summary>
        /// 移動ベクトル
        /// </summary>
        private Vector3 mMoveVec;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">         的の座標            </param>
        /// <param name="playerPos">      プレイヤーの座標    </param>
        /// <param name="moveTimeSec">    移動時間（秒）      </param>
        /// <param name="endPos">         目標座標            </param>
        public TargetMovePatternLiner(ITarget target, Vector3 playerPos, float moveTimeSec, Vector3 endPos) : base(target, playerPos)
        {
            mMoveTimeSec    = 0f;
            mMaxMoveTimeSec = moveTimeSec;
            mBeginPos       = target.LocalPosition;
            mMoveVec        = endPos - target.LocalPosition;
        }


        //====================================
        //! 関数（TargetMovePatternBase）
        //====================================

        /// <summary>
        /// 移動
        /// </summary>
        protected override void DoMove()
        {
            mMoveTimeSec += TimeManager.DeltaTime;

            var pos = mBeginPos + mMoveVec * (mMoveTimeSec / mMaxMoveTimeSec);

            mTarget.SetLocalPosition(pos);

            // 一定時間経過したら移動方向を反転
            if (mMoveTimeSec >= mMaxMoveTimeSec)
            {
                mTarget.SetLocalPosition(mBeginPos + mMoveVec);

                mBeginPos    = mTarget.LocalPosition;
                mMoveTimeSec = 0f;

                mMoveVec *= -1f;
            }
        }
    }
}

