using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的移動パターン：回転移動
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TargetMovePatternRotate : TargetMovePatternBase
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 中心座標
        /// </summary>
        private Vector3 mCenterPos;

        /// <summary>
        /// 移動にかける時間（秒）
        /// </summary>
        private float mMaxMoveTimeSec;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">         的                  </param>
        /// <param name="playerPos">      プレイヤーの座標    </param>
        /// <param name="moveTimeSec">    移動時間（秒）      </param>
        /// <param name="endPos">         目標座標            </param>
        public TargetMovePatternRotate(ITarget target, Vector3 playerPos, float moveTimeSec, Vector3 endPos) : base(target, playerPos)
        {
            mCenterPos      = target.LocalPosition + ((endPos - target.LocalPosition) / 2f);
            mMaxMoveTimeSec = moveTimeSec;
        }


        //====================================
        //! 関数（TargetMovePatternBase）
        //====================================

        /// <summary>
        /// 移動
        /// </summary>
        protected override void DoMove()
        {
            float angle = 360f * TimeManager.DeltaTime / mMaxMoveTimeSec;

            mTarget.RotateAround(mCenterPos, angle);
        }
    }
}

