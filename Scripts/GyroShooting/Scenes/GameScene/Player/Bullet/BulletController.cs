using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 弾制御
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BulletController : ExMonoBehaviour
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 弾リスト
        /// </summary>
        [SerializeField] private Bullet[] BulletList;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 弾プール
        /// </summary>
        private PoolSystem<Bullet> mBulletPool = new PoolSystem<Bullet>();

        /// <summary>
        /// 次の弾発射までのインターバル時間（秒）
        /// </summary>
        private float mFireIntervalTimeSec;

        /// <summary>
        /// 次の弾発射までの最大インターバル時間（秒）
        /// </summary>
        private float mMaxFireIntervalTimeSec;

        /// <summary>
        /// 弾の ID
        /// </summary>
        private int mBulletId;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 生成した弾の当たり判定リスト
        /// </summary>
        public IReadOnlyList<IBulletCollision> CollisionList => mBulletPool.UsedElemList;

        /// <summary>
        /// 連射レベル
        /// </summary>
        public int RapidFireLevel { get; private set; }

        /// <summary>
        /// オート連射中か
        /// </summary>
        public bool IsAutoRapidFire { get; set; }


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            BulletList = transform.root.GetComponentsInChildren<Bullet>(true);
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            RapidFireLevel = 0;

            AddRapidFireLevel(1);

            mBulletPool.Setup(BulletList);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (mFireIntervalTimeSec > 0f)
            {
                mFireIntervalTimeSec -= TimeManager.DeltaTime;
            }

            Move();
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        /// <param name="beginPos">     開始座標    </param>
        /// <param name="targetPos">    目標座標    </param>
        public void Fire(Vector3 beginPos, Vector3 targetPos)
        {
            // インターバル時間中は撃てない
            if (mFireIntervalTimeSec > 0f) {
                return;
            }

            _Fire(beginPos, targetPos);
        }

        /// <summary>
        /// 連射レベル加算
        /// </summary>
        /// <param name="addedLevel"> 加算するレベル </param>
        public void AddRapidFireLevel(int addedLevel)
        {
            RapidFireLevel          = Mathf.Min(RapidFireLevel + addedLevel, Def.Bullet.MaxRapidBulletLevel);
            mFireIntervalTimeSec    = 0f;
            mMaxFireIntervalTimeSec = BulletSetting.GetRapidFireIntervalTimeSec(RapidFireLevel);
        }

        /// <summary>
        /// 弾を消滅させる
        /// </summary>
        /// <param name="id"> 弾の ID </param>
        public void Disappear(int id)
        {
            foreach (var bullet in mBulletPool.UsedElemList)
            {
                if (bullet.Id == id)
                {
                    mBulletPool.Enqueue(bullet);
                    break;
                }
            }
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        /// <param name="beginPos">     開始座標    </param>
        /// <param name="targetPos">    目標座標    </param>
        private void _Fire(Vector3 beginPos, Vector3 targetPos)
        {
            var bullet = mBulletPool.Dequeue();
            if (bullet == null)
            {
                Debug.LogWarning("弾の生成に失敗しました。");
                return;
            }

            SoundManager.PlaySe(SceneSoundDef.GameScene.Se.Shot.ToString());

            bullet.Setup(mBulletId, beginPos, targetPos);

            mFireIntervalTimeSec = mMaxFireIntervalTimeSec;

            mBulletId++;
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            for (int i = 0; i < mBulletPool.UsedElemList.Count; i++)
            {
                var bullet = mBulletPool.UsedElemList[i];

                bullet.MoveTimeSec += TimeManager.DeltaTime;

                var rate = bullet.MoveTimeSec / BulletSetting.ReachTimeSec;
                var pos  = bullet.BeginPos + bullet.TargetPos * rate;

                bullet.SetLocalPosition(pos);

                // 的の座標に到達したら消す
                if (bullet.MoveTimeSec >= BulletSetting.ReachTimeSec)
                {
                    mBulletPool.Enqueue(bullet);
                }
            }
        }
    }
}

