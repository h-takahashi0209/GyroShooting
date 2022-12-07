using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// アイテム
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Item : ExMonoBehaviour, IItemCollision
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// アニメーション種別
        /// </summary>
        public enum AnimationType
        {
            Appear      ,
            Disappear   ,
            Hit         ,
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// メッシュレンラダ
        /// </summary>
        [SerializeField] private MeshRenderer MeshRenderer;

        /// <summary>
        /// アニメーション
        /// </summary>
        [SerializeField] private Animation Animation;


        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 消滅時間（秒）
        /// </summary>
        private float mDisappearTimeSec;

        /// <summary>
        /// 消滅リクエスト
        /// </summary>
        private Action mOnReqDisappear;

        /// <summary>
        /// アニメーション再生完了時コールバック
        /// </summary>
        private Action mOnCompAnimation;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// アイテム種別
        /// </summary>
        public Def.Item.ItemType ItemType { get; private set; }

        /// <summary>
        /// ワールド座標
        /// </summary>
        public Vector3 WorldPosition => transform.position;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        public float CollisionRadius { get; private set; }

        /// <summary>
        /// 生存中か
        /// </summary>
        public bool IsAlive { get; private set; }


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            mOnReqDisappear  = null;
            mOnCompAnimation = null;
        }

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            MeshRenderer = GetComponentInChildren<MeshRenderer>();
            Animation    = GetComponent<Animation>();
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="id">                  ID                </param>
        /// <param name="itemType">            アイテム種別      </param>
        /// <param name="position">            出現座標          </param>
        /// <param name="material">            マテリアル        </param>
        /// <param name="disappearTimeSec">    消滅時間（秒）    </param>
        /// <param name="onReqDisappear">      消滅リクエスト    </param>
        public void Setup(int id, Def.Item.ItemType itemType, Vector3 position, Material material, float disappearTimeSec, Action onReqDisappear)
        {
            Id                  = id;
            ItemType            = itemType;
            CollisionRadius     = ItemSetting.CollisionRadius;
            mDisappearTimeSec   = disappearTimeSec;
            mOnReqDisappear     = onReqDisappear;

            MeshRenderer.material = material;

            transform.SetLocalPosition(position);
            transform.ResetEulerAngles();

            Animation.Play(AnimationType.Appear.ToString());
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            mDisappearTimeSec -= TimeManager.DeltaTime;

            if (mDisappearTimeSec <= 0f)
            {
                ActionUtils.CallOnce(ref mOnReqDisappear);
            }
        }

        /// <summary>
        /// アニメーション再生
        /// </summary>
        /// <param name="animationType">    アニメーション種別    </param>
        /// <param name="onComplete">       完了時コールバック    </param>
        public void PlayAnimation(AnimationType animationType, Action onComplete = null)
        {
            mOnCompAnimation = onComplete;

            Animation.Play(animationType.ToString());
        }

        /// <summary>
        /// 非活性設定
        /// </summary>
        public void SetDead()
        {
            IsAlive = false;
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// アニメーションイベント - 出現アニメーション完了
        /// </summary>
        private void AnimationEvent_CompAppearAnimation()
        {
            IsAlive = true;
        }

        /// <summary>
        /// アニメーションイベント - 消滅アニメーション完了
        /// </summary>
        private void AnimationEvent_CompDisappearAnimation()
        {
            ActionUtils.CallOnce(ref mOnCompAnimation);
        }

        /// <summary>
        /// アニメーションイベント - ヒットアニメーション完了
        /// </summary>
        private void AnimationEvent_CompHitAnimation()
        {
            ActionUtils.CallOnce(ref mOnCompAnimation);
        }
    }
}

