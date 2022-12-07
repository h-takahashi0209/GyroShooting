using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的移動パターン：移動しない
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TargetMovePatternStay : TargetMovePatternBase
    {
        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">       的の座標            </param>
        /// <param name="playerPos">    プレイヤーの座標    </param>
        public TargetMovePatternStay(ITarget target, Vector3 playerPos) : base(target, playerPos) {}
    }
}

