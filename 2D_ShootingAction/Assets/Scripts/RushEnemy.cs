using UnityEditor.Tilemaps;
using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    [SerializeField] private GameObject targetPlayer;  // プレイヤーのオブジェクト
    [SerializeField] private float rushDistance = 2f;  // 突進を開始する距離
    [SerializeField] private float moveTime = 5f;  // 歩く時間
    [SerializeField] private float speed = 2f;  // 移動速度
    [SerializeField] private float rushSpeed = 4f;  // 突進時の移動速度
    [SerializeField] private float rushDuration = 2f;  // 突進する時間

    private float startMove;  // 移動開始時間
    private float rushStartTime;
    private int direction = 1;  // 1:右へ移動, -1:左へ移動
    private bool isRushing = false;  // 突進中かどうか
    private Rigidbody2D rb;  // Rigidbodyの変数
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2Dを取得
        startMove = Time.time;  // ゲーム開始時の時間
    }

    void Update()
    {
        Vector2 mypos = transform.position;  // 自分の位置を取得
        Vector2 playerpos = targetPlayer.transform.position;  // プレイヤーの位置を取得

        // プレイヤーとの距離を計算
        float distance = Vector2.Distance(mypos, playerpos);

        // プレイヤーの方向を取得
        float directionToPlayer = Mathf.Sign(playerpos.x - mypos.x);  // -1(左) or 1(右)

        // プレイヤーが近かったら
        if (distance < rushDistance && !isRushing)
        {
            Debug.Log("突進");

            // プレイヤーの方向を向く処理
            if (directionToPlayer != Mathf.Sign(transform.localScale.x))
            {
                Debug.Log("振り向き");
                Flip();
            }

            speed += rushSpeed;  // 突進速度を適用
            isRushing = true;  // 突進中
            rushStartTime = Time.time;  // 突進開始時間を記録
        }

        // 突進を一定時間後に終了
        if (isRushing && Time.time - rushStartTime >= rushDuration)
        {
            Debug.Log("突進終了");
            speed -= rushSpeed;  // 速度を元に戻す
            isRushing = false;   // 突進状態をリセット
        }

        PatorolMovement();
    }

    void PatorolMovement()
    {
        // 一定時間経過したら方向転換
        if (Time.time - startMove >= moveTime && !isRushing)
        {
            direction *= -1;  // 方向転換
            startMove = Time.time;  // 移動開始時間を更新
        }

        // 現在の方向に移動
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    // 方向転換メソッド
    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        direction *= -1;  // 移動方向も反転
    }
}
