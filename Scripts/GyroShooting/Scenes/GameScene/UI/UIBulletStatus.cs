using UnityEngine;
using UnityEngine.UI;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 弾のステータス UI
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIBulletStatus : ExMonoBehaviour
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// アニメーション種別
        /// </summary>
        private enum AnimationType
        {
            Show,
            Hide,
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 連射レベルテキスト UI
        /// </summary>
        [SerializeField] private Text UIRapidFireLevelText;

        /// <summary>
        /// オート連射アイコンアニメーション
        /// </summary>
        [SerializeField] private Animation UIAutoRapidIconAnimation;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            SetRapidFireLevel(1);

            UIAutoRapidIconAnimation.Play(AnimationType.Hide.ToString());
        }

        /// <summary>
        /// オート連射アイコン表示
        /// </summary>
        public void ShowAutoRapidIcon()
        {
            UIAutoRapidIconAnimation.Play(AnimationType.Show.ToString());
        }

        /// <summary>
        /// オート連射アイコン非表示
        /// </summary>
        public void HideAutoRapidIcon()
        {
            UIAutoRapidIconAnimation.Play(AnimationType.Hide.ToString());
        }

        /// <summary>
        /// 連射レベル設定
        /// </summary>
        /// <param name="rapidFireLevel"> 連射レベル </param>
        public void SetRapidFireLevel(int rapidFireLevel)
        {
            UIRapidFireLevelText.text = $"RAPID FIRE：{rapidFireLevel}/{Def.Bullet.MaxRapidBulletLevel}";
        }
    }
}

