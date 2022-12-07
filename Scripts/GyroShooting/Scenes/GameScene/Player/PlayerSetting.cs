using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// プレイヤー設定
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerSetting", menuName = "ScriptableObjects/Scenes/GameScene/PlayerSetting")]
    public sealed class PlayerSetting : ScriptableObject
    {
        //====================================
        //! 変数（private static）
        //====================================

        /// <summary>
        /// インスタンス
        /// </summary>
        private static PlayerSetting msInstance;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// キー入力による最大回転速度
        /// </summary>
        [SerializeField] private float _KeyInputMaxRotateSpeed;

        /// <summary>
        /// X軸最大回転角度
        /// </summary>
        [SerializeField] private float _MaxAngleX;

        /// <summary>
        /// Y軸最大回転角度
        /// </summary>
        [SerializeField] private float _MaxAngleY;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// キー入力による最大回転速度
        /// </summary>
        public static float KeyInputMaxRotateSpeed => msInstance._KeyInputMaxRotateSpeed;

        /// <summary>
        /// X軸最大回転角度
        /// </summary>
        public static float MaxAngleX => msInstance._MaxAngleX;

        /// <summary>
        /// Y軸最大回転角度
        /// </summary>
        public static float MaxAngleY => msInstance._MaxAngleY;


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        public static void Load()
        {
            msInstance = Resources.Load<PlayerSetting>(Path.Scenes.GameScene.PlayerSetting);
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

