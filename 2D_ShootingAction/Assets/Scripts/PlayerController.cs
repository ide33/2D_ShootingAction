using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // プレイヤーの速度
    [SerializeField] private float jumpForce = 5f;  // ジャンプの強さ

    private bool isGround = false;
    private Rigidbody2D rb2d;  // Rigidbodyの変数

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();  // Rigidbodyを取得
    }

    void Update()
    {
        // スペースキーが押され、地面にいるとき
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, 0);  // 上向きの速度をリセット
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);  // ジャンプの力を加える
        }

        // 左右移動
        float horizontal = Input.GetAxis("Horizontal");  // 左右移動の入力をhorizontalに格納
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);  // horizontalで取得した方向に速度を掛ける
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 地面に触れているとき
        if(collision.collider.CompareTag("Ground"))
        {
            isGround = true;
            Debug.Log("地面にいます");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 地面に触れていないとき
        if(collision.collider.CompareTag("Ground"))
        {
            isGround = false;
            Debug.Log("地面にいません");
        }
    }
}
