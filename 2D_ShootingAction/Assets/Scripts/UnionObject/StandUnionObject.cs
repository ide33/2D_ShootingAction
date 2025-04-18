using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 足場オブジェクトのクラス
/// </summary>
public class StandUnionObject : MonoBehaviour, IUnionObject
{
    [SerializeField] private UnionData UnionData;        // プロパティに設定するデータ
    public UnionData unionData { get; set; }             // 継承プロパティ

    // 内部処理する変数
    private Rigidbody2D _rigidbody2D;       // 移動用？
    private SpriteRenderer _spriteRenderer; // 向き確認用
    // private Player_Move currentPlayer;      // プレイヤーの情報を保持する変数

    void Start()
    {
        // プロパティにデータを設定
        if (unionData == null)
        {
            unionData = new UnionData();
        }
        unionData.unionDataNeme = UnionData.unionDataNeme;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // =========================================================
    // プレイヤー足場判定接触処理
    // =========================================================
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         currentPlayer = collision.GetComponent<Player_Move>(); // プレイヤーの情報を取得

    //         if (currentPlayer != null && !currentPlayer.AirJudge()) 
    //         {
    //             currentPlayer.RideMoveSpeed = _rigidbody2D.linearVelocity; // 足場の速度をプレイヤーに設定
    //         }
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         if (currentPlayer != null)
    //         {
    //             currentPlayer.RideMoveSpeed = Vector3.zero; // 足場から降りたとき速度をリセット
    //             currentPlayer = null;
    //         }
    //     }
    // }
}
