using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物のスクリプト
/// </summary>
/* 
    * このクラスは、合成オブジェクトの基本クラスです。
    * 合成オブジェクトは、プレイヤーが乗ることができる足場や、移動する足場など
    * 弾から生成できるオブジェクトを表します。
    * 
    * - unionData: オブジェクトの要素
    * - OnObjectCreate: 生成時の処理
    * - OnObjectDestroy: 消去時の処理
*/
public interface IUnionObject
{
    UnionData unionData { set; get; } // オブジェクトの要素
    void OnObjectCreate(); // 生成時の処理
    void OnObjectDestroy(); // 消去時の処理
}