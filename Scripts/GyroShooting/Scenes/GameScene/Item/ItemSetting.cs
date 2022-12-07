using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// アイテム設定
    /// </summary>
    [CreateAssetMenu(fileName = "ItemSetting", menuName = "ScriptableObjects/Scenes/GameScene/ItemSetting")]
    public sealed class ItemSetting : ScriptableObject
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// アイテムごとのデータ
        /// </summary>
        [Serializable]
        public sealed class ItemData
        {
            /// <summary>
            /// 色
            /// </summary>
            public Def.Item.ItemType ItemType;

            /// <summary>
            /// 生成間隔時間（秒）
            /// </summary>
            public float GenerateIntervalTimeSec;

            /// <summary>
            /// 消滅時間（秒）
            /// </summary>
            public float DisappearTimeSec;
        }


        //====================================
        //! 変数（private static）
        //====================================

        /// <summary>
        /// インスタンス
        /// </summary>
        private static ItemSetting msInstance;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 時間加算量（秒）
        /// </summary>
        [SerializeField] private float _AddedTimeSec;

        /// <summary>
        /// 連射レベルアップ量
        /// </summary>
        [SerializeField] private int _AddedRapidFireLevel;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        [SerializeField] private float _CollisionRadius;

        /// <summary>
        /// 横生成範囲
        /// </summary>
        [SerializeField] private float _GenerateWidth;

        /// <summary>
        /// 縦生成範囲
        /// </summary>
        [SerializeField] private float _GenerateHeight;

        /// <summary>
        /// アイテムごとのデータリスト
        /// </summary>
        [SerializeField] private ItemData[] _ItemDataList;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 時間加算量（秒）
        /// </summary>
        public static float AddedTimeSec => msInstance._AddedTimeSec;

        /// <summary>
        /// 連射レベルアップ量
        /// </summary>
        public static int AddedRapidFireLevel => msInstance._AddedRapidFireLevel;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        public static float CollisionRadius => msInstance._CollisionRadius;

        /// <summary>
        /// 横生成範囲
        /// </summary>
        public static float GenerateWidth => msInstance._GenerateWidth;

        /// <summary>
        /// 縦生成範囲
        /// </summary>
        public static float GenerateHeight => msInstance._GenerateHeight;


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        public static void Load()
        {
            msInstance = Resources.Load<ItemSetting>(Path.Scenes.GameScene.ItemSetting);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public static void Dispose()
        {
            msInstance = null;
        }

        /// <summary>
        /// 生成間隔時間（秒）取得
        /// </summary>
        /// <param name="itemType"> アイテム種別 </param>
        public static float GetGenerateIntervalTimeSec(Def.Item.ItemType itemType)
        {
            var itemData = msInstance._ItemDataList.FirstOrDefault(data => data.ItemType == itemType);
            if (itemData == null)
            {
                Debug.LogWarning($"生成間隔時間の取得に失敗しました。 ItemType : {itemType}");
                return 0f;
            }

            return itemData.GenerateIntervalTimeSec;
        }

        /// <summary>
        /// 消滅時間（秒）取得
        /// </summary>
        /// <param name="itemType"> アイテム種別 </param>
        public static float GetDisappearTimeSec(Def.Item.ItemType itemType)
        {
            var itemData = msInstance._ItemDataList.FirstOrDefault(data => data.ItemType == itemType);
            if (itemData == null)
            {
                Debug.LogWarning($"消滅時間の取得に失敗しました。 ItemType : {itemType}");
                return 0f;
            }

            return itemData.DisappearTimeSec;
        }
    }
}

