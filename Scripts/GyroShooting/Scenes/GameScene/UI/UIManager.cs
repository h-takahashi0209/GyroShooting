using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// UI 管理
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIManager : ExMonoBehaviour
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// テロップ UI
        /// </summary>
        [SerializeField] private UITelop UITelop;

        /// <summary>
        /// 残り時間 UI
        /// </summary>
        [SerializeField] private UIRemainingTime UIRemainingTime;

        /// <summary>
        /// スコア UI
        /// </summary>
        [SerializeField] private UIScore UIScore;

        /// <summary>
        /// コマンド UI
        /// </summary>
        [SerializeField] private UICommand UICommand;

        /// <summary>
        /// 弾ステータス UI
        /// </summary>
        [SerializeField] private UIBulletStatus UIBulletStatus;

        /// <summary>
        /// 一時停止ボタン
        /// </summary>
        [SerializeField] private UIButton UIPauseButton;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 累計スコア
        /// </summary>
        public int TotalScore => UIScore.TotalScore;


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            UITelop         = transform.root.GetComponentInChildren<UITelop>(true);
            UIRemainingTime = transform.root.GetComponentInChildren<UIRemainingTime>(true);
            UIScore         = transform.root.GetComponentInChildren<UIScore>(true);
            UICommand       = transform.root.GetComponentInChildren<UICommand>(true);
            UIBulletStatus  = transform.root.GetComponentInChildren<UIBulletStatus>(true);
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="onReqPause">     一時停止リクエスト        </param>
        /// <param name="onReqResume">    再開リクエスト            </param>
        /// <param name="onTimeOut">      時間切れ時コールバック    </param>
        /// <param name="onReqExit">      終了リクエスト            </param>
        public void Setup(Action onReqPause, Action onReqResume, Action onTimeOut, Action onReqExit)
        {
            UIPauseButton.OnClick = () =>
            {
                onReqPause();

                UICommand.SetupPause(onReqResume, onReqExit);
            };

            UIRemainingTime .Setup(GameSetting.DefRemainingTimeSec, onTimeOut);
            UIScore         .Setup();
            UIBulletStatus  .Setup();

            UIRemainingTime .SetActive(false);
            UIScore         .SetActive(false);
            UICommand       .SetActive(false);
            UIBulletStatus  .SetActive(false);
            UIPauseButton   .SetActive(false);
        }

        /// <summary>
        /// カウントダウン開始
        /// </summary>
        /// <param name="onComplete"> 完了時コールバック </param>
        public void BeginCountDown(Action onComplete)
        {
            UITelop.Play(UITelop.TelopType.CountDown, () =>
            {
                SoundManager.PlaySe(SceneSoundDef.GameScene.Se.StartAndEnd.ToString());

                UIRemainingTime .SetActive(true);
                UIScore         .SetActive(true);
                UIBulletStatus  .SetActive(true);
                UIPauseButton   .SetActive(true);

                UIBulletStatus.HideAutoRapidIcon();

                UIRemainingTime.Begin();

                onComplete();
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            UIRemainingTime .DoUpdate();
            UIScore         .DoUpdate();
        }

        /// <summary>
        /// スコア加算
        /// </summary>
        /// <param name="score"> スコア </param>
        public void AddScore(int score)
        {
            UIScore.Add(score);
        }

        /// <summary>
        /// 時間加算
        /// </summary>
        /// <param name="timeSec"> 時間（秒） </param>
        public void AddTime(float timeSec)
        {
            UIRemainingTime.Add(timeSec);
        }

        /// <summary>
        /// 時間切れのテロップ再生
        /// </summary>
        /// <param name="onComplete"> 完了時コールバック </param>
        public void PlayTimeUpTelop(Action onComplete)
        {
            UITelop.Play(UITelop.TelopType.TimeUp, onComplete);
        }

        /// <summary>
        /// オート連射アイコン表示
        /// </summary>
        public void ShowAutoRapidIcon()
        {
            UIBulletStatus.ShowAutoRapidIcon();
        }

        /// <summary>
        /// 連射レベル設定
        /// </summary>
        /// <param name="rapidFireLevel"> 連射レベル </param>
        public void SetRapidFireLevel(int rapidFireLevel)
        {
            UIBulletStatus.SetRapidFireLevel(rapidFireLevel);
        }
    }
}

