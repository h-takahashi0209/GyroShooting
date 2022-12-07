using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.LevelSelectScene
{
    /// <summary>
    /// レベル選択シーン
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LevelSelectScene : SceneBase
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// レベルボタンリスト
        /// </summary>
        [SerializeField] private UIButton[] UILevelButtonList;


        //====================================
        //! 関数（SceneBase）
        //====================================

        /// <summary>
        /// DoStart
        /// </summary>
        protected override void DoStart()
        {
            for (int i = 0; i < UILevelButtonList.Length; i++)
            {
                var level = (Level)i;

                UILevelButtonList[i].OnClick = () => StartGame(level);
            }

            UIFade.FadeIn(Color.black, CommonDef.FadeTimeSec);
        }


        //====================================
        //! 関数（SceneBase）
        //====================================

        /// <summary>
        /// ゲーム開始
        /// </summary>
        /// <param name="level"> 難易度 </param>
        private void StartGame(Level level)
        {
            var inputData = new GameScene.GameScene.InputData() {Level = level};

            UIFade.FadeOut(Color.black, CommonDef.FadeTimeSec, () =>
            {
                SceneManager.Load(SceneType.GameScene, inputData);
            });
        }
    }
}

