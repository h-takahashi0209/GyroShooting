using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.ResultScene
{
    /// <summary>
    /// リザルトシーン
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ResultScene : SceneBase
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// 他シーンから渡されるデータ
        /// </summary>
        public class InputData
        {
            /// <summary>
            /// スコア
            /// </summary>
            public int Score { get; set; }

            /// <summary>
            /// レベル
            /// </summary>
            public Level Level { get; set; }

            /// <summary>
            /// ランキングのスコアリスト
            /// </summary>
            public IReadOnlyList<int> RankingScoreList { get; set; }

            /// <summary>
            /// ランクインしたか
            /// </summary>
            public bool IsRankIn { get; set; }
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// スコアアニメーション UI
        /// </summary>
        [SerializeField] private UIScoreAnimation UIScoreAnimation;

        /// <summary>
        /// 新記録 UI
        /// </summary>
        [SerializeField] private GameObject UINewRecordObj;

        /// <summary>
        /// ランキングボタン
        /// </summary>
        [SerializeField] private UIButton UIRankingButton;

        /// <summary>
        /// リトライボタン
        /// </summary>
        [SerializeField] private UIButton UIRetryButton;

        /// <summary>
        /// 終了ボタン
        /// </summary>
        [SerializeField] private UIButton UIExitButton;

        /// <summary>
        /// 演出再生間隔時間（秒）
        /// </summary>
        [SerializeField] private float WaitTimeSec;


        //====================================
        //! 関数（SceneBase）
        //====================================

        /// <summary>
        /// DoStart
        /// </summary>
        protected override void DoStart()
        {
            var inputData = SceneManager.SceneInputData as InputData;
            if (inputData == null)
            {
                Debug.LogWarning("リザルトシーンの入力データ取得に失敗しました。");

                inputData = new InputData()
                {
                    Score            = 1000,
                    Level            = Level.Easy,
                    RankingScoreList = new int[(int)CommonDef.SavedRankingScoreNum],
                    IsRankIn         = false
                };
            }

            SoundManager.LoadSceneSoundData(SceneType.ResultScene);

            UIRankingButton .OnClick = () => OpenRanking(inputData.Level, inputData.RankingScoreList);
            UIRetryButton   .OnClick = () => Retry(inputData.Level);
            UIExitButton    .OnClick = () => Exit();

            SetActiveButton(false);

            var task = new StepTask();

            // フェードイン
            task.Push(onNext => UIFade.FadeIn(CommonDef.FadeTimeSec, onNext));

            // 待機
            task.Push(onNext => CoroutineManager.Instance.CallWaitForSeconds(WaitTimeSec, onNext));

            // スコアアニメーション
            task.Push(onNext => UIScoreAnimation.Play(inputData.Score, onNext));

            // 待機
            task.Push(onNext => CoroutineManager.Instance.CallWaitForSeconds(WaitTimeSec, onNext));

            // 新記録 UI 表示
            if (inputData.IsRankIn)
            {
                task.Push(() =>
                {
                    SoundManager.PlaySe(SceneSoundDef.ResultScene.Se.NewScore.ToString());

                    UINewRecordObj.SetActive(true);
                });
            }
            else
            {
                task.Push(() =>
                {
                    SoundManager.PlaySe(SceneSoundDef.ResultScene.Se.Result.ToString());
                });
            }

            // 待機
            task.Push(onNext => CoroutineManager.Instance.CallWaitForSeconds(WaitTimeSec, onNext));

            // ボタン表示
            task.Push(() => SetActiveButton(true));

            task.Process();
        }

        /// <summary>
        /// DoUpdate
        /// </summary>
        protected override void DoUpdate()
        {
            UIScoreAnimation.DoUpdate();
        }

        /// <summary>
        /// DoOnDestroy
        /// </summary>
        protected override void DoOnDestroy()
        {
            SoundManager.ReleaseSceneSoundData();
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// ランキングを開く
        /// </summary>
        /// <param name="level">               難易度                      </param>
        /// <param name="rankingScoreList">    ランキングのスコアリスト    </param>
        private void OpenRanking(Level level, IReadOnlyList<int> rankingScoreList)
        {
            var inputData = new RankingScene.RankingScene.InputData()
            {
                RankingScoreList = new Dictionary<Level, IReadOnlyList<int>>
                {
                    {level, rankingScoreList}
                }
            };

            SceneManager.LoadAdditive(SceneType.RankingScene, inputData);
        }

        /// <summary>
        /// リトライ
        /// </summary>
        /// <param name="level"> レベル </param>
        private void Retry(Level level)
        {
            UIFade.FadeOut(Color.black, CommonDef.FadeTimeSec, () =>
            {
                var inputData = new GameScene.GameScene.InputData() { Level = level };

                SceneManager.Load(SceneType.GameScene, inputData);
            });
        }

        /// <summary>
        /// ゲーム終了
        /// </summary>
        private void Exit()
        {
            UIFade.FadeOut(Color.black, CommonDef.FadeTimeSec, () =>
            {
                SceneManager.Load(SceneType.TitleScene);
            });
        }

        /// <summary>
        /// ボタンアクティブ制御
        /// </summary>
        /// <param name="isActive"> アクティブか </param>
        private void SetActiveButton(bool isActive)
        {
            UIRankingButton .SetActive(isActive);
            UIRetryButton   .SetActive(isActive);
            UIExitButton    .SetActive(isActive);
        }
    }
}

