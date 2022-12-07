using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TakahashiH.GyroShooting
{
    /// <summary>
    /// ランキングセーブデータ IO
    /// </summary>
    public sealed class RankingSaveDataIO
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        private RankingSaveData[] mRankingSaveData = new RankingSaveData[(int)Level.Sizeof];


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="level"> 難易度 </param>
        public void Load(Level level)
        {
            mRankingSaveData[(int)level] = Read(level);
        }

        /// <summary>
        /// 全難易度分を読み込み
        /// </summary>
        public void LoadAll()
        {
            for (int i = (int)Level.Easy; i < (int)Level.Sizeof; i++)
            {
                Load((Level)i);
            }
        }

        /// <summary>
        /// ランキングをリセット
        /// </summary>
        public void ResetRanking()
        {
            for (int i = (int)Level.Easy; i < (int)Level.Sizeof; i++)
            {
                var level = (Level)i;

                mRankingSaveData[(int)level] = new RankingSaveData();

                Write(level);
            }
        }

        /// <summary>
        /// スコア更新
        /// </summary>
        /// <param name="level">       難易度              </param>
        /// <param name="newScore">    新規スコア          </param>
        /// <returns>                  ランクインしたか    </returns>
        public bool UpdateScore(Level level, int newScore)
        {
            if (level <= Level.None || level >= Level.Sizeof)
            {
                Debug.LogWarning($"範囲外のレベルが指定されました。 Level : {level}");
                return false;
            }

            bool isRankin = mRankingSaveData[(int)level].UpdateScore(newScore);

            // ランクインしたらファイル上書き
            if (isRankin)
            {
                Write(level);
            }

            return isRankin;
        }

        /// <summary>
        /// スコアリスト取得
        /// </summary>
        /// <param name="level"> レベル </param>
        public IReadOnlyList<int> GetScoreList(Level level)
        {
            if (level <= Level.None || level >= Level.Sizeof)
            {
                Debug.LogWarning($"範囲外のレベルが指定されました。 Level : {level}");
                return new int[0];
            }

            return mRankingSaveData[(int)level].ScoreList;
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// セーブデータを読み込む
        /// ファイルが存在しない場合は新規作成する
        /// </summary>
        /// <param name="level">    レベル          </param>
        /// <returns>               セーブデータ    </returns>
        private RankingSaveData Read(Level level)
        {
            var filePath = GetSaveDataFilePath(level);
            var fileMode = File.Exists(filePath) ? FileMode.Open : FileMode.Create;

            if (fileMode == FileMode.Create)
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            }

            using var fs = new FileStream(filePath, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var sw = new StreamReader(fs, System.Text.Encoding.UTF8);

            string json = sw.ReadToEnd();

#if UNITY_EDITOR
            AssetDatabase.ImportAsset(filePath);
#endif

            if (json.Length > 0)
            {
                return JsonUtility.FromJson<RankingSaveData>(json);
            }
            else
            {
                return new RankingSaveData();
            }
        }

        /// <summary>
        /// セーブデータを書き込む
        /// </summary>
        /// <param name="level"> レベル </param>
        private void Write(Level level)
        {
            var filePath = GetSaveDataFilePath(level);

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            string json = JsonUtility.ToJson(mRankingSaveData[(int)level], prettyPrint: true);

            sw.Write(json);

#if UNITY_EDITOR
            AssetDatabase.ImportAsset(filePath);
#endif
        }

        /// <summary>
        /// セーブデータファイルのパス取得
        /// </summary>
        /// <param name="level"> レベル </param>
        private string GetSaveDataFilePath(Level level)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return $"Assets/Resources/{string.Format(Path.Scenes.RankingScene.RankingSaveData, level.ToString())}";
#elif UNITY_ANDROID || UNITY_IOS
            return $"{Application.persistentDataPath}/{string.Format(Path.Scenes.RankingScene.RankingSaveData, level.ToString())}";
#else
            return string.Empty;
#endif
        }
    }
}

