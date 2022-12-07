using System;
using System.Collections.Generic;
using TakahashiH.GyroShooting.Tools.StageEditor.ParamEditorDef;
using UnityEditor;
using UnityEngine;


namespace TakahashiH.GyroShooting.Tools.StageEditor
{
    /// <summary>
    /// パラメータエディタウインドウ
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ParamEditorWindow : EditorWindow
    {
        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// ステージの的単体データ
        /// </summary>
        private StageTargetData mStageTargetData = new StageTargetData();

        /// <summary>
        /// 決定時コールバック
        /// </summary>
        private Action<StageTargetData> mOnDecide;

        /// <summary>
        /// 削除リクエスト
        /// </summary>
        private Action<int> mOnReqRemove;

        /// <summary>
        /// 出現時間テキスト
        /// </summary>
        private string mAppearTimeText;

        /// <summary>
        /// 消滅時間テキスト
        /// </summary>
        private string mDisappearTimeText;

        /// <summary>
        /// 出現座標Xテキスト
        /// </summary>
        private string mPositionXText;

        /// <summary>
        /// 出現座標Yテキスト
        /// </summary>
        private string mPositionYText;

        /// <summary>
        /// 出現座標Zテキスト
        /// </summary>
        private string mPositionZText;

        /// <summary>
        /// 目標座標Xテキスト
        /// </summary>
        private string mEndPositionXText;

        /// <summary>
        /// 目標座標Yテキスト
        /// </summary>
        private string mEndPositionYText;

        /// <summary>
        /// 目標座標Zテキスト
        /// </summary>
        private string mEndPositionZText;

        /// <summary>
        /// 移動時間テキスト
        /// </summary>
        private string mMoveTimeText;


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// ウインドウを開く
        /// </summary>
        /// <param name="stageTargetData">    ステージの的単体データ    </param>
        /// <param name="onDecide">           決定時コールバック        </param>
        /// <param name="onReqRemove">        削除リクエスト            </param>
        public static void Open(StageTargetData stageTargetData, Action<StageTargetData> onDecide, Action<int> onReqRemove)
        {
            var window = GetWindow<ParamEditorWindow>();

            window.titleContent = new GUIContent("ParamEditor");

            window.Setup(stageTargetData, onDecide, onReqRemove);
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
            DrawButton();
            DrawLabel();
            DrawTextField();
            DrawToggle();
            CheckKeyEvent();
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            mOnDecide    = null;
            mOnReqRemove = null;
        }


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="stageTargetData">    ステージの的単体データ    </param>
        /// <param name="onDecide">           決定時コールバック        </param>
        /// <param name="onReqDelete">        削除リクエスト            </param>
        public void Setup(StageTargetData stageTargetData, Action<StageTargetData> onDecide, Action<int> onReqRemove)
        {
            mStageTargetData = new StageTargetData()
            {
                Id                  = stageTargetData.Id                ,
                Color               = stageTargetData.Color             ,
                AppearTimeSec       = stageTargetData.AppearTimeSec     ,
                DisappearTimeSec    = stageTargetData.DisappearTimeSec  ,
                MovePatternType     = stageTargetData.MovePatternType   ,
                Position            = stageTargetData.Position          ,
                EndPosition         = stageTargetData.EndPosition       ,
                MoveTimeSec         = stageTargetData.MoveTimeSec       ,
            };

            mOnDecide           = onDecide;
            mOnReqRemove        = onReqRemove;
            mAppearTimeText     = mStageTargetData.AppearTimeSec.ToString();
            mDisappearTimeText  = mStageTargetData.DisappearTimeSec.ToString();
            mPositionXText      = mStageTargetData.Position.x.ToString();
            mPositionYText      = mStageTargetData.Position.y.ToString();
            mPositionZText      = mStageTargetData.Position.z.ToString();
            mEndPositionXText   = mStageTargetData.EndPosition.x.ToString();
            mEndPositionYText   = mStageTargetData.EndPosition.y.ToString();
            mEndPositionZText   = mStageTargetData.EndPosition.z.ToString();
            mMoveTimeText       = mStageTargetData.MoveTimeSec.ToString();
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
        /// ボタン描画
        /// </summary>
        private void DrawButton()
        {
            // キャンセル
            if (GUI.Button(Button.CancelRect, "キャンセル"))
            {
                Close();
            }

            // 削除
            if (GUI.Button(Button.DeleteRect, "削除"))
            {
                mOnReqRemove(mStageTargetData.Id);
                Close();
            }

            // 決定
            if (GUI.Button(Button.DecideRect, "決定"))
            {
                if (ValidateParam())
                {
                    mOnDecide(mStageTargetData);
                    Close();
                }
            }
        }

        /// <summary>
        /// ラベル描画
        /// </summary>
        private void DrawLabel()
        {
            GUI.Label(Label.IdRect              , "ID"              );
            GUI.Label(Label.ColorRect           , "色"              );
            GUI.Label(Label.AppearTimeRect      , "出現時間(秒)"    );
            GUI.Label(Label.DisappearTimeRect   , "消滅時間(秒)"    );
            GUI.Label(Label.MovePatternRect     , "移動パターン"    );
            GUI.Label(Label.PositionRect        , "出現座標"        );
            GUI.Label(Label.PositionXRect       , "X"               );
            GUI.Label(Label.PositionYRect       , "Y"               );
            GUI.Label(Label.PositionZRect       , "Z"               );
            GUI.Label(Label.EndPositionRect     , "目標座標"        );
            GUI.Label(Label.EndPositionXRect    , "X"               );
            GUI.Label(Label.EndPositionYRect    , "Y"               );
            GUI.Label(Label.EndPositionZRect    , "Z"               );
            GUI.Label(Label.MoveTimeRect        , "移動時間(秒)"    );
        }

        /// <summary>
        /// テキストフィールド描画
        /// </summary>
        private void DrawTextField()
        {
            // ID
            GUI.TextField(new Rect(100f,  5f, 200f, 20f), mStageTargetData.Id.ToString());

            // 出現時間
            {
                mAppearTimeText = GUI.TextField(TextField.AppearTimeRect, mAppearTimeText);

                if (float.TryParse(mAppearTimeText, out float appearTimeSec))
                {
                    mStageTargetData.AppearTimeSec = appearTimeSec;
                }
            }

            // 消滅時間
            {
                mDisappearTimeText = GUI.TextField(TextField.DisappearTimeRect, mDisappearTimeText);

                if (float.TryParse(mDisappearTimeText, out float disappearTimeSec))
                {
                    mStageTargetData.DisappearTimeSec = disappearTimeSec;
                }
            }

            // 出現座標X
            {
                mPositionXText = GUI.TextField(TextField.PositionXRect, mPositionXText);

                if (float.TryParse(mPositionXText, out float positionX))
                {
                    mStageTargetData.Position = new Vector3(positionX, mStageTargetData.Position.y, mStageTargetData.Position.z);
                }
            }

            // 出現座標Y
            {
                mPositionYText = GUI.TextField(TextField.PositionYRect, mPositionYText);

                if (float.TryParse(mPositionYText, out float positionY))
                {
                    mStageTargetData.Position = new Vector3(mStageTargetData.Position.x, positionY, mStageTargetData.Position.z);
                }
            }

            // 出現座標Z
            {
                mPositionZText = GUI.TextField(TextField.PositionZRect, mPositionZText);

                if (float.TryParse(mPositionZText, out float positionZ))
                {
                    mStageTargetData.Position = new Vector3(mStageTargetData.Position.x, mStageTargetData.Position.y, positionZ);
                }
            }

            // 目標座標X
            {
                mEndPositionXText = GUI.TextField(TextField.EndPositionXRect, mEndPositionXText);

                if (float.TryParse(mEndPositionXText, out float moveVecX))
                {
                    mStageTargetData.EndPosition = new Vector3(moveVecX, mStageTargetData.EndPosition.y, mStageTargetData.EndPosition.z);
                }
            }

            // 目標座標Y
            {
                mEndPositionYText = GUI.TextField(TextField.EndPositionYRect, mEndPositionYText);

                if (float.TryParse(mEndPositionYText, out float moveVecY))
                {
                    mStageTargetData.EndPosition = new Vector3(mStageTargetData.EndPosition.x, moveVecY, mStageTargetData.EndPosition.z);
                }
            }

            // 目標座標Z
            {
                mEndPositionZText = GUI.TextField(TextField.EndPositionZRect, mEndPositionZText);

                if (float.TryParse(mEndPositionZText, out float moveVecZ))
                {
                    mStageTargetData.EndPosition = new Vector3(mStageTargetData.EndPosition.x, mStageTargetData.EndPosition.y, moveVecZ);
                }
            }

            // 移動時間
            {
                mMoveTimeText = GUI.TextField(TextField.MoveTimeRect, mMoveTimeText);

                if (float.TryParse(mMoveTimeText, out float moveTimeSec))
                {
                    mStageTargetData.MoveTimeSec = moveTimeSec;
                }
            }
        }

        /// <summary>
        /// トグル描画
        /// </summary>
        private void DrawToggle()
        {
            // 色
            {
                if (GUI.Toggle(Toggle.ColorRedRect, mStageTargetData.Color == TargetColor.Red, "赤"))
                {
                    mStageTargetData.Color = TargetColor.Red;
                }

                if (GUI.Toggle(Toggle.ColorGreenRect, mStageTargetData.Color == TargetColor.Green, "緑"))
                {
                    mStageTargetData.Color = TargetColor.Green;
                }

                if (GUI.Toggle(Toggle.ColorBlueRect, mStageTargetData.Color == TargetColor.Blue, "青"))
                {
                    mStageTargetData.Color = TargetColor.Blue;
                }
            }

            // 移動パターン
            {
                if (GUI.Toggle(Toggle.MovePatternWaitRect, mStageTargetData.MovePatternType == TargetMovePatternType.Stay, "待機"))
                {
                    mStageTargetData.MovePatternType = TargetMovePatternType.Stay;
                }

                if (GUI.Toggle(Toggle.MovePatternMoveRect, mStageTargetData.MovePatternType == TargetMovePatternType.Liner, "直線"))
                {
                    mStageTargetData.MovePatternType = TargetMovePatternType.Liner;
                }

                if (GUI.Toggle(Toggle.MovePatternRotateRect, mStageTargetData.MovePatternType == TargetMovePatternType.Rotate, "回転"))
                {
                    mStageTargetData.MovePatternType = TargetMovePatternType.Rotate;
                }
            }
        }

        /// <summary>
        /// キーイベントのチェック
        /// </summary>
        private void CheckKeyEvent()
        {
            // Escape キー押下でウインドウを閉じる
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                Close();
            }
        }

        /// <summary>
        /// パラメータの整合性チェック
        /// 不正な値が入っていたらダイアログで通知
        /// </summary>
        /// <returns> 不正な値が入っていないか </returns>
        private bool ValidateParam()
        {
            if (mStageTargetData.DisappearTimeSec < mStageTargetData.AppearTimeSec)
            {
                EditorUtility.DisplayDialog("エラー", "消滅時間が出現時間より前の時間になっています。", "OK");
                return false;
            }

            if ((mStageTargetData.MovePatternType == TargetMovePatternType.Liner || mStageTargetData.MovePatternType == TargetMovePatternType.Rotate) && mStageTargetData.MoveTimeSec <= 0f)
            {
                EditorUtility.DisplayDialog("エラー", "移動パターンが直線・回転で、移動時間が0以下になっています。", "OK");
                return false;
            }

            return true;
        }
    }
}

