using System.Collections.Generic;
using UnityEngine;


namespace TakahashiH
{
    /// <summary>
    /// プールシステム
    /// </summary>
    public sealed class PoolSystem<T> where T : MonoBehaviour
    {
        //====================================
        //! 変数（private）
        //====================================

        /// <summary>
        /// 未使用要素スタック
        /// </summary>
        private Stack<T> mUnusedElemStack = new Stack<T>();

        /// <summary>
        /// 使用中要素リスト
        /// </summary>
        private List<T> mUsedElemList = new List<T>();


        //====================================
        //! プロパティ
        //====================================

        /// <summary>
        /// 使用中的リスト
        /// </summary>
        public IReadOnlyList<T> UsedElemList => mUsedElemList;


        //====================================
        //! 関数（public）
        //====================================

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="elemList"> 要素リスト </param>
        public void Setup(IReadOnlyList<T> elemList)
        {
            mUnusedElemStack .Clear();
            mUsedElemList    .Clear();

            for (int i = 0; i < elemList.Count; i++)
            {
                var elem = elemList[i];

                mUnusedElemStack.Push(elem);

                elem.SetActive(false);
            }
        }

        /// <summary>
        /// 取り出す
        /// </summary>
        public T Dequeue()
        {
            var elem = mUnusedElemStack.PopOrDefault();
            if (elem == null)
            {
                Debug.LogWarning("要素の取り出しに失敗しました。");
                return null;
            }

            mUsedElemList.Add(elem);

            elem.SetActive(true);

            return elem;
        }

        /// <summary>
        /// 戻す
        /// </summary>
        /// <param name="elem"> 要素 </param>
        public void Enqueue(T elem)
        {
            elem.SetActive(false);

            mUsedElemList.Remove(elem);

            mUnusedElemStack.Push(elem);
        }
    }
}

