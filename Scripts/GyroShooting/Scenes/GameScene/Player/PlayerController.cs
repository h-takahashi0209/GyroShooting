using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// プレイヤー制御
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PlayerController : ExMonoBehaviour
    {
        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// カメラのトランスフォーム
        /// </summary>
        [SerializeField] private Transform CameraTransform;

        /// <summary>
        /// 弾制御
        /// </summary>
        [SerializeField] private BulletController BulletController;


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 生成した弾の当たり判定リスト
        /// </summary>
        public IReadOnlyList<IBulletCollision> CollisionList => BulletController.CollisionList;

        /// <summary>
        /// プレイヤー座標
        /// </summary>
        public Vector3 Position => CameraTransform.localPosition;

        /// <summary>
        /// 連射レベル
        /// </summary>
        public int RapidFireLevel => BulletController.RapidFireLevel;

        /// <summary>
        /// オート連射中か
        /// </summary>
        public bool IsAutoRapidFire => BulletController.IsAutoRapidFire;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            BulletController.Setup();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void DoUpdate()
        {
            BulletController.DoUpdate();

            if (BulletController.IsAutoRapidFire)
            {
                _Fire();
            }
        }

        /// <summary>
        /// ジャイロによる回転
        /// </summary>
        /// <param name="quaternion"> クォータニオン </param>
        public void GyroRotate(Quaternion quaternion)
        {
            var qulerAngles = new Vector3(-quaternion.eulerAngles.y + 90f, quaternion.eulerAngles.x, 0f);

            if (qulerAngles.y >= 180f)
            {
                qulerAngles.y = qulerAngles.y - 360f - (360f - qulerAngles.y);
            }

            // 最大角度を超えないようにする
            if (qulerAngles.y > PlayerSetting.MaxAngleY)
            {
                qulerAngles.y = PlayerSetting.MaxAngleY;
            }
            else if (qulerAngles.y < -PlayerSetting.MaxAngleY)
            {
                qulerAngles.y = -PlayerSetting.MaxAngleY;
            }
            if (qulerAngles.x > PlayerSetting.MaxAngleX)
            {
                qulerAngles.x = PlayerSetting.MaxAngleX;
            }
            else if (qulerAngles.x < -PlayerSetting.MaxAngleX)
            {
                qulerAngles.x = -PlayerSetting.MaxAngleX;
            }

            CameraTransform.eulerAngles = qulerAngles;
        }

        /// <summary>
        /// キー入力による回転
        /// </summary>
        /// <param name="direction"> 回転方向 </param>
        public void KeyInputRotate(Def.Player.RotateDirection direction)
        {
            var rotateVec = GetRotateVec(direction);

            CameraTransform.SetEulerAngles(rotateVec);
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        public void Fire()
        {
            // オート連射中は受け付けない
            if (BulletController.IsAutoRapidFire) {
                return;
            }

            _Fire();
        }

        /// <summary>
        /// 弾を消滅させる
        /// </summary>
        /// <param name="id"> 弾の ID </param>
        public void DisappearBullet(int id)
        {
            BulletController.Disappear(id);
        }

        /// <summary>
        /// 連射レベル加算
        /// </summary>
        /// <param name="addedLevel"> 加算するレベル </param>
        public void AddRapidFireLevel(int addedLevel)
        {
            BulletController.AddRapidFireLevel(addedLevel);
        }

        /// <summary>
        /// オート連射制御
        /// </summary>
        /// <param name="isAutoRapidFire"> オート連射するか </param>
        public void SetAutoRapidFire(bool isAutoRapidFire)
        {
            BulletController.IsAutoRapidFire = isAutoRapidFire;
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        public void _Fire()
        {
            Vector3 beginPos  = Position + BulletSetting.FirePosOffset;
            Vector3 targetPos = CameraTransform.forward.normalized * BulletSetting.TargetPosZ - beginPos;

            BulletController.Fire(beginPos, targetPos);
        }

        /// <summary>
        /// 回転ベクトル取得
        /// </summary>
        /// <param name="direction"> 回転方向 </param>
        private Vector3 GetRotateVec(Def.Player.RotateDirection direction)
        {
            var normalizedRotateVec = ToNormalizedRotateVec(direction);
            var nowEulerAngles      = CameraTransform.localEulerAngles;
            var retVec              = nowEulerAngles + (normalizedRotateVec * PlayerSetting.KeyInputMaxRotateSpeed * TimeManager.DeltaTime);

            // 最大角度を超えないようにする
            if (direction == Def.Player.RotateDirection.Left && retVec.y > 180f && retVec.y < (360f - PlayerSetting.MaxAngleY))
            {
                retVec.y = 360f - PlayerSetting.MaxAngleY;
            }
            else if (direction == Def.Player.RotateDirection.Right && retVec.y < 180f && retVec.y > PlayerSetting.MaxAngleY)
            {
                retVec.y = PlayerSetting.MaxAngleY;
            }
            if (direction == Def.Player.RotateDirection.Up && retVec.x > 180f && retVec.x < (360f - PlayerSetting.MaxAngleX))
            {
                retVec.x = 360f - PlayerSetting.MaxAngleX;
            }
            else if (direction == Def.Player.RotateDirection.Down && retVec.x < 180f && retVec.x > PlayerSetting.MaxAngleX)
            {
                retVec.x = PlayerSetting.MaxAngleX;
            }

            return retVec;
        }

        /// <summary>
        /// 正規化回転ベクトルに変換
        /// </summary>
        /// <param name="direction"> 回転方向 </param>
        private Vector3 ToNormalizedRotateVec(Def.Player.RotateDirection direction)
        {
            return direction switch
            {
                Def.Player.RotateDirection.Left  => Vector3.down    ,
                Def.Player.RotateDirection.Right => Vector3.up      ,
                Def.Player.RotateDirection.Up    => Vector3.left    ,
                Def.Player.RotateDirection.Down  => Vector3.right   ,
                _                                => Vector3.zero
            };
        }
    }
}

