using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// ゲームシーン
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameScene : SceneBase
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// 他シーンから渡されるデータ
        /// </summary>
        public class InputData
        {
            /// <summary>
            /// レベル
            /// </summary>
            public Level Level { get; set; }
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// プレイヤー制御
        /// </summary>
        [SerializeField] private PlayerController PlayerController;

        /// <summary>
        /// 的制御
        /// </summary>
        [SerializeField] private TargetController TargetController;

        /// <summary>
        /// アイテム制御
        /// </summary>
        [SerializeField] private ItemController ItemController;

        /// <summary>
        /// UI 管理
        /// </summary>
        [SerializeField] private UIManager UIManager;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 入力管理
        /// </summary>
        private InputManager mInputManager = new InputManager();

        /// <summary>
        /// 当たり判定管理
        /// </summary>
        private CollisionManager mCollisionManager = new CollisionManager();

        /// <summary>
        /// ステージ制御
        /// </summary>
        private StageController mStageController = new StageController();

        /// <summary>
        /// ランキングセーブデータ IO
        /// </summary>
        private RankingSaveDataIO mRankingSaveDataIO = new RankingSaveDataIO();

        /// <summary>
        /// 一時停止中か
        /// </summary>
        private bool mIsPause;

        /// <summary>
        /// レベル
        /// </summary>
        private Level mLevel;

        /// <summary>
        /// BGM サウンドハンドル
        /// </summary>
        private ISoundHandle mBgmSoundHandle;


        //====================================
        //! 関数（SceneBase）
        //====================================

        /// <summary>
        /// DoStart
        /// </summary>
        protected override void DoStart()
        {
            var inputData = SceneManager.SceneInputData as InputData;
            if (inputData == null)
            {
                Debug.LogWarning("ゲームシーンの入力データ取得に失敗しました。");

                mLevel = Level.Easy;
            }
            else
            {
                mLevel = inputData.Level;
            }

            Load();
            Setup();

            mInputManager.SetEnableInput(false);

            UIFade.FadeIn(Color.black, CommonDef.FadeTimeSec, () =>
            {
                UIManager.BeginCountDown(() =>
                {
                    mBgmSoundHandle = SoundManager.PlayBgm(SceneSoundDef.GameScene.Bgm.GameScene.ToString());

                    mStageController.Begin();
                    ItemController.Begin();
                    mInputManager.SetEnableInput(true);
                });
            });
        }

        /// <summary>
        /// DoUpdate
        /// </summary>
        protected override void DoUpdate()
        {
            if (mIsPause) {
                return;
            }

            mInputManager    .DoUpdate();
            PlayerController .DoUpdate();
            TargetController .DoUpdate();
            ItemController   .DoUpdate();
            UIManager        .DoUpdate();
            mStageController .DoUpdate();

            mCollisionManager.Check(PlayerController.CollisionList, TargetController.CollisionList, ItemController.CollisionList);
        }

        /// <summary>
        /// DoOnDestroy
        /// </summary>
        protected override void DoOnDestroy()
        {
            SoundManager.ReleaseSceneSoundData();

            GameSetting         .Dispose();
            PlayerSetting       .Dispose();
            TargetSetting       .Dispose();
            BulletSetting       .Dispose();
            ItemSetting         .Dispose();
            mInputManager       .Dispose();
            mCollisionManager   .Dispose();
            mStageController    .Dispose();
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// 読み込み
        /// </summary>
        private void Load()
        {
            GameSetting   .Load();
            PlayerSetting .Load();
            TargetSetting .Load();
            BulletSetting .Load();
            ItemSetting   .Load();

            SoundManager.LoadSceneSoundData(SceneType.GameScene);

            mStageController.Load(mLevel);
            mRankingSaveDataIO.Load(mLevel);
        }

        /// <summary>
        /// セットアップ
        /// </summary>
        private void Setup()
        {
            mCollisionManager   .OnHitBullet            = id            => PlayerController.DisappearBullet(id);
            mCollisionManager   .OnHitTarget            = (id, score)   => ProcessHitTarget(id, score);
            mCollisionManager   .OnHitItem              = (id, type)    => ProcessHitItem(id, type);
            mStageController    .OnReqAppearTarget      = data          => TargetController.Appear(data);
            mStageController    .OnReqDisappearTarget   = data          => TargetController.DisappearByTimeOut(data);

            PlayerController .Setup();
            TargetController .Setup(PlayerController.Position);
            ItemController   .Setup();
            UIManager        .Setup(Pause, Resume, ProcessTimeOut, EndGame);
            mInputManager    .Setup(PlayerController.GyroRotate, PlayerController.KeyInputRotate, Fire);
            mStageController .Setup();
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        private void Pause()
        {
            mIsPause = true;

            mInputManager.SetEnableInput(false);
        }

        /// <summary>
        /// 再開
        /// </summary>
        private void Resume()
        {
            mIsPause = false;

            mInputManager.SetEnableInput(true);
        }

        /// <summary>
        /// 時間切れ時処理
        /// </summary>
        private void ProcessTimeOut()
        {
            bool isRankIn = mRankingSaveDataIO.UpdateScore(mLevel, UIManager.TotalScore);

            mInputManager.SetEnableInput(false);

            PlayerController.SetAutoRapidFire(false);

            SoundManager.Stop(mBgmSoundHandle);

            SoundManager.PlaySe(SceneSoundDef.GameScene.Se.StartAndEnd.ToString());

            // リザルト画面へ
            UIManager.PlayTimeUpTelop(() =>
            {
                UIFade.FadeOut(Color.black, CommonDef.FadeTimeSec, () =>
                {
                    var inputData = new ResultScene.ResultScene.InputData()
                    {
                        Score            = UIManager.TotalScore,
                        Level            = mLevel,
                        IsRankIn         = isRankIn,
                        RankingScoreList = mRankingSaveDataIO.GetScoreList(mLevel)
                    };

                    SceneManager.Load(SceneType.ResultScene, inputData);
                });
            });
        }

        /// <summary>
        /// ゲーム終了
        /// </summary>
        private void EndGame()
        {
            SoundManager.Stop(mBgmSoundHandle);

            UIFade.FadeOut(Color.black, CommonDef.FadeTimeSec, () =>
            {
                SceneManager.Load(SceneType.TitleScene);
            });
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        private void Fire()
        {
            PlayerController.Fire();
        }

        /// <summary>
        /// 的が衝突したときの処理
        /// </summary>
        /// <param name="targetId">      的の ID    </param>
        /// <param name="addedScore">    スコア     </param>
        private void ProcessHitTarget(int targetId, int addedScore)
        {
            TargetController.DisappearByHit(targetId);

            UIManager.AddScore(addedScore);
        }

        /// <summary>
        /// アイテムが衝突したときの処理
        /// </summary>
        /// <param name="itemId">      アイテムの ID    </param>
        /// <param name="itemType">    アイテム種別     </param>
        private void ProcessHitItem(int itemId, Def.Item.ItemType itemType)
        {
            ItemController.DisappearByHit(itemId);

            switch (itemType)
            {
                case Def.Item.ItemType.AddTime:
                    {
                        SoundManager.PlaySe(SceneSoundDef.GameScene.Se.ItemAddTime.ToString());

                        UIManager.AddTime(ItemSetting.AddedTimeSec);
                    }
                    break;

                case Def.Item.ItemType.AddRapidFireLevel:
                    {
                        SoundManager.PlaySe(SceneSoundDef.GameScene.Se.ItemAddRapidLevel.ToString());

                        PlayerController.AddRapidFireLevel(ItemSetting.AddedRapidFireLevel);
                        UIManager.SetRapidFireLevel(PlayerController.RapidFireLevel);
                    }
                    break;

                case Def.Item.ItemType.AutoRapidFire:
                    {
                        SoundManager.PlaySe(SceneSoundDef.GameScene.Se.ItemAuto.ToString());

                        if (!PlayerController.IsAutoRapidFire)
                        {
                            PlayerController.SetAutoRapidFire(true);
                            UIManager.ShowAutoRapidIcon();
                        }
                    }
                    break;
            }
        }
    }
}

