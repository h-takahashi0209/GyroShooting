using System;
using System.Collections.Generic;
using System.Text;
using TakahashiH.GyroShooting.Tools.StageEditor.TargetListDef;
using UnityEditor;
using UnityEngine;


namespace TakahashiH.GyroShooting.Tools.StageEditor
{
    /// <summary>
    /// 的リストウインドウ
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TargetListWindow : EditorWindow
    {
        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// 選択リクエスト
        /// </summary>
        private Action<int> mOnReqSelect;

        /// <summary>
        /// 編集リクエスト
        /// </summary>
        private Action<int> mOnReqEdit;

        /// <summary>
        /// 削除リクエスト
        /// </summary>
        private Action<int> mOnReqDelete;

        /// <summary>
        /// 的リストのスクロール座標
        /// </summary>
        private Vector2 mTargetListScrollPos;

        /// <summary>
        /// StringBuilder
        /// </summary>
        private StringBuilder mStringBuilder = new StringBuilder();


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 編集中の的リスト
        /// </summary>
        public IReadOnlyList<IEditedTarget> EditedTargetList { private get; set; }


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// ウインドウを開く
        /// </summary>
        /// <param name="editedtTargetList">    編集中の的リスト    </param>
        /// <param name="onReqSelect">          選択リクエスト      </param>
        /// <param name="onReqEdit">            編集リクエスト      </param>
        /// <param name="onReqDelete">          削除リクエスト      </param>
        public static void Open(IReadOnlyList<IEditedTarget> editedtTargetList, Action<int> onReqSelect, Action<int> onReqEdit, Action<int> onReqDelete)
        {
            var window = GetWindow<TargetListWindow>();

            window.titleContent = new GUIContent("TargetList");

            window.Setup(editedtTargetList, onReqSelect, onReqEdit, onReqDelete);
        }

        /// <summary>
        /// 編集中の的リストを更新
        /// </summary>
        /// <param name="editedTargetList"> 編集中の的リスト </param>
        public static void UpdateEditedTargetList(IReadOnlyList<IEditedTarget> editedTargetList)
        {
            var window = GetWindow<TargetListWindow>();

            window.EditedTargetList = editedTargetList;
        }


        //====================================
        //! 関数（MonoBehaviour）
        //====================================

        /// <summary>
        /// OnGUI
        /// </summary>
        private void OnGUI()
        {
            DrawWindow();
            DrawScroll();
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            mOnReqSelect = null;
            mOnReqEdit   = null;
            mOnReqDelete = null;
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="editedTargetList">    編集中の的リスト    </param>
        /// <param name="onReqSelect">         選択リクエスト      </param>
        /// <param name="onReqEdit">           編集リクエスト      </param>
        /// <param name="onReqDelete">         削除リクエスト      </param>
        public void Setup(IReadOnlyList<IEditedTarget> editedTargetList, Action<int> onReqSelect, Action<int> onReqEdit, Action<int> onReqDelete)
        {
            EditedTargetList = editedTargetList;
            mOnReqSelect     = onReqSelect;
            mOnReqEdit       = onReqEdit;
            mOnReqDelete     = onReqDelete;
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// ウインドウ描画
        /// </summary>
        private void DrawWindow()
        {
            minSize = Window.Size;
            maxSize = Window.Size;
        }

        /// <summary>
        /// スクロール描画
        /// </summary>
        private void DrawScroll()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(Window.Size.x));

            mTargetListScrollPos = EditorGUILayout.BeginScrollView(mTargetListScrollPos, GUI.skin.box);

            for (int i = 0; i < EditedTargetList.Count; i++)
            {
                var stageTargetData = EditedTargetList[i].Data;

                mStringBuilder.Length = 0;
                mStringBuilder.AppendLine($"ID：{i + 1}");
                mStringBuilder.AppendLine($"色：{ToString(stageTargetData.Color)}");
                mStringBuilder.AppendLine($"出現時間：{stageTargetData.AppearTimeSec.ToString("F2")} 〜 {stageTargetData.DisappearTimeSec.ToString("F2")}秒");
                mStringBuilder.AppendLine($"移動パターン：{ToString(stageTargetData.MovePatternType)}");
                mStringBuilder.AppendLine($"座標：({stageTargetData.Position.x.ToString("F2")}, {stageTargetData.Position.y.ToString("F2")}) -> ({stageTargetData.EndPosition.x.ToString("F2")}, {stageTargetData.EndPosition.y.ToString("F2")})");
                mStringBuilder.AppendLine($"移動時間：{stageTargetData.MoveTimeSec.ToString("F2")}秒");

                EditorGUILayout.LabelField(mStringBuilder.ToString(), GUILayout.Width(Window.Size.x), GUILayout.Height(Label.Height));

                EditorGUILayout.BeginHorizontal(GUI.skin.box);

                if (GUILayout.Button("選択", GUILayout.Width(Button.Interval)))
                {
                    mOnReqSelect(stageTargetData.Id);
                }

                if (GUILayout.Button("編集", GUILayout.Width(Button.Interval)))
                {
                    mOnReqEdit(stageTargetData.Id);
                }

                if (GUILayout.Button("削除", GUILayout.Width(Button.Interval)))
                {
                    mOnReqDelete(stageTargetData.Id);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(Label.Space);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 表示用の文字列に変換
        /// </summary>
        /// <param name="color"> 的の色 </param>
        private string ToString(TargetColor color)
        {
            return color switch
            {
                TargetColor.Red   => "赤",
                TargetColor.Green => "緑",
                TargetColor.Blue  => "青",
                _                 => "",
            };
        }

        /// <summary>
        /// 表示用の文字列に変換
        /// </summary>
        /// <param name="movePatternType"> 的の移動パターン種別 </param>
        private string ToString(TargetMovePatternType movePatternType)
        {
            return movePatternType switch
            {
                TargetMovePatternType.Stay   => "待機",
                TargetMovePatternType.Liner  => "直線",
                TargetMovePatternType.Rotate => "回転",
                _                            => "",
            };
        }
    }
}

