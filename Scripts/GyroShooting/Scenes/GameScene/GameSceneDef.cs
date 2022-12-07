
namespace TakahashiH.GyroShooting.Scenes.GameScene.Def
{
    /// <summary>
    /// プレイヤー定義
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// 回転方向
        /// </summary>
        public enum RotateDirection
        {
            Up      ,
            Down    ,
            Right   ,
            Left    ,
        }
    }

    /// <summary>
    /// 弾定義
    /// </summary>
    public static class Bullet
    {
        /// <summary>
        /// 最大連射レベル
        /// </summary>
        public const int MaxRapidBulletLevel = 5;
    }

    /// <summary>
    /// アイテム定義
    /// </summary>
    public static class Item
    {
        /// <summary>
        /// アイテム種別
        /// </summary>
        public enum ItemType
        {
            None = -1           ,
            AddTime             ,   // 時間加算
            AddRapidFireLevel   ,   // 連射レベル加算
            AutoRapidFire       ,   // オート連射
            Sizeof
        }
    }
}
