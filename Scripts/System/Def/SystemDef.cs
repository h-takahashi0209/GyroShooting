
/// <summary>
/// システム共通定義
/// </summary>
namespace TakahashiH
{
    /// <summary>
    /// シーン種別
    /// </summary>
    public enum SceneType
    {
        None                = -1,
        ResidentScene       ,
        TitleScene          ,
        LevelSelectScene    ,
        GameScene           ,
        ResultScene         ,
        RankingScene        ,
        Sizeof              ,
    }

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
}
