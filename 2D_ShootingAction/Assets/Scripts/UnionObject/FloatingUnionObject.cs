using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動足場オブジェクトのクラス
/// </summary>
/*
    * このクラスは、合成オブジェクトの移動し壁を上る足場を表します。
    * 移動足場は、前方に移動し、壁に触れると上に移動します。
*/
public class FloatingUnionObject : BaseUnionObject
{
    // インスペクター上で設定する変数
    [SerializeField] private UnionData FloatingUnionData;         // プロパティに設定するデータ

    [Header("移動設定")]
    [SerializeField] private float SideMoveSpeed = 4f;    // 左右移動速度
    [SerializeField] private float WallClimbSpeed = 3f;   // 壁を登る速度
    [SerializeField] private float WallDistance = 0.2f;   // 登る時の壁との距離
    [SerializeField] private LayerMask WallMask;          // 壁とするレイヤー
    [SerializeField] private float WallCheckDistance = 0.1f; // 壁チェックの距離

    [Header("消滅時間")]
    [SerializeField] private float DestroyTime = 5f;      // 消滅時間


    // 内部処理する変数
    private Rigidbody2D _rigidbody2D;       // 移動用
    private SpriteRenderer _spriteRenderer; // 向き確認用
    private int direction;                  // 移動する向き
    private Collider2D _collider;
    private BoxCollider2D _boxCollider;
    private bool _isBoxCollider;
    private bool previousIsWall = false;    // 現在の壁接触判定
    // private Player_Move currentPlayer;      // プレイヤーの情報を保持する変数

    protected override void Start()
    {
        base.Start(); // BaseUnionObjectのStartを呼び出す

        // プロパティにデータを設定
        if (unionData == null)
        {
            unionData = new UnionData();
        }
        unionData.unionDataNeme = FloatingUnionData.unionDataNeme;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // 移動する向きをSpriteRendererの向きから取得
        direction = _spriteRenderer.flipX ? -1 : 1;

        // コライダー取得・判別
        _collider = GetComponent<Collider2D>();
        _boxCollider = _collider as BoxCollider2D;
        _isBoxCollider = _boxCollider != null;

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
        Vector2 bottomPosition;

        if (_isBoxCollider)
        {
            // BoxCollider2Dの場合はoffsetとsizeを考慮して端の下側を計算
            Vector2 boxCenter = (Vector2)_boxCollider.transform.position + _boxCollider.offset;
            float x = direction >= 1
                ? boxCenter.x + (_boxCollider.size.x * 0.5f * _boxCollider.transform.lossyScale.x)
                : boxCenter.x - (_boxCollider.size.x * 0.5f * _boxCollider.transform.lossyScale.x);
            float y = boxCenter.y - (_boxCollider.size.y * 0.5f * _boxCollider.transform.lossyScale.y) + WallCheckDistance;
            bottomPosition = new Vector2(x, y);
            Debug.Log($"BoxCollider2Dの位置: {bottomPosition}");
        }
        else
        {
            // 他のコライダーの場合
            float x = direction >= 1 ? _collider.bounds.max.x : _collider.bounds.min.x;
            float y = _collider.bounds.min.y + WallCheckDistance;
            bottomPosition = new Vector2(x, y);
        }

        // 向きに応じた方向にレイを発射して、すぐ先に壁があるか判定
        RaycastHit2D hit = Physics2D.Raycast(bottomPosition, direction >= 1 ? Vector2.right : Vector2.left, rayLength, layerMask);

        // ヒットした場合、自分自身のコライダーでなければtrue
        _isWall = hit.collider != null && hit.collider != _collider;

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