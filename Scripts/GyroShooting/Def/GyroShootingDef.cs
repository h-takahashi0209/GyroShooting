
/// <summary>
/// アプリ内共通定義
/// </summary>
namespace TakahashiH.GyroShooting
{
    /// <summary>
    /// サウンド種別
    /// </summary>
    public enum SoundType
    {
        None    = -1,
        Bgm     ,
        Se      ,
        Sizeof  ,
    }

    /// <summary>
    /// 難易度
    /// </summary>
    public enum Level
    {
        None    = -1,
        Easy    ,
        Normal  ,
        Hard    ,
        Sizeof  ,
    }

    /// <summary>
    /// 的の色
    /// </summary>
    public enum TargetColor
    {
        None = -1,
        Red,
        Green,
        Blue,
        Sizeof,
    }

    /// <summary>
    /// 的の移動パターン種別
    /// </summary>
    public enum TargetMovePatternType
    {
        None = -1,
        Stay,
        Liner,
        Rotate,
        Sizeof,
    }

    /// <summary>
    /// 数値定義
    /// </summary>
    public static class CommonDef
    {
        /// <summary>
        /// フェードにかける時間（秒）
        /// </summary>
        public static float FadeTimeSec = 0.5f;

        /// <summary>
        /// ランキングで保存するスコア数
        /// </summary>
        public static float SavedRankingScoreNum = 5;
    }
}
