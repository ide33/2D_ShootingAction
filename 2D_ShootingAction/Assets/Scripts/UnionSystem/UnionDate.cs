using UnityEngine;

/// <summary>
/// 合成する物体のデータ（名前）を設定できるデータクラス
/// </summary>
/*
    * このクラスは、合成する物体のデータを設定するためのクラスです。
    * 合成する物体の名前を設定することができます。
    * 
    * - unionDataNeme: 要素名（例: "Stand", "Floting" など）
*/
[System.Serializable]
public class UnionData
{
    public string unionDataNeme; // 要素名（例: "Stand", "Floting" など）
}