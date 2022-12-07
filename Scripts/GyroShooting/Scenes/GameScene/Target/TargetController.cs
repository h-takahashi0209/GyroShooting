using System;
using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的制御
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TargetController : ExMonoBehaviour
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// マテリアルデータ
        /// </summary>
        [Serializable]
        private sealed class MaterialData
        {
            /// <summary>
            /// 色
            /// </summary>
            public TargetColor Color;

            /// <summary>
            /// マテリアル
            /// </summary>
            public Material Material;
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// マテリアルデータリスト
        /// </summary>
        [SerializeField] private MaterialData[] MaterialDataList;

        /// <summary>
        /// 的リスト
        /// </summary>
        [SerializeField] private Target[] TargetList;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 的プール
        /// </summary>
        private PoolSystem<Target> mTargetPool = new PoolSystem<Target>();

        /// <summary>
        /// プレイヤーの座標
        /// </summary>
        private Vector3 mPlayerPos;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 生成した的の当たり判定リスト
        /// </summary>
        public IReadOnlyList<ITargetCollision> CollisionList => mTargetPool.UsedElemList;


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            TargetList = transform.root.GetComponentsInChildren<Target>(true);
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="playerPos"> プレイヤーの座標 </param>
        public void Setup(Vector3 playerPos)
        {
            mPlayerPos = playerPos;

            mTargetPool.Setup(TargetList);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            for (int i = 0; i < mTargetPool.UsedElemList.Count; i++)
            {
                var target = mTargetPool.UsedElemList[i];

                target.DoUpdate();
            }
        }

        /// <summary>
        /// 的を出現させる
        /// </summary>
        /// <param name="stageTargetData"> ステージの的単体データ </param>
        public void Appear(IStageTargetData stageTargetData)
        {
            var target = mTargetPool.Dequeue();

            target.SetLocalPosition(stageTargetData.Position);

            var movePattern = GetMovePattern(target, mPlayerPos, stageTargetData);
            var material    = GetMaterial(stageTargetData.Color);

            target.Setup(stageTargetData.Id, stageTargetData.Color, movePattern, stageTargetData.Position, material);
            target.PlayAnimation(Target.AnimationType.Appear);
        }

        /// <summary>
        /// 時間切れにより的を自然消滅させる
        /// </summary>
        /// <param name="id"> 的の ID </param>
        public void DisappearByTimeOut(int id)
        {
            foreach (var target in mTargetPool.UsedElemList)
            {
                if (target.Id == id && !target.IsAppearing && !target.IsDisappearing)
                {
                    target.SetDead();

                    target.PlayAnimation(Target.AnimationType.Disappear, () =>
                    {
                        mTargetPool.Enqueue(target);
                    });

                    break;
                }
            }
        }

        /// <summary>
        /// 弾の衝突により的を消滅させる
        /// </summary>
        /// <param name="id"> 的の ID </param>
        public void DisappearByHit(int id)
        {
            foreach (var target in mTargetPool.UsedElemList)
            {
                if (target.Id == id && !target.IsAppearing && !target.IsDisappearing)
                {
                    SoundManager.PlaySe(SceneSoundDef.GameScene.Se.HitTarget.ToString());

                    target.SetDead();

                    target.PlayAnimation(Target.AnimationType.Hit, () =>
                    {
                        mTargetPool.Enqueue(target);
                    });

                    break;
                }
            }
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// 移動パターン取得
        /// </summary>
        /// <param name="target">             的                        </param>
        /// <param name="playerPos">          プレイヤーの座標          </param>
        /// <param name="stageTargetData">    ステージの的単体データ    </param>
        private ITargetMovePattern GetMovePattern(Target target, Vector3 playerPos, IStageTargetData stageTargetData)
        {
            return stageTargetData.MovePatternType switch
            {
                TargetMovePatternType.Stay   => new TargetMovePatternStay   (target, playerPos),
                TargetMovePatternType.Liner  => new TargetMovePatternLiner  (target, playerPos, stageTargetData.MoveTimeSec, stageTargetData.EndPosition),
                TargetMovePatternType.Rotate => new TargetMovePatternRotate (target, playerPos, stageTargetData.MoveTimeSec, stageTargetData.EndPosition),
                _                            => new TargetMovePatternStay   (target, playerPos),
            };
        }

        /// <summary>
        /// マテリアル取得
        /// </summary>
        /// <param name="color"> 色 </param>
        private Material GetMaterial(TargetColor color)
        {
            var materialData = MaterialDataList.FirstOrDefault(data => data.Color == color);
            if (materialData == null)
            {
                Debug.LogWarning($"マテリアルデータの取得に失敗しました。 Color : {color}");
                return null;
            }

            return materialData.Material;
        }
    }
}

