using System;
using UnityEngine;


namespace TakahashiH.GyroShooting
{
    /// <summary>
    /// ステージの的単体データインターフェース
    /// </summary>
    public interface IStageTargetData
    {
        /// <summary>
        /// ID
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 色
        /// </summary>
        public TargetColor Color { get; }

        /// <summary>
        /// 出現時間（秒）
        /// </summary>
        float AppearTimeSec { get; }

        /// <summary>
        /// 消滅時間（秒）
        /// </summary>
        float DisappearTimeSec { get; }

        /// <summary>
        /// 移動パターン種別
        /// </summary>
        TargetMovePatternType MovePatternType { get; }

        /// <summary>
        /// 出現座標
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 目標座標
        /// </summary>
        Vector3 EndPosition { get; }

        /// <summary>
        /// 移動時間（秒）
        /// </summary>
        float MoveTimeSec { get; }
    }

    /// <summary>
    /// ステージの的単体データ
    /// </summary>
    [Serializable]
    public sealed class StageTargetData : IStageTargetData
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// ID
        /// </summary>
        [SerializeField] private int _Id;

        /// <summary>
        /// 色
        /// </summary>
        [SerializeField] private TargetColor _Color;

        /// <summary>
        /// 出現時間（秒）
        /// </summary>
        [SerializeField] private float _AppearTimeSec;

        /// <summary>
        /// 消滅時間（秒）
        /// </summary>
        [SerializeField] private float _DisappearTimeSec;

        /// <summary>
        /// 移動パターン種別
        /// </summary>
        [SerializeField] private TargetMovePatternType _MovePatternType;

        /// <summary>
        /// 出現座標
        /// </summary>
        [SerializeField] private Vector3 _Position;

        /// <summary>
        /// 目標座標
        /// </summary>
        [SerializeField] private Vector3 _EndPosition;

        /// <summary>
        /// 移動時間（秒）
        /// </summary>
        [SerializeField] private float _MoveTimeSec;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get { return _Id; } set { _Id = value; } }

        /// <summary>
        /// 色
        /// </summary>
        public TargetColor Color { get { return _Color; } set { _Color = value; } }

        /// <summary>
        /// 出現時間（秒）
        /// </summary>
        public float AppearTimeSec { get { return _AppearTimeSec; } set { _AppearTimeSec = value; } }

        /// <summary>
        /// 消滅時間（秒）
        /// </summary>
        public float DisappearTimeSec { get { return _DisappearTimeSec; } set { _DisappearTimeSec = value; } }

        /// <summary>
        /// 移動パターン種別
        /// </summary>
        public TargetMovePatternType MovePatternType { get { return _MovePatternType; } set { _MovePatternType = value; } }

        /// <summary>
        /// 出現座標
        /// </summary>
        public Vector3 Position { get { return _Position; } set { _Position = value; } }

        /// <summary>
        /// 目標座標
        /// </summary>
        public Vector3 EndPosition { get { return _EndPosition; } set { _EndPosition = value; } }

        /// <summary>
        /// 移動時間（秒）
        /// </summary>
        public float MoveTimeSec { get { return _MoveTimeSec; } set { _MoveTimeSec = value; } }
    }
}

