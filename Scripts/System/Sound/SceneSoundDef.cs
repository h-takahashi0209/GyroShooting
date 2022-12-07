
namespace TakahashiH
{
    /// <summary>
    /// シーン別サウンド定義
    /// </summary>
    public static class SceneSoundDef
    {
        /// <summary>
        /// GameScene
        /// </summary>
        public static class GameScene
        {
            /// <summary>
            /// Se
            /// </summary>
            public enum Se
            {
                CountDown           ,
                StartAndEnd         ,
                Shot                ,
                HitTarget           ,
                ItemAddTime         ,
                ItemAddRapidLevel   ,
                ItemAuto            ,
                Sizeof              ,
            }

            /// <summary>
            /// Bgm
            /// </summary>
            public enum Bgm
            {
                GameScene,
                Sizeof,
            }
        }

        /// <summary>
        /// ResultScene
        /// </summary>
        public static class ResultScene
        {
            /// <summary>
            /// Se
            /// </summary>
            public enum Se
            {
                CountUp     ,
                Result      ,
                NewScore    ,
                Sizeof      ,
            }
        }
    }
}
