using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;


namespace TakahashiH
{
    /// <summary>
    /// エディタ用ショートカットキー登録エディタ拡張
    /// </summary>
    public static class EditorShortcutKey
    {
        [Shortcut("TakahashiH/Delete", KeyCode.Delete)]
        private static void Delete()
        {
            EditorApplication.ExecuteMenuItem("Edit/Delete");
        }
    }
}
