using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動し壁を上る足場オブジェクトのクラス
/// </summary>
public class FloatingUnionObject : MonoBehaviour, IUnionObject
{
    [SerializeField] private UnionData UnionData;         // プロパティに設定するデータ
    public UnionData unionData { get; set; }              // 継承プロパティ

    // インスペクター上で設定する変数
    [Header("移動設定")]
    [SerializeField] private float SideMoveSpeed = 4f;    // 左右移動速度
    [SerializeField] private float WallClimbSpeed = 3f;   // 壁を登る速度
    [SerializeField] private float WallDistance = 0.2f;   // 登る時の壁との距離
    [SerializeField] private LayerMask WallMask;          // 壁とするレイヤー


    // 内部処理する変数
    private Rigidbody2D _rigidbody2D;       // 移動用
    private SpriteRenderer _spriteRenderer; // 向き確認用
    private int direction;                  // 移動する向き
    private bool previousIsWall = false;    // 現在の壁接触判定
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

        // 移動する向きをSpriteRendererの向きから取得
        direction = _spriteRenderer.flipX ? -1 : 1;
    }

    void Update()
    {
        bool currentIsWall = WallChack(); // 現在の壁接触判定

        // isWallの状態が切り替わったときに一度だけリセット処理
        if (currentIsWall != previousIsWall)
        {
            ResetVelocity();
        }

        // 壁に触れていなければ
        if (!currentIsWall)
        {
            // 横方向に移動
            SideMove();
        }
        // 壁に触れていれば
        else
        {
            // 上方向に移動
            WallCliming();
        }

        // 状態更新
        previousIsWall = currentIsWall;
    }

    // ===============================================
    // 移動処理
    // ===============================================

    // 前方壁チェック
    private bool WallChack()
    {
        bool _isWall;
        float rayLength = WallDistance;
        var layerMask = WallMask;

        // コライダーを取得
        Collider2D col = GetComponent<Collider2D>();

        // 向きに応じてコライダーの移動方向の最下を取得
        float colDirection = direction >= 1 ? col.bounds.max.x : col.bounds.min.x;

        // コライダーの移動方向の最下の位置を取得
        Vector2 bottomPosition = new Vector2(colDirection, col.bounds.min.y + 0.1f);

        // 向きに応じた方向にレイを発射して、すぐ先に壁があるか判定
        _isWall = Physics2D.Raycast(bottomPosition, direction >= 1 ? Vector2.right : Vector2.left, rayLength, layerMask);

        // シーン上でレイを表示（緑で表示、判定取得で赤で表示）
        Debug.DrawRay(bottomPosition, direction >= 1 ? Vector2.right * rayLength : Vector2.left * rayLength, _isWall ? Color.green : Color.red);

        return _isWall;
    }

    // 水平方向に移動する
    private void SideMove()
    {
        if (_rigidbody2D == null) return; // nullチェック

        // 横方向の速度を変更する
        _rigidbody2D.linearVelocity = new Vector2(SideMoveSpeed * direction, _rigidbody2D.linearVelocity.y);
    }

    // 垂直に壁を上る
    private void WallCliming()
    {
        if (_rigidbody2D == null) return; // nullチェック

        // 上方向に速度を変更する
        _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, WallClimbSpeed);
    }

    // 速度をリセット
    private void ResetVelocity()
    {
        if (_rigidbody2D == null) return; // nullチェック

        // 速度を0（移動しない）
        _rigidbody2D.linearVelocity = Vector2.zero;
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
    //             currentPlayer.RideMoveSpeed = _rigidbody2D.velocity; // 足場の速度をプレイヤーに設定

    //             // 壁に触れているかどうかを判定
    //             if (WallChack() != previousIsWall)
    //             {
    //                 currentPlayer.ResetVelocity(); // 壁に触れているときはプレイヤーの速度をリセット
    //             }
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