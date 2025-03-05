using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // プレイヤーの速度
    [SerializeField] private float jumpForce = 5f;  // ジャンプの強さ
    [SerializeField] private float shootCoolTime = 0.5f;  // 攻撃のクールタイム
    [SerializeField] private float lastAttack = 0.0f;  // 最後に攻撃した時間
    [SerializeField] GameObject ScaffoldAmmo;  // 足場のプレハブ
    [SerializeField] GameObject MoveScaffoldAmmo;  // 移動足場のプレハブ
    [SerializeField] GameObject CannonAmmo;  // 大砲のプレハブ
    [SerializeField] Transform shootPoint;  // 弾の発射位置

    private bool isGround = false;  // 地面にいるかどうか
    // private bool isAttackable = false;  // 攻撃可能かどうか
    private Rigidbody2D rb2d;  // Rigidbodyの変数
    private SpriteRenderer spriteRenderer;  // スプライトの変数

    void Start()
    {
        lastAttack = 0f;  // 最後の攻撃の値を初期化
        rb2d = GetComponent<Rigidbody2D>();  // Rigidbodyを取得
        spriteRenderer = GetComponent<SpriteRenderer>(); // spriteRendererの初期化
        // isAttackable = true;  // 攻撃可能
    }

    void Update()
    {
        // //  攻撃不可のとき
        // if (isAttackable == false)
        // {
        //     // 経過時間を足す
        //     lastAttack += Time.deltaTime;
        //     Debug.Log("クールタイム中");
        // }

        // スペースキーが押され、地面にいるとき
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, 0);  // 上向きの速度をリセット
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);  // ジャンプの力を加える
        }

        // 左右移動
        float horizontal = Input.GetAxis("Horizontal");  // 左右移動の入力をhorizontalに格納
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);  // horizontalで取得した方向に速度を掛ける

        if (horizontal > 0)
        {
            spriteRenderer.flipX = false;  //スプライトを通常の向きで表示
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;  //スプライトを左右反転した向きで表示
        }

        // if (lastAttack >= shootCoolTime)
        // {
            // isAttackable = true;
            // Debug.Log("攻撃可能");

            // jキーが押されたら
            if (Input.GetKeyDown(KeyCode.J))
            {
                Instantiate(ScaffoldAmmo, shootPoint.position, transform.rotation);   // 足場弾を生成
                // isAttackable = false;  // 攻撃不可
                // Debug.Log("クールタイムに入ります");
            }

            // kキーが押されたら
            if (Input.GetKeyDown(KeyCode.K))
            {
                Instantiate(MoveScaffoldAmmo, shootPoint.position, transform.rotation);  // 移動足場弾を生成
                // isAttackable = false;  // 攻撃不可
                // Debug.Log("クールタイムに入ります");
            }

            // lキーが押されたら
            if (Input.GetKeyDown(KeyCode.L))
            {
                Instantiate(CannonAmmo, shootPoint.position, transform.rotation);  // 大砲弾を生成
                // isAttackable = false;  // 攻撃不可
                // Debug.Log("クールタイムに入ります");
            }

        // }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 地面に触れているとき
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = true;
            Debug.Log("地面にいます");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 地面に触れていないとき
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = false;
            Debug.Log("地面にいません");
        }
    }
}
