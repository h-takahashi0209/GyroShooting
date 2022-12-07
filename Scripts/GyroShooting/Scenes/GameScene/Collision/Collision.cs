using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 当たり判定情報インターフェース
    /// </summary>
    public interface ICollision
    {
        /// <summary>
        /// ID
        /// </summary>
        int Id { get; }

        /// <summary>
        /// ワールド座標
        /// </summary>
        Vector3 WorldPosition { get; }

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        float CollisionRadius { get; }

        /// <summary>
        /// 生存中か
        /// </summary>
        bool IsAlive { get; }
    }

    /// <summary>
    /// 弾の当たり判定情報インターフェース
    /// </summary>
    public interface IBulletCollision : ICollision {}

    /// <summary>
    /// 的の当たり判定情報インターフェース
    /// </summary>
    public interface ITargetCollision : ICollision
    {
        /// <summary>
        /// ヒット時の獲得スコア
        /// </summary>
        int Score { get; }
    }

    /// <summary>
    /// アイテムの当たり判定情報インターフェース
    /// </summary>
    public interface IItemCollision : ICollision
    {
        /// <summary>
        /// アイテム種別
        /// </summary>
        Def.Item.ItemType ItemType { get; }
    }
}

