using UnityEngine;

public class JumpEnemy : MonoBehaviour
{
    private enum Jp_State
    {
        Jump
    }

    [SerializeField] private float jumpForce = 5f;  // 上方向の力
    [SerializeField] private float sideForce = 2f;  // 横方向の力
    [SerializeField] private Transform groundCheck;  // 接地判定の位置
    [SerializeField] private int maxHealth = 2;  // 最大Hp

    private int currentHealth;  // 現在のHp
    private bool isWallhit;  // 壁に当たったかどうか

    private Rigidbody2D rb;  // Rigidbodyの変数

    private Jp_State currentState = Jp_State.Jump;  // 初期状態はjump

    void Start()
    {
        currentHealth = maxHealth;  // Hpを初期化
    }

    void Update()
    {
        switch (currentState)
        {
            case Jp_State.Jump:
                StateJump();
                break;
        }
    }

    private void StateJump()
    {
        rb.AddForce(new Vector2(jumpForce , sideForce), ForceMode2D.Impulse);
    }

    private void OllisionEnter2D(Collision2D collision)
    {
        // 壁にぶつかったら
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("壁にぶつかりました");
            isWallhit = true;
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
