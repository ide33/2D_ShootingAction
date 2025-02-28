using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // プレイヤーの速度
    [SerializeField] private float jumpForce = 5f;  // ジャンプの強さ
    [SerializeField] GameObject ScaffoldAmmo;  // 足場のプレハブ
    [SerializeField] GameObject MoveScaffoldAmmo;  // 移動足場のプレハブ
    [SerializeField] GameObject CannonAmmo;  // 大砲のプレハブ
    [SerializeField] Transform shootPoint;  // 弾の発射位置

    private bool isGround = false;  // 地面にいるかどうか
    private Rigidbody2D rb2d;  // Rigidbodyの変数
    // private Vector2 shootDirection = Vector2.right;  // デフォルトの発射方向
    private SpriteRenderer spriteRenderer;  // スプライトの変数

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();  // Rigidbodyを取得
        spriteRenderer = GetComponent<SpriteRenderer>(); // spriteRendererの初期化
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

        if(horizontal > 0)
        {
            // shootDirection = Vector2.right;  // 発射方向を右向き
            spriteRenderer.flipX = false;  //スプライトを通常の向きで表示
        }
        else if (horizontal < 0)
        {
            // shootDirection = Vector2.left;  // 発射方向を左向き
            spriteRenderer.flipX = true;  //スプライトを左右反転した向きで表示
        }

        // if(Input.GetKey(KeyCode.W)) shootDirection = Vector2.up;  // 発射方向を上向き
        // if(Input.GetKey(KeyCode.S)) shootDirection = Vector2.down;  // 発射方向を上向き

        // jキーが押されたら
        if(Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(ScaffoldAmmo, shootPoint.position, transform.rotation);   // 足場弾を生成
        }

        // kキーが押されたら
        if(Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(MoveScaffoldAmmo, shootPoint.position, transform.rotation);  // 移動足場弾を生成
        }

        // lキーが押されたら
        if(Input.GetKeyDown(KeyCode.L))
        {
            Instantiate(CannonAmmo, shootPoint.position, transform.rotation);  // 大砲弾を生成
        }
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
