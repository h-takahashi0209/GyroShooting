
namespace TakahashiH.GyroShooting.Scenes.Resident
{
    /// <summary>
    /// 常駐シーン
    /// </summary>
    public sealed class ResidentScene : SceneBase
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// DoAwake
        /// </summary>
        protected override void DoAwake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
