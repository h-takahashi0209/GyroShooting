using System;
using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// アイテム制御
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ItemController : ExMonoBehaviour
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// 生成時間データ
        /// </summary>
        private class GenerateTimeData
        {
            /// <summary>
            /// アイテム種別
            /// </summary>
            public Def.Item.ItemType ItemType { get; set; }

            /// <summary>
            /// 生成時間（秒）
            /// </summary>
            public float GenerateTimeSec { get; set; }
        }

        /// <summary>
        /// マテリアルデータ
        /// </summary>
        [Serializable]
        private sealed class MaterialData
        {
            /// <summary>
            /// アイテム種別
            /// </summary>
            public Def.Item.ItemType ItemType;

            /// <summary>
            /// マテリアル
            /// </summary>
            public Material Material;
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// アイテムリスト
        /// </summary>
        [SerializeField] private Item[] ItemList;

        /// <summary>
        /// マテリアルデータリスト
        /// </summary>
        [SerializeField] private MaterialData[] MaterialDataList;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// アイテムプール
        /// </summary>
        private PoolSystem<Item> mItemPool = new PoolSystem<Item>();

        /// <summary>
        /// 生成時間データリスト
        /// </summary>
        private GenerateTimeData[] mGenerateTimeDataList = new GenerateTimeData[(int)Def.Item.ItemType.Sizeof];

        /// <summary>
        /// アイテムの ID
        /// </summary>
        private int mItemId;

        /// <summary>
        /// 開始済みか
        /// </summary>
        private bool mIsBegin;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 生成したアイテムの当たり判定リスト
        /// </summary>
        public IReadOnlyList<IItemCollision> CollisionList => mItemPool.UsedElemList;


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            ItemList = transform.root.GetComponentsInChildren<Item>(true);
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            for (int i = 0; i < (int)Def.Item.ItemType.Sizeof; i++)
            {
                mGenerateTimeDataList[i] = new GenerateTimeData
                {
                    ItemType        = (Def.Item.ItemType)i,
                    GenerateTimeSec = ItemSetting.GetGenerateIntervalTimeSec((Def.Item.ItemType)i)
                };
            }

            mItemPool.Setup(ItemList);
        }

        /// <summary>
        /// 開始
        /// </summary>
        public void Begin()
        {
            mIsBegin = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (!mIsBegin) {
                return;
            }

            for (int i = 0; i < (int)Def.Item.ItemType.Sizeof; i++)
            {
                var timeData = mGenerateTimeDataList[i];

                timeData.GenerateTimeSec -= TimeManager.DeltaTime;

                if (timeData.GenerateTimeSec <= 0f)
                {
                    Generate((Def.Item.ItemType)i);

                    timeData.GenerateTimeSec = ItemSetting.GetGenerateIntervalTimeSec((Def.Item.ItemType)i);
                }
            }

            for (int i = 0; i < mItemPool.UsedElemList.Count; i++)
            {
                var item = mItemPool.UsedElemList[i];

                item.DoUpdate();
            }
        }

        /// <summary>
        /// 弾の衝突によりアイテムを消滅させる
        /// </summary>
        /// <param name="id"> アイテムの ID </param>
        public void DisappearByHit(int id)
        {
            foreach (var bullet in mItemPool.UsedElemList)
            {
                if (bullet.Id == id)
                {
                    bullet.SetDead();

                    bullet.PlayAnimation(Item.AnimationType.Hit, () =>
                    {
                        mItemPool.Enqueue(bullet);
                    });

                    break;
                }
            }
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// アイテム生成
        /// </summary>
        /// <param name="itemType"> アイテム種別 </param>
        private void Generate(Def.Item.ItemType itemType)
        {
            var item = mItemPool.Dequeue();
            if (item == null)
            {
                Debug.LogWarning("アイテムの生成に失敗しました。");
                return;
            }

            var position         = new Vector3(UnityEngine.Random.Range(-ItemSetting.GenerateWidth / 2f, ItemSetting.GenerateWidth / 2f), UnityEngine.Random.Range(-ItemSetting.GenerateHeight / 2f, ItemSetting.GenerateHeight / 2f), 0f);
            var material         = GetMaterial(itemType);
            var disappearTimeSec = ItemSetting.GetDisappearTimeSec(itemType);

            item.Setup(mItemId, itemType, position, material, disappearTimeSec, () => DisappearByTimeOut(item.Id));

            mItemId++;
        }

        /// <summary>
        /// マテリアル取得
        /// </summary>
        /// <param name="color"> 色 </param>
        private Material GetMaterial(Def.Item.ItemType itemType)
        {
            var materialData = MaterialDataList.FirstOrDefault(data => data.ItemType == itemType);
            if (materialData == null)
            {
                Debug.LogWarning($"マテリアルデータの取得に失敗しました。 ItemType : {itemType}");
                return null;
            }

            return materialData.Material;
        }

        /// <summary>
        /// 時間切れによりアイテムを消滅させる
        /// </summary>
        /// <param name="id"> アイテムの ID </param>
        private void DisappearByTimeOut(int id)
        {
            foreach (var bullet in mItemPool.UsedElemList)
            {
                if (bullet.Id == id)
                {
                    bullet.SetDead();

                    bullet.PlayAnimation(Item.AnimationType.Disappear, () =>
                    {
                        mItemPool.Enqueue(bullet);
                    });

                    break;
                }
            }
        }
    }
}

