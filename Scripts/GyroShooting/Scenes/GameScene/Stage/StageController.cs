using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// ステージ制御
    /// </summary>
    public sealed class StageController : IDisposable
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 待機中のステージの的単体データリスト
        /// </summary>
        private List<IStageTargetData> mWaitStageTargetDataList = new List<IStageTargetData>();

        /// <summary>
        /// 出現中のステージの的単体データリスト
        /// </summary>
        private List<IStageTargetData> mActiveStageTargetDataList = new List<IStageTargetData>();

        /// <summary>
        /// 削除対象のステージの的単体データリスト
        /// </summary>
        private List<IStageTargetData> mRemovedStageTargetDataList = new List<IStageTargetData>();

        /// <summary>
        /// 経過時間（秒）
        /// </summary>
        private float mElapsedTimeSec;

        /// <summary>
        /// 開始済みか
        /// </summary>
        private bool mIsBegin;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 的出現リクエスト
        /// </summary>
        public Action<IStageTargetData> OnReqAppearTarget { private get; set; }

        /// <summary>
        /// 的消滅リクエスト
        /// </summary>
        public Action<int> OnReqDisappearTarget { private get; set; }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="level"> レベル </param>
        public void Load(Level level)
        {
            string path = $"{Path.Scenes.GameScene.StageData}{level}";

            var stageDataText   = Resources.Load<TextAsset>(path);
            var stageData       = JsonUtility.FromJson<StageData>(stageDataText?.text);
            if (stageData == null)
            {
                Debug.Assert(false, "ステージデータの読み込みに失敗しました。");
                return;
            }

            mWaitStageTargetDataList = stageData.StageTargetDataListAsReadOnly.OrderBy(data => data.AppearTimeSec).ToList();
        }

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            mElapsedTimeSec = 0f;
            mIsBegin        = false;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public void Begin()
        {
            mIsBegin = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            if (!mIsBegin) {
                return;
            }

            mElapsedTimeSec += TimeManager.DeltaTime;

            mRemovedStageTargetDataList.Clear();

            // 消滅時間を過ぎた的の消滅リクエストを呼ぶ
            foreach (var stageTargetData in mActiveStageTargetDataList)
            {
                if (mElapsedTimeSec < stageTargetData.DisappearTimeSec) {
                    continue;
                }

                OnReqDisappearTarget(stageTargetData.Id);

                mRemovedStageTargetDataList.Add(stageTargetData);
            }

            // 消滅時間を過ぎた的のデータを削除
            foreach (var stageTargetData in mRemovedStageTargetDataList)
            {
                mActiveStageTargetDataList.Remove(stageTargetData);
            }

            mRemovedStageTargetDataList.Clear();

            // 出現時間を過ぎた的の出現リクエストを呼ぶ
            foreach (var stageTargetData in mWaitStageTargetDataList)
            {
                if (mElapsedTimeSec < stageTargetData.AppearTimeSec) {
                    break;
                }

                OnReqAppearTarget(stageTargetData);

                mActiveStageTargetDataList  .Add(stageTargetData);
                mRemovedStageTargetDataList .Add(stageTargetData);
            }

            // 出現時間を過ぎた的のデータを削除
            foreach (var stageTargetData in mRemovedStageTargetDataList)
            {
                mWaitStageTargetDataList.Remove(stageTargetData);
            }
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            OnReqAppearTarget = null;
        }
    }
}

