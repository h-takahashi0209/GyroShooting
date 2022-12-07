using System;
using System.Collections.Generic;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 当たり判定管理
    /// </summary>
    public sealed class CollisionManager : IDisposable
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 衝突した当たり判定情報リスト
        /// </summary>
        private List<ICollision> mHitCollisionList = new List<ICollision>();


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 弾が衝突したときのコールバック
        /// </summary>
        public Action<int> OnHitBullet { private get; set; }

        /// <summary>
        /// 的が衝突したときのコールバック
        /// </summary>
        public Action<int, int> OnHitTarget { private get; set; }

        /// <summary>
        /// アイテムが衝突したときのコールバック
        /// </summary>
        public Action<int, Def.Item.ItemType> OnHitItem { private get; set; }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 当たり判定を行う
        /// </summary>
        /// <param name="bulletCollisionList">    弾の当たり判定情報リスト      </param>
        /// <param name="targetCollisionList">    的の当たり判定情報リスト      </param>
        /// <param name="itemCollisionList">      アイテムの当たり判定リスト    </param>
        public void Check
        (
            IReadOnlyList<IBulletCollision>     bulletCollisionList     ,
            IReadOnlyList<ITargetCollision>     targetCollisionList     ,
            IReadOnlyList<IItemCollision>       itemCollisionList
        )
        {
            mHitCollisionList.Clear();

            foreach (var bulletCollision in bulletCollisionList)
            {
                if (!bulletCollision.IsAlive) {
                    continue;
                }

                foreach (var targetCollision in targetCollisionList)
                {
                    if (!targetCollision.IsAlive) {
                        continue;
                    }

                    var sqrMagnitude   = (targetCollision.WorldPosition - bulletCollision.WorldPosition).sqrMagnitude;
                    var totalRadiusSqr = (targetCollision.CollisionRadius + bulletCollision.CollisionRadius) * (targetCollision.CollisionRadius + bulletCollision.CollisionRadius);

                    if (sqrMagnitude <= totalRadiusSqr)
                    {
                        mHitCollisionList.Add(bulletCollision);
                        mHitCollisionList.Add(targetCollision);
                    }
                }

                foreach (var itemCollision in itemCollisionList)
                {
                    if (!itemCollision.IsAlive) {
                        continue;
                    }

                    var sqrMagnitude   = (itemCollision.WorldPosition - bulletCollision.WorldPosition).sqrMagnitude;
                    var totalRadiusSqr = (itemCollision.CollisionRadius + bulletCollision.CollisionRadius) * (itemCollision.CollisionRadius + bulletCollision.CollisionRadius);

                    if (sqrMagnitude <= totalRadiusSqr)
                    {
                        mHitCollisionList.Add(bulletCollision);
                        mHitCollisionList.Add(itemCollision);
                    }
                }
            }

            // 衝突したオブジェクトのコールバックを呼ぶ
            foreach (var hitCollision in mHitCollisionList)
            {
                var bulletHitCollision = hitCollision as IBulletCollision;
                var targetHitCollision = hitCollision as ITargetCollision;
                var itemHitCollision   = hitCollision as IItemCollision;

                if (bulletHitCollision != null)
                {
                    OnHitBullet(bulletHitCollision.Id);
                }

                if (targetHitCollision != null)
                {
                    OnHitTarget(targetHitCollision.Id, targetHitCollision.Score);
                }

                if (itemHitCollision != null)
                {
                    OnHitItem(itemHitCollision.Id, itemHitCollision.ItemType);
                }
            }
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            OnHitBullet = null;
            OnHitTarget = null;
            OnHitItem   = null;
        }
    }
}

