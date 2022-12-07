using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的移動パターンインターフェース
    /// </summary>
    public interface ITargetMovePattern
    {
        /// <summary>
        /// 移動
        /// </summary>
        void Move();
    }

    /// <summary>
    /// 的移動パターン基底
    /// </summary>
    public abstract class TargetMovePatternBase : ITargetMovePattern
    {
        //====================================
        //! 変数（protected）
        //====================================

        /// <summary>
        /// 的
        /// </summary>
        protected ITarget mTarget;

        /// <summary>
        /// プレイヤーの座標
        /// </summary>
        protected Vector3 mPlayerPos;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">       的                  </param>
        /// <param name="playerPos">    プレイヤーの座標    </param>
        public TargetMovePatternBase(ITarget target, Vector3 playerPos)
        {
            mTarget     = target;
            mPlayerPos  = playerPos;
        }

        /// <summary>
        /// 移動
        /// </summary>
        public void Move()
        {
            if (!mTarget.IsDisappearing)
            {
                DoMove();
            }
        }


        //====================================
        //! 関数（public virtual）
        //====================================

        /// <summary>
        /// 移動
        /// </summary>
        protected virtual void DoMove() {}
    }
}

