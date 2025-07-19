using Unity.VisualScripting;
using UnityEngine;

public class JumpEnemy : MonoBehaviour, IDamageable
{
    private enum Jp_State
    {
        Jump,
        Landing
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
    private Animator animator;

    private Rigidbody2D rb;  // Rigidbodyの変数

    private Jp_State currentState = Jp_State.Landing;  // 初期状態はlanding

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2Dを取得
        currentHealth = maxHealth;  // Hpを初期化
        animator = GetComponent<Animator>();

        StateChange(Jp_State.Landing);
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

            case Jp_State.Landing:

                // landing状態の処理
                StateLanding();
                break;
        }
    }

    private void StateJump()
    {
        // jumpTimerに前フレームからの経過時間を足す
        jumpTimer += Time.deltaTime;

        // 地面にいて且つjumpTimerがクールダウンの値より大きければジャンプ
        if (isGrounded() && jumpTimer >= jumpCooldown)
        {
            jumpTimer = 0f;
            StateChange(Jp_State.Landing);
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

    private void StateLanding()
    {
        // jumpTimerに前フレームからの経過時間を足す
        jumpTimer += Time.deltaTime;

        if (isGrounded() && jumpTimer >= jumpCooldown)
        {
            jumpTimer = 0f;
            StateChange(Jp_State.Jump);
        }
    }

    private void StateChange(Jp_State newState)
    {
        // 現在のStateを離れるときの処理
        if (currentState == Jp_State.Landing)
        {
            // Landing状態を離れるとき
        }
        else if (currentState == Jp_State.Jump)
        {
            // Jump状態を離れるとき
        }

        // 新しいStateに入るときの処理
        if (newState == Jp_State.Landing)
        {
            // Landing状態に入るとき
            animator.SetBool("IsJumping", false);
        }
        else if (newState == Jp_State.Jump)
        {
            // Jump状態に入るとき
            Jumping();
            animator.SetBool("IsJumping", true);
        }
        currentState = newState;
        Debug.Log($"{currentState}");
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
        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, groundCheckDistance, Vector2.down);

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
