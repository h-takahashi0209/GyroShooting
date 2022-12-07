using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// 的インターフェース
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// ローカル座標設定
        /// </summary>
        void SetLocalPosition(Vector3 position);

        /// <summary>
        /// 指定した座標を中心に回転させる
        /// </summary>
        /// <param name="centerPos">    中心座標    </param>
        /// <param name="angle">        角度        </param>
        void RotateAround(Vector3 centerPos, float angle);

        /// <summary>
        /// ローカル座標取得
        /// </summary>
        Vector3 LocalPosition { get; }

        /// <summary>
        /// 消滅中か
        /// </summary>
        bool IsDisappearing { get; }
    }

    /// <summary>
    /// 的
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Target : ExMonoBehaviour, ITarget, ITargetCollision
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
        /// 移動パターン
        /// </summary>
        private ITargetMovePattern mMovePattern;

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
        /// 色
        /// </summary>
        public TargetColor Color { get; private set; }

        /// <summary>
        /// ワールド座標
        /// </summary>
        public Vector3 WorldPosition => transform.position;

        /// <summary>
        /// ローカル座標
        /// </summary>
        public Vector3 LocalPosition => transform.localPosition;

        /// <summary>
        /// 当たり判定半径
        /// </summary>
        public float CollisionRadius { get; private set; }

        /// <summary>
        /// スコア
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// 出現中か
        /// </summary>
        public bool IsAppearing { get; private set; }

        /// <summary>
        /// 消滅中か
        /// </summary>
        public bool IsDisappearing { get; private set; }

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
        /// <param name="id">             ID              </param>
        /// <param name="color">          色              </param>
        /// <param name="movePattern">    移動パターン    </param>
        /// <param name="defaultPos">     初期座標        </param>
        /// <param name="material">       マテリアル      </param>
        public void Setup(int id, TargetColor color, ITargetMovePattern movePattern, Vector3 defaultPos, Material material)
        {
            float scale = TargetSetting.GetScale(color);

            Id              = id;
            Color           = color;
            mMovePattern    = movePattern;
            CollisionRadius = TargetSetting.GetCollisionRadius(color);
            Score           = TargetSetting.GetScore(color);
            IsAppearing     = false;
            IsDisappearing  = false;
            IsAlive         = true;

            MeshRenderer.material = material;

            transform.SetLocalScale(new Vector3(scale, scale, 1f));
            transform.ResetEulerAngles();

            SetLocalPosition(defaultPos);
            PlayAnimation(AnimationType.Appear);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            mMovePattern.Move();
        }

        /// <summary>
        /// アニメーション再生
        /// </summary>
        /// <param name="animationType">    アニメーション種別    </param>
        /// <param name="onComplete">       完了時コールバック    </param>
        public void PlayAnimation(AnimationType animationType, Action onComplete = null)
        {
            mOnCompAnimation = onComplete;

            switch (animationType)
            {
                case AnimationType.Appear    : IsAppearing    = true;   break;
                case AnimationType.Disappear : IsDisappearing = true;   break;
                case AnimationType.Hit       : IsDisappearing = true;   break;
            }

            Animation.Play(animationType.ToString());
        }

        /// <summary>
        /// ローカル座標設定
        /// </summary>
        /// <param name="position"> 座標 </param>
        public void SetLocalPosition(Vector3 position)
        {
            transform.SetLocalPosition(position);
        }

        /// <summary>
        /// 指定した座標を中心に回転させる
        /// </summary>
        /// <param name="centerPos">    中心座標    </param>
        /// <param name="angle">        角度        </param>
        public void RotateAround(Vector3 centerPos, float angle)
        {
            transform.RotateAround(centerPos, Vector3.forward, angle);

            transform.SetEulerAnglesZ(transform.GetEulerAnglesZ() - angle);
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
            IsAppearing = false;
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

