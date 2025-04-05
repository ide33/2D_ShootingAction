using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    [SerializeField] private GameObject targetPlayer;  // プレイヤーのオブジェクト
    [SerializeField] private float rushDistance = 2f;  // 突進を開始する距離
    [SerializeField] private float patrolTime = 5f;  // 歩く時間
    [SerializeField] private float normalSpeed = 2f;  // 移動速度
    [SerializeField] private float rushSpeed = 4f;  // 突進時の移動速度
    [SerializeField] private float rushDuration = 2f;  // 突進する時間

    private float patrolStartTime;  // 移動開始時間
    private float rushStartTime;  // 突進開始時間
    private int direction = 1;  // 1:右へ移動, -1:左へ移動
    private bool isRushing = false;  // 突進中かどうか
    private bool isStopped = false;  // 停止中かどうか

    private Rigidbody2D rb;  // Rigidbodyの変数

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2Dを取得
        patrolStartTime = Time.time;  // ゲーム開始時の時間
    }

    void Update()
    {
        Vector2 mypos = transform.position;  // 自分の位置を取得
        Vector2 playerpos = targetPlayer.transform.position;  // プレイヤーの位置を取得

        // プレイヤーとの距離を計算
        float distance = Vector2.Distance(mypos, playerpos);

        // プレイヤーの方向を取得
        float directionToPlayer = Mathf.Sign(playerpos.x - mypos.x);  // -1(左) or 1(右)

        if (isRushing)
        {
            if (Time.time - rushStartTime >= rushDuration)
            {
                Debug.Log("突進終了");
                isRushing = false;   // 突進状態をリセット
                patrolStartTime = Time.time;  // パトロール再開の時間を更新
                StartCoroutine(StopForSeconds(1f));  // 1秒停止
            }

            Move();
            return;
        }

        // プレイヤーが近かったら
        if (distance < rushDistance && !isRushing)
        {
            Debug.Log("突進");

            // 振り向き
            if ((directionToPlayer > 0 && transform.localScale.x < 0) || (directionToPlayer < 0 && transform.localScale.x > 0))
            {
                Flip();
            }

            Debug.Log("突進開始");
            isRushing = true;  // 突進中
            rushStartTime = Time.time;  // 突進開始時間を記録
        }

        // // 突進を一定時間後に終了
        // if (isRushing && Time.time - rushStartTime >= rushDuration)
        // {
        //     Debug.Log("突進終了");
        //     isRushing = false;   // 突進状態をリセット
        //     patrolStartTime = Time.time;  // パトロール再開の時間を更新
        //     StartCoroutine(StopForSeconds(1f));  // 1秒停止
        // }

        Move();
    }

    void Move()
    {
        if (isStopped)
        {
            Debug.Log("敵が停止しました");
            rb.linearVelocity = Vector2.zero;
            return;  // 停止中は移動しない
        }

        float currentSpeed = isRushing ? rushSpeed : normalSpeed;

        // パトロール時のみ方向転換
        if (!isRushing && Time.time - patrolStartTime > patrolTime)
        {
            Flip();
            patrolStartTime = Time.time;
        }

        // 現在の方向に移動
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
    }

    // 壁にぶつかったとき
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 突進中は停止しない
            if (!isRushing)
            {
                Debug.Log("壁にぶつかった 停止");
                StartCoroutine(StopForSeconds(1f));  // 1秒停止
            }
        }
    }

    // 方向転換メソッド
    void Flip()
    {
        // スプライトの向きと移動方向を反転
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        direction *= -1;  // 移動方向を反転
    }

    // 一定時間停止
    private IEnumerator StopForSeconds(float duration)
    {
        Debug.Log("敵が停止しました");
        isStopped = true;  // 一時停止
        rb.linearVelocity = Vector2.zero;  // 速度を0
        yield return new WaitForSeconds(duration);  // 時間経過を待つ
        isStopped = false;  // 停止終了
    }
}
