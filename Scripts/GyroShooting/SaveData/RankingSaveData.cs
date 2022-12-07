using System;
using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting
{
    /// <summary>
    /// ランキングセーブデータ
    /// </summary>
    [Serializable]
    public sealed class RankingSaveData
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// 保存するスコアの数
        /// </summary>
        private const int SavedScoreNum = 5;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// スコアリスト
        /// </summary>
        [SerializeField] private int[] _ScoreList = new int[SavedScoreNum];


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// スコアリスト
        /// </summary>
        public IReadOnlyList<int> ScoreList => _ScoreList;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// スコア更新
        /// </summary>
        /// <param name="newScore">    新規スコア          </param>
        /// <returns>                  ランクインしたか    </returns>
        public bool UpdateScore(int newScore)
        {
            int newScoreIdx = -1;

            // ランクインしたインデックスを取得
            for (int i = SavedScoreNum - 1; i >= 0; i--)
            {
                int score = _ScoreList[i];

                if (newScore > score)
                {
                    newScoreIdx = i;
                }
                else
                {
                    break;
                }
            }

            // ランク外
            if (newScoreIdx < 0)
            {
                return false;
            }

            // 新規スコアで上書きし、古いスコアをずらしていく
            for (int i = SavedScoreNum - 1; i >= 0; i--)
            {
                if (i < SavedScoreNum - 1)
                {
                    _ScoreList[i + 1] = _ScoreList[i];
                }

                if (i == newScoreIdx)
                {
                    _ScoreList[i] = newScore;
                    return true;
                }
            }

            return false;
        }
    }
}

