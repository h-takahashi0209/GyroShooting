
namespace TakahashiH.GyroShooting
{
    /// <summary>
    /// アプリ内パス定義用クラス
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// シーン別
        /// </summary>
        public static class Scenes
        {
            /// <summary>
            /// GameScene
            /// </summary>
            public static class GameScene
            {
                /// <summary>
                /// ゲーム設定
                /// </summary>
                public static string GameSetting = "ScriptableObjects/Scenes/GameScene/GameSetting";

                /// <summary>
                /// プレイヤー設定
                /// </summary>
                public static string PlayerSetting = "ScriptableObjects/Scenes/GameScene/PlayerSetting";

                /// <summary>
                /// 的設定
                /// </summary>
                public static string TargetSetting = "ScriptableObjects/Scenes/GameScene/TargetSetting";

                /// <summary>
                /// 弾設定
                /// </summary>
                public static string BulletSetting = "ScriptableObjects/Scenes/GameScene/BulletSetting";

                /// <summary>
                /// アイテム設定
                /// </summary>
                public static string ItemSetting = "ScriptableObjects/Scenes/GameScene/ItemSetting";

                /// <summary>
                /// ステージデータ
                /// </summary>
                public static string StageData = "Json/Scenes/GameScene/StageData/Stage_";
            }

            /// <summary>
            /// RankingScene
            /// </summary>
            public static class RankingScene
            {
                /// <summary>
                /// ランキングセーブデータ
                /// </summary>
                public static string RankingSaveData = "Json/Scenes/RankingScene/RankingSaveData/RankingSaveData_{0}.json";
            }
        }
    }
}
