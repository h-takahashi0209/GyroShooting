using UnityEngine;


namespace TakahashiH.GyroShooting.Tools.StageEditor.ParamEditorDef
{
    /// <summary>
    /// ウインドウ
    /// </summary>
    public static class Window
    {
        /// <summary>
        /// サイズ
        /// </summary>
        public static Vector2 Size = new Vector2(310f, 214f);
    }

    /// <summary>
    /// ボタン
    /// </summary>
    public static class Button
    {
        /// <summary>
        /// Y座標
        /// </summary>
        private static float PositionY = 189f;

        /// <summary>
        /// 横幅
        /// </summary>
        private static float Width = 90f;

        /// <summary>
        /// 縦幅
        /// </summary>
        private static float Height = 20f;

        /// <summary>
        /// キャンセルボタン Rect
        /// </summary>
        public static Rect CancelRect = new Rect(10f, PositionY, Width, Height);

        /// <summary>
        /// 削除ボタン Rect
        /// </summary>
        public static Rect DeleteRect = new Rect(110f, PositionY, Width, Height);

        /// <summary>
        /// 決定ボタン Rect
        /// </summary>
        public static Rect DecideRect = new Rect(210f, PositionY, Width, Height);
    }

    /// <summary>
    /// ラベル
    /// </summary>
    public static class Label
    {
        /// <summary>
        /// X座標
        /// </summary>
        private static float PositionX = 10f;

        /// <summary>
        /// 出現座標Y座標
        /// </summary>
        private static float PositionPositionY = 115f;

        /// <summary>
        /// 目標座標Y座標
        /// </summary>
        private static float EndPositionPositionY = 137f;

        /// <summary>
        /// 横幅
        /// </summary>
        private static float Wifth = 100f;

        /// <summary>
        /// 横幅小
        /// </summary>
        private static float WifthMin = 20f;

        /// <summary>
        /// 縦幅
        /// </summary>
        private static float Height = 20f;

        /// <summary>
        /// ID Rect
        /// </summary>
        public static Rect IdRect = new Rect(PositionX, 5f, Wifth, Height);

        /// <summary>
        /// 色 Rect
        /// </summary>
        public static Rect ColorRect = new Rect(PositionX, 27f, Wifth, Height);

        /// <summary>
        /// 出現時間 Rect
        /// </summary>
        public static Rect AppearTimeRect = new Rect(PositionX, 49f, Wifth, Height);

        /// <summary>
        /// 消滅時間 Rect
        /// </summary>
        public static Rect DisappearTimeRect = new Rect(PositionX, 71f, Wifth, Height);

        /// <summary>
        /// 移動パターン Rect
        /// </summary>
        public static Rect MovePatternRect = new Rect(PositionX, 93f, Wifth, Height);

        /// <summary>
        /// 出現座標 Rect
        /// </summary>
        public static Rect PositionRect = new Rect(PositionX, 115f, Wifth, Height);

        /// <summary>
        /// 出現座標X Rect
        /// </summary>
        public static Rect PositionXRect = new Rect(104f, PositionPositionY, WifthMin, Height);

        /// <summary>
        /// 出現座標Y Rect
        /// </summary>
        public static Rect PositionYRect = new Rect(171f, PositionPositionY, WifthMin, Height);

        /// <summary>
        /// 出現座標Z Rect
        /// </summary>
        public static Rect PositionZRect = new Rect(238f, PositionPositionY, WifthMin, Height);

        /// <summary>
        /// 目標座標 Rect
        /// </summary>
        public static Rect EndPositionRect = new Rect(PositionX, EndPositionPositionY, Wifth, Height);

        /// <summary>
        /// 目標座標X Rect
        /// </summary>
        public static Rect EndPositionXRect = new Rect(104f, EndPositionPositionY, WifthMin, Height);

        /// <summary>
        /// 目標座標Y Rect
        /// </summary>
        public static Rect EndPositionYRect = new Rect(171f, EndPositionPositionY, WifthMin, Height);

        /// <summary>
        /// 目標座標Z Rect
        /// </summary>
        public static Rect EndPositionZRect = new Rect(238f, EndPositionPositionY, WifthMin, Height);

        /// <summary>
        /// 移動時間 Rect
        /// </summary>
        public static Rect MoveTimeRect = new Rect(PositionX, 159f, Wifth, Height);
    }

    /// <summary>
    /// テキストフィールド
    /// </summary>
    public static class TextField
    {
        /// <summary>
        /// X座標
        /// </summary>
        private static float PositionX = 100f;

        /// <summary>
        /// X X座標
        /// </summary>
        private static float XPositionX = 121f;

        /// <summary>
        /// Y X座標
        /// </summary>
        private static float YPositionX = 188f;

        /// <summary>
        /// Z X座標
        /// </summary>
        private static float ZPositionX = 255f;

        /// <summary>
        /// 出現座標Y座標
        /// </summary>
        private static float PositionPositionY = 115f;

        /// <summary>
        /// 目標座標Y座標
        /// </summary>
        private static float EndPositionPositionY = 137f;

        /// <summary>
        /// 横幅
        /// </summary>
        private static float Wifth = 200f;

        /// <summary>
        /// 横幅小
        /// </summary>
        private static float WifthMin = 45f;

        /// <summary>
        /// 縦幅
        /// </summary>
        private static float Height = 20f;

        /// <summary>
        /// 出現時間 Rect
        /// </summary>
        public static Rect AppearTimeRect = new Rect(PositionX, 49f, Wifth, Height);

        /// <summary>
        /// 消滅時間 Rect
        /// </summary>
        public static Rect DisappearTimeRect = new Rect(PositionX, 71f, Wifth, Height);

        /// <summary>
        /// 出現座標X Rect
        /// </summary>
        public static Rect PositionXRect = new Rect(XPositionX, PositionPositionY, WifthMin, Height);

        /// <summary>
        /// 出現座標Y Rect
        /// </summary>
        public static Rect PositionYRect = new Rect(YPositionX, PositionPositionY, WifthMin, Height);

        /// <summary>
        /// 出現座標Z Rect
        /// </summary>
        public static Rect PositionZRect = new Rect(ZPositionX, PositionPositionY, WifthMin, Height);

        /// <summary>
        /// 目標座標X Rect
        /// </summary>
        public static Rect EndPositionXRect = new Rect(XPositionX, EndPositionPositionY, WifthMin, Height);

        /// <summary>
        /// 目標座標Y Rect
        /// </summary>
        public static Rect EndPositionYRect = new Rect(YPositionX, EndPositionPositionY, WifthMin, Height);

        /// <summary>
        /// 目標座標Z Rect
        /// </summary>
        public static Rect EndPositionZRect = new Rect(ZPositionX, EndPositionPositionY, WifthMin, Height);

        /// <summary>
        /// 移動時間 Rect
        /// </summary>
        public static Rect MoveTimeRect = new Rect(PositionX, 159f, Wifth, Height);
    }

    /// <summary>
    /// トグル
    /// </summary>
    public static class Toggle
    {
        /// <summary>
        /// 1項目目X座標
        /// </summary>
        private static float FirstPositionX = 121f;

        /// <summary>
        /// 2項目目X座標
        /// </summary>
        private static float SecondPositionX = 188f;

        /// <summary>
        /// 3項目目X座標
        /// </summary>
        private static float ThirdPositionX = 255f;

        /// <summary>
        /// 色Y座標
        /// </summary>
        private static float ColorPositionY = 27f;

        /// <summary>
        /// 移動パターンY座標
        /// </summary>
        private static float MovePatternPositionY = 93f;

        /// <summary>
        /// 横幅
        /// </summary>
        private static float Wifth = 50f;

        /// <summary>
        /// 縦幅
        /// </summary>
        private static float Height = 20f;

        /// <summary>
        /// 赤色 Rect
        /// </summary>
        public static Rect ColorRedRect = new Rect(FirstPositionX, ColorPositionY, Wifth, Height);

        /// <summary>
        /// 緑色 Rect
        /// </summary>
        public static Rect ColorGreenRect = new Rect(SecondPositionX, ColorPositionY, Wifth, Height);

        /// <summary>
        /// 青色 Rect
        /// </summary>
        public static Rect ColorBlueRect = new Rect(ThirdPositionX, ColorPositionY, Wifth, Height);

        /// <summary>
        /// 移動パターン待機 Rect
        /// </summary>
        public static Rect MovePatternWaitRect = new Rect(FirstPositionX, MovePatternPositionY, Wifth, Height);

        /// <summary>
        /// 移動パターン移動 Rect
        /// </summary>
        public static Rect MovePatternMoveRect = new Rect(SecondPositionX, MovePatternPositionY, Wifth, Height);

        /// <summary>
        /// 移動パターン回転 Rect
        /// </summary>
        public static Rect MovePatternRotateRect = new Rect(ThirdPositionX, MovePatternPositionY, Wifth, Height);
    }
}
