using System;
using UnityEngine;


namespace TakahashiH
{
    /// <summary>
    /// サウンドデータ
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SoundData), menuName = "ScriptableObjects/System/Sound/" + nameof(SoundData))]
    public sealed class SoundData : ScriptableObject
    {
        //====================================
        //! 定義
        //====================================

        /// <summary>
        /// AudioClip 単体データ
        /// </summary>
        [Serializable]
        public class AudioClipData
        {
            public string       Name;
            public AudioClip    AudioClip;
        }


        //====================================
        //! 変数（public）
        //====================================

        /// <summary>
        /// SE の AudioClip リスト
        /// </summary>
        public AudioClipData[] SeAudioClipDataList;

        /// <summary>
        /// BGM の AudioClip リスト
        /// </summary>
        public AudioClipData[] BgmAudioClipDataList;
    }
}
