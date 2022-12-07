using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace TakahashiH.GyroShooting.Scenes.RankingScene
{
    /// <summary>
    /// ランキングシーン
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RankingScene : SceneBase
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
            /// ランキングのスコアリスト
            /// </summary>
            public IReadOnlyDictionary<Level, IReadOnlyList<int>> RankingScoreList { get; set; }
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// スコアテキストリスト
        /// </summary>
        [SerializeField] private Text[] UIScoreTextList;

        /// <summary>
        /// レベルテキスト
        /// </summary>
        [SerializeField] private Text UILevelText;

        /// <summary>
        /// 順位ごとのスコア色リスト
        /// </summary>
        [SerializeField] private Color[] ScoreColorList;

        /// <summary>
        /// 左カーソルボタン
        /// </summary>
        [SerializeField] private UIButton UILeftCursorButton;

        /// <summary>
        /// 右カーソルボタン
        /// </summary>
        [SerializeField] private UIButton UIRightCursorButton;

        /// <summary>
        /// 閉じるボタン
        /// </summary>
        [SerializeField] private UIButton UICloseButton;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// ランキングのスコアリスト
        /// </summary>
        private IReadOnlyDictionary<Level, IReadOnlyList<int>> mRankingScoreList;

        /// <summary>
        /// 選択中のレベル
        /// </summary>
        private Level mCurrentLevel;


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
                Debug.LogWarning("ランキングシーンの入力データ取得に失敗しました。");

                inputData = new InputData()
                {
                    RankingScoreList = new Dictionary<Level, IReadOnlyList<int>>()
                    {
                        {Level.Easy   , new int[(int)CommonDef.SavedRankingScoreNum]},
                        {Level.Normal , new int[(int)CommonDef.SavedRankingScoreNum]},
                        {Level.Hard   , new int[(int)CommonDef.SavedRankingScoreNum]}
                    }
                };
            }

            mRankingScoreList = inputData.RankingScoreList;
            mCurrentLevel     = mRankingScoreList.FirstOrDefault().Key;

            SetupRanking(mCurrentLevel);
            UpdateCursor();

            UICloseButton       .OnClick = () => SceneManager.Unload(SceneType.RankingScene);
            UILeftCursorButton  .OnClick = () => ChangePage(-1);
            UIRightCursorButton .OnClick = () => ChangePage( 1);
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// ランキングのセットアップ
        /// </summary>
        /// <param name="level"> レベル </param>
        private void SetupRanking(Level level)
        {
            var rankingScoreList = mRankingScoreList[level];

            for (int i = 0; i < CommonDef.SavedRankingScoreNum; i++)
            {
                var textUI = UIScoreTextList[i];

                textUI.text = $"{i + 1}{GetRankAddedText(i + 1)}          " + "{0, 8}".Format(rankingScoreList[i]);
                textUI.color = ScoreColorList[i];
            }

            UILevelText.text = level.ToString();
        }

        /// <summary>
        /// ページ切り替え
        /// </summary>
        /// <param name="addedLevel"> 加算するレベル </param>
        private void ChangePage(int addedLevel)
        {
            mCurrentLevel = (Level)Mathf.Clamp((int)mCurrentLevel + addedLevel, (int)Level.Easy, (int)Level.Hard);

            SetupRanking(mCurrentLevel);
            UpdateCursor();
        }

        /// <summary>
        /// カーソル表示更新
        /// </summary>
        private void UpdateCursor()
        {
            var headLevel = mRankingScoreList.FirstOrDefault().Key;
            var tailLevel = mRankingScoreList.LastOrDefault().Key;

            UILeftCursorButton  .SetActive(headLevel != mCurrentLevel);
            UIRightCursorButton .SetActive(tailLevel != mCurrentLevel);
        }

        /// <summary>
        /// 順位に付け足す文言取得
        /// </summary>
        /// <param name="rank"> ランク </param>
        private string GetRankAddedText(int rank)
        {
            return rank switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }
    }
}

