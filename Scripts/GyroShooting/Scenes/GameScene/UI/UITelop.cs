using System;
using UnityEngine;


namespace TakahashiH.GyroShooting.Scenes.GameScene
{
    /// <summary>
    /// テロップ UI
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UITelop : ExMonoBehaviour
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// テロップ種別
        /// </summary>
        public enum TelopType
        {
            CountDown   ,
            TimeUp      ,
        }


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// アニメーション
        /// </summary>
        [SerializeField] private Animation Animation;


        //====================================
        //! 変数（SerializeField）
        //====================================

        /// <summary>
        /// アニメーション完了時コールバック
        /// </summary>
        private Action mOnComplete;


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            mOnComplete = null;
        }

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            Animation = GetComponent<Animation>();
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// 再生
        /// </summary>
        /// <param name="telopType">     テロップ種別          </param>
        /// <param name="onComplete">    完了時コールバック    </param>
        public void Play(TelopType telopType, Action onComplete)
        {
            mOnComplete = onComplete;

            Animation.Play(telopType.ToString());
        }


        //====================================
        //! 関数（AnimationEvent）
        //====================================

        /// <summary>
        /// アニメーションイベント - カウントダウン
        /// </summary>
        private void AnimationEvent_CountDown()
        {
            SoundManager.PlaySe(SceneSoundDef.GameScene.Se.CountDown.ToString());
        }

        /// <summary>
        /// アニメーションイベント - ゲーム開始 / 終了
        /// </summary>
        private void AnimationEvent_ComplateAnimation()
        {
            mOnComplete();
        }
    }
}

