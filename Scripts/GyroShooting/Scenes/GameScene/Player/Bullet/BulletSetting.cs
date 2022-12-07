using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 弾設定
    /// </summary>
    [CreateAssetMenu(fileName = "BulletSetting", menuName = "ScriptableObjects/Scenes/GameScene/BulletSetting")]
    public sealed class BulletSetting : ScriptableObject
    {
        //====================================
        //! 変数（private static）
        //====================================

        /// <summary>
        /// インスタンス
        /// </summary>
        private static BulletSetting msInstance;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 的への到達にかける時間（秒）
        /// </summary>
        [SerializeField] private float _ReachTimeSec;

        /// <summary>
        /// レベルごとの連射インターバル時間リスト（秒）
        /// </summary>
        [SerializeField] private float[] _RapidFireIntervalTimeSecList;

        /// <summary>
        /// 発射座標オフセット
        /// </summary>
        [SerializeField] private Vector3 _FirePosOffset;

        /// <summary>
        /// 目標Z座標
        /// </summary>
        [SerializeField] private float _TargetPosZ;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        [SerializeField] private float _CollisionRadius;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 的への到達にかける時間（秒）
        /// </summary>
        public static float ReachTimeSec => msInstance._ReachTimeSec;

        /// <summary>
        /// 発射座標オフセット
        /// </summary>
        public static Vector3 FirePosOffset => msInstance._FirePosOffset;

        /// <summary>
        /// 目標Z座標
        /// </summary>
        public static float TargetPosZ => msInstance._TargetPosZ;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        public static float CollisionRadius => msInstance._CollisionRadius;


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        public static void Load()
        {
            msInstance = Resources.Load<BulletSetting>(Path.Scenes.GameScene.BulletSetting);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public static void Dispose()
        {
            msInstance = null;
        }

        /// <summary>
        /// 連射インターバル時間（秒）取得
        /// </summary>
        /// <param name="level"> 連射レベル </param>
        public static float GetRapidFireIntervalTimeSec(int level)
        {
            int idx = level - 1;

            if (idx < 0 || idx >= msInstance._RapidFireIntervalTimeSecList.Length)
            {
                Debug.LogWarning($"連射インターバル時間の取得に失敗しました。 Level : {level}");
                return 1f;
            }

            return msInstance._RapidFireIntervalTimeSecList[idx];
        }
    }
}

