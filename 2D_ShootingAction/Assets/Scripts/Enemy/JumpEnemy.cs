using Unity.VisualScripting;
using UnityEngine;

public class JumpEnemy : MonoBehaviour, IDamageable
{
    private enum Jp_State
    {
        Jump
    }

    [SerializeField] private float jumpForce = 8f;  // 上方向の力
    [SerializeField] private float sideForce = 0.5f;  // 横方向の力
    [SerializeField] private float groundCheckDistance = 0.1f;  // 地面判定の長さ
    [SerializeField] private int maxHealth = 2;  // 最大Hp
    [SerializeField] private int damageToPlayer = 1;  // プレイヤーに与えるダメージ
    [SerializeField] private Transform groundCheck;  // 接地判定の位置
    [SerializeField] private LayerMask groundLayer; // 地面レイヤーの指定

    private float jumpCooldown = 1f;  // ジャンプの間隔
    private float jumpTimer = 0f;  // タイマー管理
    private float currentjumpForce;  // 現在の上方向への力
    private float currentsideForce;  // 現在の横方向への力
    private int currentHealth;  // 現在のHp
    private int direction = -1;  // 1:右へ移動, -1:左へ移動
    private bool isWallhit;  // 壁に当たったかどうか

    private Rigidbody2D rb;  // Rigidbodyの変数

    private Jp_State currentState = Jp_State.Jump;  // 初期状態はjump

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2Dを取得
        currentHealth = maxHealth;  // Hpを初期化
    }

    void Update()
    {
        switch (currentState)
        {
            case Jp_State.Jump:
                currentjumpForce = jumpForce;
                currentsideForce = sideForce;

                // jump状態の処理
                StateJump();
                break;
        }
    }

    private void StateJump()
    {
        // jumpTimerに前フレームからの経過時間を足す
        jumpTimer += Time.deltaTime;

        // 地面にいて且つjumpTimerがクールダウンの値より小さければジャンプ
        if (isGrounded() && jumpTimer >= jumpCooldown)
        {
            Jumping();
            jumpTimer = 0f;
        }

        // 壁にぶつかったら
        if (isWallhit)
        {
            isWallhit = false;  // フラグをリセット
            Flip();
        }
    }

    private void Jumping()
    {
        // 現在の速度をリセット
        rb.linearVelocity = Vector2.zero;

        // 現在の方向にジャンプ
        rb.AddForce(new Vector2(direction * currentsideForce, currentjumpForce), ForceMode2D.Impulse);
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

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // デバッグ表示（Sceneビューで確認できる）
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, Color.red);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁にぶつかったら
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("壁にぶつかりました");
            isWallhit = true;
        }

        //  プレイヤーにぶつかったら
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーにぶつかりました");

            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();  // IDamageableを実装していればインスタンスを取得

            if (damageable != null)
            {
                damageable.TakeDamage(damageToPlayer);  // ダメージを与える
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
