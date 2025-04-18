using UnityEngine;

/// <summary>
/// 合成する物体のデータ（名前）を設定できるデータクラス
/// </summary>
[System.Serializable]
public class UnionData
{
    public string unionDataNeme; // 要素名（例: "Stand", "Floting" など）
}


/*
　　メモ書き：現在の弾オブジェクトには地面接触でオブジェクト化の実装は無い。
　　　　　　　StandEntityでオブジェクト生成時に重なりを防止する仕組みを実装する。
　　　　　　　敵に攻撃を行う処理も実装する。
　　　　　　　→弾のスクリプトに移動処理を実装する。

　　　　　　　CannonBulletShotStructureとFloatingBulletShotStructureに
　　　　　　　BulletShotStructureを継承する。
　　　　　　　→コルーチン使用不可能。移動処理は弾スクリプトに処理を移す。

　　　　　　　CombinationRuleの処理の合成ルールを実装する。
　　　　　　　弾の合成ルールは、弾のスクリプタブルオブジェクト変数に設定する。
　　　　　　　→合成時の生成は１つのコライダーを持ったオブジェクトを想定して重なり防止を実装する。
*/