using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.TitleScene
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TitleScene : SceneBase
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// ランキングセーブデータ IO
        /// </summary>
        private RankingSaveDataIO mRankingSaveDataIO = new RankingSaveDataIO();


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// 開始ボタン
        /// </summary>
        [SerializeField] private UIButton UIStartButton;

        /// <summary>
        /// ランキングボタン
        /// </summary>
        [SerializeField] private UIButton UIRankingButton;

        /// <summary>
        /// ランキング削除確認 UI
        /// </summary>
        [SerializeField] private GameObject UIConfirmDeleteRankingObj;

        /// <summary>
        /// ランキング削除ボタン
        /// </summary>
        [SerializeField] private UIButton UIResetRankingButton;

        /// <summary>
        /// ランキング削除キャンセルボタン
        /// </summary>
        [SerializeField] private UIButton UICancelResetRankingButton;


        //====================================
        //! 関数（SceneBase）
        //====================================

        /// <summary>
        /// DoStart
        /// </summary>
        protected override void DoStart()
        {
            mRankingSaveDataIO.LoadAll();

            UIStartButton               .OnClick = () => StartGame();
            UIRankingButton             .OnClick = () => OpenRanking();
            UIResetRankingButton        .OnClick = () => ResetRanking();
            UICancelResetRankingButton  .OnClick = () => UIConfirmDeleteRankingObj.SetActive(false);

            UIConfirmDeleteRankingObj.SetActive(false);

            UIFade.FadeIn(Color.black, CommonDef.FadeTimeSec);
        }

        /// <summary>
        /// DoUpdate
        /// </summary>
        protected override void DoUpdate()
        {
            if (IsOpenConfirmResetRankingUI())
            {
                UIConfirmDeleteRankingObj.SetActive(true);
            }
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// ゲーム開始
        /// </summary>
        private void StartGame()
        {
            UIFade.FadeOut(Color.black, CommonDef.FadeTimeSec, () =>
            {
                SceneManager.Load(SceneType.LevelSelectScene);
            });
        }

        /// <summary>
        /// ランキングを開く
        /// </summary>
        private void OpenRanking()
        {
            var inputData = new RankingScene.RankingScene.InputData()
            {
                RankingScoreList = new Dictionary<Level, IReadOnlyList<int>>()
                {
                    {Level.Easy   , mRankingSaveDataIO.GetScoreList(Level.Easy  )},
                    {Level.Normal , mRankingSaveDataIO.GetScoreList(Level.Normal)},
                    {Level.Hard   , mRankingSaveDataIO.GetScoreList(Level.Hard  )},
                }
            };

            SceneManager.LoadAdditive(SceneType.RankingScene, inputData);
        }

        /// <summary>
        /// ランキングをリセット
        /// </summary>
        private void ResetRanking()
        {
            mRankingSaveDataIO.ResetRanking();

            UIConfirmDeleteRankingObj.SetActive(false);
        }

        /// <summary>
        /// ランキングリセット確認 UI を開くか
        /// </summary>
        private bool IsOpenConfirmResetRankingUI()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

            if (Input.GetMouseButtonDown(2))
            {
                return true;
            }

#elif UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount == 3)
            {
                return true;
            }

#endif

            return false;
        }
    }
}

