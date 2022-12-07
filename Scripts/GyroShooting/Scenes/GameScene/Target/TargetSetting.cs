using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的設定
    /// </summary>
    [CreateAssetMenu(fileName = "TargetSetting", menuName = "ScriptableObjects/Scenes/GameScene/TargetSetting")]
    public sealed class TargetSetting : ScriptableObject
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// 色ごとのデータリスト
        /// </summary>
        [Serializable]
        public sealed class ColorData
        {
            /// <summary>
            /// 色
            /// </summary>
            public TargetColor Color;

            /// <summary>
            /// スコア
            /// </summary>
            public int Score;

            /// <summary>
            /// スケール
            /// </summary>
            public float Scale;

            /// <summary>
            /// 当たり判定半径
            /// </summary>
            public float CollisionRadius;
        }


        //====================================
        //! 変数（private static）
        //====================================

        /// <summary>
        /// インスタンス
        /// </summary>
        private static TargetSetting msInstance;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 色ごとのデータリスト
        /// </summary>
        [SerializeField] private ColorData[] _ColorDataList;


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        public static void Load()
        {
            msInstance = Resources.Load<TargetSetting>(Path.Scenes.GameScene.TargetSetting);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public static void Dispose()
        {
            msInstance = null;
        }

        /// <summary>
        /// スコア取得
        /// </summary>
        /// <param name="color"> 色 </param>
        public static int GetScore(TargetColor color)
        {
            var scoreData = msInstance._ColorDataList.FirstOrDefault(data => data.Color == color);
            if (scoreData == null)
            {
                Debug.LogWarning($"スコアの取得に失敗しました。 Color : {color}");
                return 0;
            }

            return scoreData.Score;
        }

        /// <summary>
        /// スケール取得
        /// </summary>
        /// <param name="color"> 色 </param>
        public static float GetScale(TargetColor color)
        {
            var scoreData = msInstance._ColorDataList.FirstOrDefault(data => data.Color == color);
            if (scoreData == null)
            {
                Debug.LogWarning($"スケールの取得に失敗しました。 Color : {color}");
                return 0;
            }

            return scoreData.Scale;
        }

        /// <summary>
        /// 当たり判定半径取得
        /// </summary>
        /// <param name="color"> 色 </param>
        public static float GetCollisionRadius(TargetColor color)
        {
            var scoreData = msInstance._ColorDataList.FirstOrDefault(data => data.Color == color);
            if (scoreData == null)
            {
                Debug.LogWarning($"当たり判定半径の取得に失敗しました。 Color : {color}");
                return 0;
            }

            return scoreData.CollisionRadius;
        }
    }
}

