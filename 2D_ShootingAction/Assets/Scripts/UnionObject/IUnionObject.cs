using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物のスクリプト
/// </summary>
public interface IUnionObject
{
    UnionData unionData { set; get; } // オブジェクトの要素
}