using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TakahashiH.GyroShooting.Scenes.GameScene;
using TakahashiH.GyroShooting.Tools.StageEditor.StageEditorDef;
using UnityEditor;
using UnityEngine;


namespace TakahashiH.GyroShooting.Tools.StageEditor
{
    /// <summary>
    /// ステージエディタウインドウ
    /// </summary>
    public sealed class StageEditorWindow : EditorWindow
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 的データリスト
        /// </summary>
        private List<EditedTarget> mEditedTargetList = new List<EditedTarget>();

        /// <summary>
        /// クリックした的
        /// </summary>
        private EditedTarget mClickedTarget;

        /// <summary>
        /// 下地テクスチャ
        /// </summary>
        private Texture mBaseTexture;

        /// <summary>
        /// カーソルテクスチャ
        /// </summary>
        private Texture mCursorTexture;

        /// <summary>
        /// 巻き戻しボタンテクスチャ
        /// </summary>
        private Texture mRewindButtonTexture;

        /// <summary>
        /// 再生ボタンテクスチャ
        /// </summary>
        private Texture mPlayButtonTexture;

        /// <summary>
        /// 早送りボタンテクスチャ
        /// </summary>
        private Texture mFastForwardButtonTexture;

        /// <summary>
        /// 一時停止ボタンテクスチャ
        /// </summary>
        private Texture mPauseButtonTexture;

        /// <summary>
        /// 停止ボタンテクスチャ
        /// </summary>
        private Texture mStopButtonTexture;

        /// <summary>
        /// 赤の的画像バイトデータ
        /// </summary>
        private IReadOnlyList<byte> mTargetRedTexByteData;

        /// <summary>
        /// 緑の的画像バイトデータ
        /// </summary>
        private IReadOnlyList<byte> mTargetGreenTexByteData;

        /// <summary>
        /// 青の的画像バイトデータ
        /// </summary>
        private IReadOnlyList<byte> mTargetBlueTexByteData;

        /// <summary>
        /// クリック座標
        /// </summary>
        private Vector2 mClickedPos;

        /// <summary>
        /// クリックした的のオフセット
        /// </summary>
        private Vector2 mClickedTargetOffset;

        /// <summary>
        /// 開いているファイル名
        /// </summary>
        private string mOpenedFileName;

        /// <summary>
        /// 再生開始時間（秒）
        /// </summary>
        private double mStartTimeSec;

        /// <summary>
        /// 一時停止時間（秒）
        /// </summary>
        private double mPauseTimeSec;

        /// <summary>
        /// 最大時間（秒）
        /// </summary>
        private float mMaxTimeSec;

        /// <summary>
        /// 再生中か
        /// </summary>
        private bool mIsPlaying;

        /// <summary>
        /// 的の移動中か
        /// </summary>
        private bool mIsMovingTarget;

        /// <summary>
        /// 編集したか
        /// </summary>
        private bool mIsEdited;


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
            DrawSlider();
            DrawTexture();
            DrawLabel();
            CheckMouseEvent();
            CheckKeyEvent();
        }

        /// <summary>
        /// OnEnable
        /// </summary>
        private void OnEnable()
        {
            EditorApplication.update += DoUpdate;
        }

        /// <summary>
        /// OnDisable
        /// </summary>
        private void OnDisable()
        {
            EditorApplication.update -= DoUpdate;
        }


        //====================================
        //! 関数（public static）
        //====================================

        /// <summary>
        /// ウインドウを開く
        /// </summary>
        [MenuItem("GyroShooting/StageEditor", false, 1)]
        private static void Open()
        {
            GetWindow<StageEditorWindow>().Setup();
        }


        //====================================
        //! 関数（private）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        private void Setup()
        {
            Texture2D texture;

            // 下地画像
            {
                texture = new Texture2D(1, 1);

                texture.SetPixel(0, 0, Color.gray);
                texture.Apply();

                mBaseTexture = texture;
            }

            // 巻き戻しボタン画像
            {
                texture = new Texture2D(0, 0);

                var byteData = File.ReadAllBytes(Tex.RewindButtonPath);

                texture.LoadImage(byteData);

                mRewindButtonTexture = texture;
            }

            // 再生ボタン画像
            {
                texture = new Texture2D(0, 0);

                var byteData = File.ReadAllBytes(Tex.PlayButtonPath);

                texture.LoadImage(byteData);

                mPlayButtonTexture = texture;
            }

            // 早送りボタン画像
            {
                texture = new Texture2D(0, 0);

                var byteData = File.ReadAllBytes(Tex.FastForwardButtonPath);

                texture.LoadImage(byteData);

                mFastForwardButtonTexture = texture;
            }

            // 一時停止ボタン画像
            {
                texture = new Texture2D(0, 0);

                var byteData = File.ReadAllBytes(Tex.PauseButtonPath);

                texture.LoadImage(byteData);

                mPauseButtonTexture = texture;
            }

            // 停止ボタン画像
            {
                texture = new Texture2D(0, 0);

                var byteData = File.ReadAllBytes(Tex.StopButtonPath);

                texture.LoadImage(byteData);

                mStopButtonTexture = texture;
            }

            // 的画像
            {
                mTargetRedTexByteData   = File.ReadAllBytes(Tex.TargetRedPath);
                mTargetGreenTexByteData = File.ReadAllBytes(Tex.TargetGreenPath);
                mTargetBlueTexByteData  = File.ReadAllBytes(Tex.TargetBluePath);
            }

            // カーソル画像
            {
                texture = new Texture2D(0, 0);

                var byteData = File.ReadAllBytes(Tex.CursorPath);

                texture.LoadImage(byteData);

                mCursorTexture = texture;
            }

            GameSetting   .Load();
            TargetSetting .Load();
            ItemSetting   .Load();

            mMaxTimeSec     = GameSetting.DefRemainingTimeSec + ItemSetting.AddedTimeSec;
            mOpenedFileName = Label.DefFileNameText;
            mIsPlaying      = false;
            mPauseTimeSec   = 0f;

            GameSetting   .Dispose();
            TargetSetting .Dispose();
            ItemSetting   .Dispose();
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void DoUpdate()
        {
            // 最後まで再生したら停止
            if (mIsPlaying && (EditorApplication.timeSinceStartup - mStartTimeSec) >= mMaxTimeSec)
            {
                mIsPlaying    = false;
                mPauseTimeSec = 0f;
            }

            Repaint();
        }

        /// <summary>
        /// ウインドウ描画
        /// </summary>
        private void DrawWindow()
        {
            minSize = Window.Size;
            maxSize = Window.Size;

            string windowName = "StageEditor" + (mIsEdited ? "*" : string.Empty);

            titleContent = new GUIContent(windowName);
        }

        /// <summary>
        /// ボタン描画
        /// </summary>
        private void DrawButton()
        {
            if (GUI.Button(Button.RewindButton, mRewindButtonTexture))
            {
                Rewind();
            }

            if (GUI.Button(Button.PlayRect, mPlayButtonTexture))
            {
                Play();
            }

            if (GUI.Button(Button.FastForwardButton, mFastForwardButtonTexture))
            {
                FastForward();
            }

            if (GUI.Button(Button.PauseRect, mPauseButtonTexture))
            {
                Pause();
            }

            if (GUI.Button(Button.StopRect, mStopButtonTexture))
            {
                Stop();
            }

            if (GUI.Button(Button.LoadRect, "読み込み"))
            {
                Load();
            }

            if (GUI.Button(Button.SaveRect, "保存"))
            {
                Save();
            }
        }

        /// <summary>
        /// スライダー描画
        /// </summary>
        private void DrawSlider()
        {
            if (mIsPlaying)
            {
                float elapsedTimeSec = (float)(EditorApplication.timeSinceStartup - mStartTimeSec);

                mPauseTimeSec = GUI.HorizontalSlider(Slider.SeekBarRect, elapsedTimeSec, 0f, mMaxTimeSec);
                mPauseTimeSec = (float)Math.Round(mPauseTimeSec, 2, MidpointRounding.AwayFromZero);
            }
            else if (HasOpenInstances<ParamEditorWindow>() || string.IsNullOrEmpty(mOpenedFileName))
            {
                GUI.HorizontalSlider(Slider.SeekBarRect, (float)mPauseTimeSec, 0f, mMaxTimeSec);
            }
            else
            {
                mPauseTimeSec = GUI.HorizontalSlider(Slider.SeekBarRect, (float)mPauseTimeSec, 0f, mMaxTimeSec);
                mPauseTimeSec = (float)Math.Round(mPauseTimeSec, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 画像描画
        /// </summary>
        private void DrawTexture()
        {
            // 下地描画
            GUI.DrawTexture(Tex.TargetScreenRect, mBaseTexture, ScaleMode.StretchToFill, true, 0);

            Handles.color = Color.white;

            EditedTarget focusedTarget = null;

            foreach (var editedTarget in mEditedTargetList)
            {
                if (mPauseTimeSec < editedTarget.Data.AppearTimeSec || mPauseTimeSec > editedTarget.Data.DisappearTimeSec) {
                    continue;
                }

                var pos     = new Vector3(editedTarget.Rect.x + editedTarget.SizeHalf, editedTarget.Rect.y + editedTarget.SizeHalf);
                var moveVec = ToEditorPosition(editedTarget.Data.EndPosition);

                // 移動パターンが直線の場合、目標座標に向かう矢印を描画
                if (editedTarget.Data.MovePatternType == TargetMovePatternType.Liner)
                {
                    var leftArrowPos  = (moveVec + ((Quaternion.Euler(0f, 0f,  45f) * Vector3.Normalize(pos - moveVec)) * 10f));
                    var rightArrowPos = (moveVec + ((Quaternion.Euler(0f, 0f, -45f) * Vector3.Normalize(pos - moveVec)) * 10f));

                    Handles.DrawLine(pos, moveVec, 2f);
                    Handles.DrawLine(moveVec, leftArrowPos, 2f);
                    Handles.DrawLine(moveVec, rightArrowPos, 2f);
                }
                // 回転の場合、目標座標へ向かう円を描画
                else if (editedTarget.Data.MovePatternType == TargetMovePatternType.Rotate)
                {
                    var center = pos + ((moveVec - pos) / 2f);
                    var radius = Vector3.Distance(pos, moveVec) / 2f;

                    Handles.DrawWireDisc(center, Vector3.forward, radius, 2f);
                }

                // クリックした的は最前面に表示するためスキップ
                if (mClickedTarget != null && editedTarget.Data.Id == mClickedTarget.Data.Id)
                {
                    focusedTarget = editedTarget;
                    continue;
                }

                // 的描画
                GUI.DrawTexture(editedTarget.Rect, editedTarget.Texture, ScaleMode.StretchToFill, true, 0);
            }

            // クリックした的を描画
            if (focusedTarget != null)
            {
                GUI.DrawTexture(focusedTarget.Rect, focusedTarget.Texture, ScaleMode.StretchToFill, true, 0);
                GUI.DrawTexture(focusedTarget.Rect, mCursorTexture, ScaleMode.StretchToFill, true, 0);
            }
        }

        /// <summary>
        /// ラベル描画
        /// </summary>
        private void DrawLabel()
        {
            float timeSec = mIsPlaying ? (float)(EditorApplication.timeSinceStartup - mStartTimeSec) : (float)mPauseTimeSec;

            GUI.Label(Label.TimeRect, $"Time : {timeSec.ToString("F2")}");
            GUI.Label(Label.FileNameRect, mOpenedFileName);

            if (!mIsPlaying)
            {
                foreach (var editedTarget in mEditedTargetList)
                {
                    if (mPauseTimeSec >= editedTarget.Data.AppearTimeSec && mPauseTimeSec <= editedTarget.Data.DisappearTimeSec)
                    {
                        GUI.Label(new Rect(editedTarget.Rect.x - 20f, editedTarget.Rect.y + editedTarget.Size, 100f, 15f), $"({editedTarget.Data.Position.x.ToString("F2")}, {editedTarget.Data.Position.y.ToString("F2")})");
                    }
                }
            }
        }

        /// <summary>
        /// マウスイベントのチェック
        /// </summary>
        private void CheckMouseEvent()
        {
            wantsMouseMove = true;

            if (mIsPlaying || HasOpenInstances<ParamEditorWindow>() || string.IsNullOrEmpty(mOpenedFileName)) {
                return;
            }

            var mousePos         = Event.current.mousePosition;
            var targetScreenRect = Tex.TargetScreenRect;

            var isClickInside =
            (
                (mousePos.x >= targetScreenRect.x)
            &&  (mousePos.y >= targetScreenRect.y)
            &&  (mousePos.x <= targetScreenRect.x + targetScreenRect.width)
            &&  (mousePos.y <= targetScreenRect.y + targetScreenRect.height)
            );

            if (isClickInside)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    mClickedTarget       = GetClickedTarget();
                    mIsMovingTarget      = mClickedTarget != null;
                    mClickedTargetOffset = mIsMovingTarget ? mClickedTarget.EditorPosition - mousePos : Vector2.zero;
                    mClickedPos          = mousePos;
                }

                if (Event.current.type == EventType.MouseUp)
                {
                    var distance = Vector2.Distance(mClickedPos, Event.current.mousePosition);

                    if (distance < 0.1f)
                    {
                        OpenParamEditor();
                    }

                    mIsMovingTarget = false;
                }
            }

            if (mIsMovingTarget && Event.current.type == EventType.MouseDrag)
            {
                DragTarget();
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
                if (mIsEdited)
                {
                    // TODO : 保存していないかのチェック
                    if (EditorUtility.DisplayDialog("確認", "内容が保存されていません。\n終了しますか？", "キャンセル", "OK"))
                    {
                        Focus();
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 再生
        /// </summary>
        private void Play()
        {
            if (mIsPlaying || HasOpenInstances<ParamEditorWindow>() || string.IsNullOrEmpty(mOpenedFileName)) {
                return;
            }

            mStartTimeSec = EditorApplication.timeSinceStartup - mPauseTimeSec;
            mIsPlaying    = true;
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        private void Pause()
        {
            if (!mIsPlaying) {
                return;
            }

            mPauseTimeSec = (float)(EditorApplication.timeSinceStartup - mStartTimeSec);
            mIsPlaying    = false;
        }

        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            mPauseTimeSec = 0f;
            mIsPlaying    = false;
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        private void Load()
        {
            if (mIsPlaying || HasOpenInstances<ParamEditorWindow>()) {
                return;
            }

            string directory = Application.dataPath + "/Resources/Json/Scenes/GameScene/StageData";
            string fullPath  = EditorUtility.OpenFilePanelWithFilters("開く", directory, new[]{"テキストファイル", "json" });

            if (string.IsNullOrEmpty(fullPath)) {
                return;
            }

            int pathStartIdx    = Mathf.Max(fullPath.IndexOf("Json/"), 0);
            var assetPath       = fullPath.Substring(pathStartIdx).Replace(".json", string.Empty).Replace("\\", "/");
            var stageDataText   = Resources.Load<TextAsset>(assetPath);
            var stageData       = JsonUtility.FromJson<StageData>(stageDataText?.text);

            if (stageData == null || stageData.StageTargetDataListAsReadOnly.IsNullOrEmpty())
            {
                if (EditorUtility.DisplayDialog("読み込み失敗", "ファイルの読み込みに失敗しました。", "OK"))
                {
                    Focus();
                    return;
                }
            }

            mEditedTargetList = stageData.StageTargetDataList.Select(data => ToEditedTarget(data)).ToList();
            mOpenedFileName   = $"Assets/Resources/{assetPath}.json";

            TargetListWindow.Open(mEditedTargetList, id => SelectTarget(id), id => OpenParamEditor(id), RemoveTarget);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            if (mIsPlaying || !mIsEdited || string.IsNullOrEmpty(mOpenedFileName) || HasOpenInstances<ParamEditorWindow>()) {
                return;
            }

            if (mEditedTargetList.Count <= 0)
            {
                if (EditorUtility.DisplayDialog("保存不可", "的を1つ以上配置してください。", "OK"))
                {
                    Focus();
                    return;
                }
            }

            if (!EditorUtility.DisplayDialog("確認", "保存しますか？", "キャンセル", "OK"))
            {
                mEditedTargetList = mEditedTargetList.OrderBy(target => target.Data.AppearTimeSec).ToList();

                for (int i = 0; i < mEditedTargetList.Count; i++)
                {
                    mEditedTargetList[i].Data.Id = i + 1;
                }

                var stageData = new StageData()
                {
                    StageTargetDataList = mEditedTargetList.Select(target => target.Data).ToArray()
                };

                string json = JsonUtility.ToJson(stageData, prettyPrint : true);

                File.WriteAllText(mOpenedFileName, json);

#if UNITY_EDITOR
                AssetDatabase.ImportAsset(mOpenedFileName);
#endif

                mIsEdited = false;

                if (EditorUtility.DisplayDialog("完了", "保存が完了しました。", "OK"))
                {
                    Focus();
                }
            }
            else
            {
                Focus();
            }
        }

        /// <summary>
        /// 早送り
        /// </summary>
        private void FastForward()
        {
            if (mPauseTimeSec + Common.FastForwardTimeSec > mMaxTimeSec)
            {
                mPauseTimeSec = mMaxTimeSec;
                return;
            }

            mPauseTimeSec = ((int)Math.Round(mPauseTimeSec * 100d) + (int)Math.Round(Common.FastForwardTimeSec * 100d)) / 10 / 10d;
        }

        /// <summary>
        /// 巻き戻し
        /// </summary>
        private void Rewind()
        {
            if (mPauseTimeSec - Common.RewindTimeSec < 0f)
            {
                mPauseTimeSec = 0f;
                return;
            }

            if (Math.Round(mPauseTimeSec * 100d) % 10 == 0d)
            {
                mPauseTimeSec = mPauseTimeSec - Common.RewindTimeSec;
                return;
            }

            mPauseTimeSec = (int)(Math.Round(Math.Round(mPauseTimeSec * 100d)) / 10) / 10d;
        }

        /// <summary>
        /// 的の選択
        /// </summary>
        /// <param name="id"> ID </param>
        private void SelectTarget(int id)
        {
            var editedTarget = mEditedTargetList.FirstOrDefault(data => data.Data.Id == id);
            if (editedTarget == null) {
                return;
            }

            mPauseTimeSec  = editedTarget.Data.AppearTimeSec;
            mClickedTarget = editedTarget;
        }

        /// <summary>
        /// パラメータエディタを開く
        /// </summary>
        private void OpenParamEditor()
        {
            var editedTarget = mClickedTarget ?? CreateNewTarget();

            if (mClickedTarget == null)
            {
                mIsEdited      = true;
                mClickedTarget = editedTarget;

                mEditedTargetList.Add(editedTarget);
            }

            ParamEditorWindow.Open(editedTarget.Data, ApplyEditedParam, RemoveTarget);
        }

        /// <summary>
        /// パラメータエディタを開く
        /// </summary>
        /// <param name="id"> ID </param>
        private void OpenParamEditor(int id)
        {
            var editedTarget = mEditedTargetList.FirstOrDefault(data => data.Data.Id == id);
            if (editedTarget == null) {
                return;
            }

            mPauseTimeSec  = editedTarget.Data.AppearTimeSec;
            mClickedTarget = editedTarget;

            ParamEditorWindow.Open(editedTarget.Data, ApplyEditedParam, RemoveTarget);
        }

        /// <summary>
        /// 編集したパラメータの反映
        /// </summary>
        /// <param name="editedStageTargetData"> 編集したステージの的単体データ </param>
        private void ApplyEditedParam(StageTargetData editedStageTargetData)
        {
            var target = mEditedTargetList.FirstOrDefault(target => target.Data.Id == editedStageTargetData.Id);
            if (target == null) {
                return;
            }

            // 色が変わったらテクスチャを新しく読み込む
            if (target.Data.Color != editedStageTargetData.Color)
            {
                var byteData = GetTargetTextureByteData(editedStageTargetData.Color);

                target.LoadImage(byteData);

                target.Size = GetTargetSize(editedStageTargetData.Color);
            }

            target.Data.Color               = editedStageTargetData.Color;
            target.Data.AppearTimeSec       = (float)Math.Round(editedStageTargetData.AppearTimeSec, 2, MidpointRounding.AwayFromZero);
            target.Data.DisappearTimeSec    = (float)Math.Round(editedStageTargetData.DisappearTimeSec, 2, MidpointRounding.AwayFromZero);
            target.Data.MovePatternType     = editedStageTargetData.MovePatternType;
            target.Data.EndPosition         = editedStageTargetData.EndPosition;
            target.Data.MoveTimeSec         = (float)Math.Round(editedStageTargetData.MoveTimeSec, 2, MidpointRounding.AwayFromZero);

            var editorPos = ToInScreenPos(ToEditorPosition(editedStageTargetData.Position), target.Data.Color);

            target.Rect          = new Rect(editorPos.x - target.SizeHalf, editorPos.y - target.SizeHalf, target.Size, target.Size);
            target.Data.Position = ToGamePosition(editorPos);

            mIsEdited = true;

            TargetListWindow.UpdateEditedTargetList(mEditedTargetList);
        }

        /// <summary>
        /// 的の削除
        /// </summary>
        /// <param name="id"> 削除対象の的の ID </param>
        private void RemoveTarget(int id)
        {
            mEditedTargetList.RemoveAll(target => target.Data.Id == id);

            // ID の振り直し
            for (int i = 0; i < mEditedTargetList.Count; i++)
            {
                mEditedTargetList[i].Data.Id = i + 1;
            }

            mClickedTarget = null;
            mIsEdited      = true;

            TargetListWindow.UpdateEditedTargetList(mEditedTargetList);
        }

        /// <summary>
        /// 的のテクスチャのバイトデータ取得
        /// </summary>
        /// <param name="target">         的    </param>
        /// <param name="targetColor">    色    </param>
        private IReadOnlyList<byte> GetTargetTextureByteData(TargetColor targetColor)
        {
            return targetColor switch
            {
                TargetColor.Red   => mTargetRedTexByteData      ,
                TargetColor.Green => mTargetGreenTexByteData    ,
                TargetColor.Blue  => mTargetBlueTexByteData     ,
                _                 => mTargetRedTexByteData      ,
            };
        }

        /// <summary>
        /// 的のドラッグ
        /// </summary>
        private void DragTarget()
        {
            if (mClickedTarget == null) {
                return;
            }

            var mousePos  = Event.current.mousePosition;
            var editorPos = new Vector2(mousePos.x + mClickedTargetOffset.x, mousePos.y + mClickedTargetOffset.y);

            editorPos = ToInScreenPos(editorPos, mClickedTarget.Data.Color);

            mClickedTarget.Rect          = new Rect(editorPos.x - mClickedTarget.SizeHalf, editorPos.y - mClickedTarget.SizeHalf, mClickedTarget.Size, mClickedTarget.Size);
            mClickedTarget.Data.Position = ToGamePosition(editorPos);

            mIsEdited = true;
        }

        /// <summary>
        /// クリックした的取得
        /// </summary>
        private EditedTarget GetClickedTarget()
        {
            foreach (var editedTarget in mEditedTargetList)
            {
                if (mPauseTimeSec < editedTarget.Data.AppearTimeSec || mPauseTimeSec > editedTarget.Data.DisappearTimeSec) {
                    continue;
                }

                if (Vector2.Distance(Event.current.mousePosition, editedTarget.EditorPosition) <= (editedTarget.SizeHalf))
                {
                    return editedTarget;
                }
            }

            return null;
        }

        /// <summary>
        /// 新規の的を生成
        /// </summary>
        private EditedTarget CreateNewTarget()
        {
            var defColor  = TargetColor.Red;
            var editorPos = ToInScreenPos(Event.current.mousePosition, defColor);
            var gamePos   = ToGamePosition(editorPos);

            var stageTargetData = new StageTargetData()
            {
                Id                  = mEditedTargetList.Count + 1,
                Color               = defColor,
                AppearTimeSec       = (float)Math.Round(mPauseTimeSec, 2, MidpointRounding.AwayFromZero),
                DisappearTimeSec    = (float)Math.Round(mPauseTimeSec + 1d, 2, MidpointRounding.AwayFromZero),
                MovePatternType     = TargetMovePatternType.Stay,
                Position            = gamePos
            };

            var size         = GetTargetSize(defColor);
            var rect         = new Rect(editorPos.x - size / 2f, editorPos.y - size / 2f, size, size);
            var byteDataList = GetTargetTextureByteData(defColor);
            var target       = new EditedTarget(stageTargetData, size, rect, byteDataList);

            return target;
        }

        /// <summary>
        /// 編集用の的に変換
        /// </summary>
        /// <param name="stageTargetData"> ステージの的単体データ </param>
        private EditedTarget ToEditedTarget(StageTargetData stageTargetData)
        {
            var size        = GetTargetSize(stageTargetData.Color);
            var editorPos   = ToEditorPosition(stageTargetData.Position);
            var rect        = new Rect(editorPos.x - size / 2f, editorPos.y - size / 2f, size, size);
            var texByteList = GetTargetTextureByteData(stageTargetData.Color);

            return new EditedTarget(stageTargetData, size, rect, texByteList);
        }

        /// <summary>
        /// 的のサイズ取得
        /// </summary>
        /// <param name="targetColor"> 色 </param>
        private float GetTargetSize(TargetColor targetColor)
        {
            return targetColor switch
            {
                TargetColor.Red   => Tex.TargetRedSize      ,
                TargetColor.Green => Tex.TargetGreenSize    ,
                TargetColor.Blue  => Tex.TargetBlueSize     ,
                _                 => 0f                     ,
            };
        }

        /// <summary>
        /// エディタ上の座標に変換
        /// </summary>
        /// <param name="gamePosition"> ゲーム上の座標 </param>
        private Vector3 ToEditorPosition(Vector3 gamePosition)
        {
            float x = (Window.Size.x / 2f) + (gamePosition.x * Tex.TargetEditorScale);
            float y = (Tex.TargetScreenRect.y + Tex.TargetScreenRect.height / 2f) - (gamePosition.y * Tex.TargetEditorScale);

            return new Vector3(x, y);
        }

        /// <summary>
        /// ゲーム上の座標に変換
        /// </summary>
        /// <param name="editorPosition"> エディタ上の座標 </param>
        private Vector3 ToGamePosition(Vector3 editorPosition)
        {
            float x = (editorPosition.x - (Window.Size.x / 2f)) / Tex.TargetEditorScale;
            float y = (editorPosition.y - (Tex.TargetScreenRect.y + Tex.TargetScreenRect.height / 2f)) / Tex.TargetEditorScale * -1f;

            return new Vector3(x, y);
        }

        /// <summary>
        /// 指定された座標を画面内に収めて返す
        /// </summary>
        /// <param name="editorPos">      エディタ上の座標    </param>
        /// <param name="targetColor">    的の色              </param>
        private Vector3 ToInScreenPos(Vector3 editorPos, TargetColor targetColor)
        {
            var retPos            = editorPos;
            var maxTargetSizeHalf = GetTargetSize(targetColor) / 2f;

            if (retPos.x < Tex.TargetScreenRect.x + maxTargetSizeHalf)
            {
                retPos.x = Tex.TargetScreenRect.x + maxTargetSizeHalf;
            }
            if (retPos.y < Tex.TargetScreenRect.y + maxTargetSizeHalf)
            {
                retPos.y = Tex.TargetScreenRect.y + maxTargetSizeHalf;
            }
            if (retPos.x + maxTargetSizeHalf > Tex.TargetScreenRect.x + Tex.TargetScreenRect.width)
            {
                retPos.x = Tex.TargetScreenRect.x + Tex.TargetScreenRect.width - maxTargetSizeHalf;
            }
            if (retPos.y + maxTargetSizeHalf > Tex.TargetScreenRect.y + Tex.TargetScreenRect.height)
            {
                retPos.y = Tex.TargetScreenRect.y + Tex.TargetScreenRect.height - maxTargetSizeHalf;
            }

            return retPos;
        }
    }
}

