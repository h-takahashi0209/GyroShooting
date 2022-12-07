using UnityEngine;


namespace TakahashiH.GyroShooting.Tools.StageEditor.StageEditorDef
{
    /// <summary>
    /// 共通定義
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 早送り1回ごとに進める時間（秒）
        /// </summary>
        public static double FastForwardTimeSec = 0.1f;

        /// <summary>
        /// 巻き戻し1回ごとに戻す時間（秒）
        /// </summary>
        public static double RewindTimeSec = 0.1f;
    }

    /// <summary>
    /// ウインドウ
    /// </summary>
    public static class Window
    {
        /// <summary>
        /// サイズ
        /// </summary>
        public static Vector2 Size = new Vector2(600f, 500f);
    }

    /// <summary>
    /// ボタン
    /// </summary>
    public static class Button
    {
        /// <summary>
        /// Y座標
        /// </summary>
        private static float PositionY = 455f;

        /// <summary>
        /// 縦幅
        /// </summary>
        private static float Height = 30f;

        /// <summary>
        /// 再生・一時停止・停止ボタン横幅
        /// </summary>
        private static float PlayPauseStopWidth = 30f;

        /// <summary>
        /// 読み込み・保存ボタン横幅
        /// </summary>
        private static float SaveAndLoadWidth = 170f;

        /// <summary>
        /// 巻き戻しボタン Rect
        /// </summary>
        public static Rect RewindButton = new Rect(25f, PositionY, PlayPauseStopWidth, Height);

        /// <summary>
        /// 再生ボタン Rect
        /// </summary>
        public static Rect PlayRect = new Rect(65f, PositionY, PlayPauseStopWidth, Height);

        /// <summary>
        /// 早送りボタン Rect
        /// </summary>
        public static Rect FastForwardButton = new Rect(105f, PositionY, PlayPauseStopWidth, Height);

        /// <summary>
        /// 一時停止ボタン Rect
        /// </summary>
        public static Rect PauseRect = new Rect(145f, PositionY, PlayPauseStopWidth, Height);

        /// <summary>
        /// 停止ボタン Rect
        /// </summary>
        public static Rect StopRect = new Rect(185f, PositionY, PlayPauseStopWidth, Height);

        /// <summary>
        /// 読み込みボタン Rect
        /// </summary>
        public static Rect LoadRect = new Rect(225f, PositionY, SaveAndLoadWidth, Height);

        /// <summary>
        /// 保存ボタン Rect
        /// </summary>
        public static Rect SaveRect = new Rect(405f, PositionY, SaveAndLoadWidth, Height);
    }

    /// <summary>
    /// スライダー
    /// </summary>
    public static class Slider
    {
        /// <summary>
        /// シークバー Rect
        /// </summary>
        public static Rect SeekBarRect = new Rect(25f, 410f, 550f, 30f);
    }

    /// <summary>
    /// テクスチャ
    /// </summary>
    public static class Tex
    {
        /// <summary>
        /// ルートパス
        /// </summary>
        private static string RootPath = $"{Application.dataPath}/Resources/Editor/Textures/StageEditor/";

        /// <summary>
        /// ボタンのルートパス
        /// </summary>
        private static string ButtonRootPath = $"{RootPath}/Button";

        /// <summary>
        /// 巻き戻しボタンパス
        /// </summary>
        public static string RewindButtonPath = $"{ButtonRootPath}/RewindButton.png";

        /// <summary>
        /// 再生ボタンパス
        /// </summary>
        public static string PlayButtonPath = $"{ButtonRootPath}/PlayButton.png";

        /// <summary>
        /// 早送りボタンパス
        /// </summary>
        public static string FastForwardButtonPath = $"{ButtonRootPath}/FastForwardButton.png";

        /// <summary>
        /// 一時停止ボタンパス
        /// </summary>
        public static string PauseButtonPath = $"{ButtonRootPath}/PauseButton.png";

        /// <summary>
        /// 停止ボタンパス
        /// </summary>
        public static string StopButtonPath = $"{ButtonRootPath}/StopButton.png";

        /// <summary>
        /// 的のルートパス
        /// </summary>
        private static string TargetRootPath = $"{RootPath}/Target/";

        /// <summary>
        /// 赤の的パス
        /// </summary>
        public static string TargetRedPath = $"{TargetRootPath}/EditorTargetRed.png";

        /// <summary>
        /// 緑の的パス
        /// </summary>
        public static string TargetGreenPath = $"{TargetRootPath}/EditorTargetGreen.png";

        /// <summary>
        /// 青の的パス
        /// </summary>
        public static string TargetBluePath = $"{TargetRootPath}/EditorTargetBlue.png";

        /// <summary>
        /// カーソルパス
        /// </summary>
        public static string CursorPath = $"{TargetRootPath}/Cursor.png";

        /// <summary>
        /// 的画面 Rect
        /// </summary>
        public static Rect TargetScreenRect = new Rect(50f, 45f, 500f, 328f);

        /// <summary>
        /// 赤の的サイズ
        /// </summary>
        public static float TargetRedSize = 45f;

        /// <summary>
        /// 緑の的サイズ
        /// </summary>
        public static float TargetGreenSize = 40f;

        /// <summary>
        /// 青の的サイズ
        /// </summary>
        public static float TargetBlueSize = 35f;

        /// <summary>
        /// エディタで表示する際のスケール
        /// </summary>
        public static float TargetEditorScale = 8.6f;
    }

    /// <summary>
    /// ラベル
    /// </summary>
    public static class Label
    {
        /// <summary>
        /// 時間 Rect
        /// </summary>
        public static Rect TimeRect = new Rect(250f, 390f, 100f, 20f);

        /// <summary>
        /// ファイル名 Rect
        /// </summary>
        public static Rect FileNameRect = new Rect(50f, 25f, 500f, 20f);

        /// <summary>
        /// ファイル名初期文字列
        /// </summary>
        public static string DefFileNameText = "ステージファイルを読み込んでください。";
    }
}

