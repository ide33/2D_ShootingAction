using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 足場オブジェクトのクラス
/// </summary>
/*
    * このクラスは、合成オブジェクトの足場を表します。
    * 足場は、プレイヤーが乗ることができます。
*/
public class StandUnionObject : BaseUnionObject
{
    // インスペクター上で設定する変数
    [SerializeField] private UnionData StandUnionData;    // プロパティに設定するデータ

    [Header("消滅時間")]
    [SerializeField] private float DestroyTime = 5f;      // 消滅時間

    // 内部処理する変数
    private Rigidbody2D _rigidbody2D;       // 移動用？
    private SpriteRenderer _spriteRenderer; // 向き確認用
    // private Player_Move currentPlayer;      // プレイヤーの情報を保持する変数

    protected override void Start()
    {
        base.Start(); // BaseUnionObjectのStartを呼び出す
        
        // プロパティにデータを設定
        if (unionData == null)
        {
            unionData = new UnionData();
        }
        unionData.unionDataNeme = StandUnionData.unionDataNeme;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, DestroyTime); // 指定時間後にオブジェクトを削除
    }
    public override void OnObjectCreate()
    {
        base.OnObjectCreate();
    }   
    public override void OnObjectDestroy()
    {
        base.OnObjectDestroy();
    }
}
