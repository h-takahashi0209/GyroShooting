using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// ステージデータ
    /// </summary>
    [Serializable]
    public sealed class StageData
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// ステージの的単体データリスト
        /// </summary>
        [SerializeField] private StageTargetData[] _StageTargetDataList;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// ステージの的単体データリスト
        /// </summary>
        public IReadOnlyList<StageTargetData> StageTargetDataList { get { return _StageTargetDataList; } set { _StageTargetDataList = value.ToArray(); } }

        /// <summary>
        /// ステージの的単体データリスト（読み取り専用）
        /// </summary>
        public IReadOnlyList<IStageTargetData> StageTargetDataListAsReadOnly => _StageTargetDataList;
    }
}

