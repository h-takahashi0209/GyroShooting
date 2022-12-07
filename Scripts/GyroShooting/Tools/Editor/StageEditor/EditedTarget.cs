using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TakahashiH.GyroShooting.Tools.StageEditor
{
    /// <summary>
    /// 編集中の的インターフェース
    /// </summary>
    public interface IEditedTarget
    {
        /// <summary>
        /// ステージの的単体データ
        /// </summary>
        public StageTargetData Data { get; }
    }

    /// <summary>
    /// 編集中の的
    /// </summary>
    public sealed class EditedTarget : IEditedTarget
    {
        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// ステージの的単体データ
        /// </summary>
        public StageTargetData Data { get; private set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Rect
        /// </summary>
        public Rect Rect { get; set; }

        /// <summary>
        /// テクスチャ
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// 移動ベクトルテクスチャ
        /// </summary>
        public Texture2D MoveVecTexture { get; private set; }

        /// <summary>
        /// 半分のサイズ
        /// </summary>
        public float SizeHalf => Size / 2f;

        /// <summary>
        /// エディタ上の座標
        /// </summary>
        public Vector2 EditorPosition => new Vector2(Rect.x + SizeHalf, Rect.y + SizeHalf);


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">           ステージの的単体データ    </param>
        /// <param name="size">           サイズ                    </param>
        /// <param name="rect">           Rect                      </param>
        /// <param name="texByteList">    テクスチャのバイト配列    </param>
        public EditedTarget(StageTargetData data, float size, Rect rect, IReadOnlyList<byte> texByteList)
        {
            Data = data;
            Size = size;
            Rect = rect;

            LoadImage(texByteList);
        }

        /// <summary>
        /// 画像読み込み
        /// </summary>
        /// <param name="texByteList"> 画像のバイト配列 </param>
        public void LoadImage(IReadOnlyList<byte> texByteList)
        {
            Texture = new Texture2D(0, 0);

            Texture.LoadImage(texByteList.ToArray());
        }
    }
}

