using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// ゲーム設定
    /// </summary>
    [CreateAssetMenu(fileName = "GameSetting", menuName = "ScriptableObjects/Scenes/GameScene/GameSetting")]
    public sealed class GameSetting : ScriptableObject
    {
        //====================================
        //! 変数（private static）
        //====================================

        /// <summary>
        /// インスタンス
        /// </summary>
        private static GameSetting msInstance;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 初期残り時間（秒）
        /// </summary>
        [SerializeField] private float _DefRemainingTimeSec;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 初期残り時間（秒）
        /// </summary>
        public static float DefRemainingTimeSec => msInstance._DefRemainingTimeSec;


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        public static void Load()
        {
            msInstance = Resources.Load<GameSetting>(Path.Scenes.GameScene.GameSetting);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public static void Dispose()
        {
            msInstance = null;
        }
    }
}

