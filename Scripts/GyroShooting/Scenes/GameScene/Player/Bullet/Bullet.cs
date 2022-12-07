using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 弾
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Bullet : ExMonoBehaviour, IBulletCollision
    {
        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 移動時間（秒）
        /// </summary>
        public float MoveTimeSec { get; set; }

        /// <summary>
        /// 移動開始座標
        /// </summary>
        public Vector3 BeginPos { get; private set; }

        /// <summary>
        /// 目標座標
        /// </summary>
        public Vector3 TargetPos { get; private set; }

        /// <summary>
        /// ワールド座標
        /// </summary>
        public Vector3 WorldPosition => transform.position;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        public float CollisionRadius { get; private set; }

        /// <summary>
        /// 生存中か
        /// </summary>
        public bool IsAlive { get; private set; }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="id">           ID          </param>
        /// <param name="beginPos">     開始座標    </param>
        /// <param name="targetPos">    目標座標    </param>
        public void Setup(int id, Vector3 beginPos, Vector3 targetPos)
        {
            Id              = id;
            MoveTimeSec     = 0f;
            BeginPos        = beginPos;
            TargetPos       = targetPos;
            CollisionRadius = BulletSetting.CollisionRadius;
            IsAlive         = true;
        }

        /// <summary>
        /// 非活性設定
        /// </summary>
        public void SetDead()
        {
            IsAlive = false;
        }
    }
}

