using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  合成ルールを定義するスクリプタブルオブジェクト（弾にこの合成ルールを設定する）
/// </summary>
[CreateAssetMenu(fileName = "CombinationRule", menuName = "Combination/Rule")]
public class CombinationRule : ScriptableObject
{
    [Serializable]
    public struct Combination
    {
        public string elementA; // Aオブジェクト要素
        public string elementB; // Bオブジェクト要素
        public GameObject resultObject; // 合成後のオブジェクト
    }

    public List<Combination> combinations = new List<Combination>();

    /// <summary>
    /// 2つの要素に対応する合成オブジェクトを取得
    /// </summary>
    public GameObject GetCombinationResult(string elementA, string elementB)
    {
        foreach (var combo in combinations)
        {
            if ((combo.elementA == elementA && combo.elementB == elementB) ||
                (combo.elementA == elementB && combo.elementB == elementA))
            {
                return combo.resultObject;
            }
        }
        return null; // 合成できない場合
    }
}