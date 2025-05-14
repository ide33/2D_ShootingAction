using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // プレイヤーの速度
    [SerializeField] private float jumpForce = 5f;  // ジャンプの強さ
    [SerializeField] private float shootCoolTime = 0.3f;  // 攻撃のクールタイム
    [SerializeField] private float checkGroundDistance = 0.1f;  // 地面までの距離
    [SerializeField] private LayerMask groundLayer;  // Groundレイヤー
    [SerializeField] Transform checkGround;  // 地面判定位置
    [SerializeField] Transform shootPoint;  // 弾の発射位置
    [SerializeField] BulletShotStructure standBullet;  // 足場弾のスクリプタブルオブジェクト
    [SerializeField] BulletShotStructure cannonBullet;  // 大砲弾のスクリプタブルオブジェクト
    [SerializeField] BulletShotStructure moovStandBullet;  // 移動足場弾のスクリプタブルオブジェクト

    private float lastAttack;  // 最後に攻撃した時間
    private Rigidbody2D rb2d;  // Rigidbodyの変数
    private SpriteRenderer spriteRenderer;  // スプライトの変数

    void Start()
    {
        lastAttack = 0f;  // 最後の攻撃の値を初期化
        rb2d = GetComponent<Rigidbody2D>();  // Rigidbodyを取得
        spriteRenderer = GetComponent<SpriteRenderer>(); // spriteRendererの初期化
    }

    void Update()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // スペースキーが押され、地面にいるとき
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
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

        // 発射位置の調整
        float shootOffsetX = 0.6f;  // 弾の発射位置のオフセット(右向き)
        if (spriteRenderer.flipX)
        {
            shootOffsetX = -0.6f;  // 左向きの場合、反対側に移動
        }
        shootPoint.localPosition = new Vector3(shootOffsetX, 0.3f, 0);

        // クールタイムをチェック
        if (Time.time - lastAttack >= shootCoolTime)
        {
            // wキーが押されたら
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.J))  // jキーが押されたら
                {
                    standBullet.UpwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.L))  // lキーが押されたら
                {
                    cannonBullet.UpwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.K))  // kキーが押されたら
                {
                    moovStandBullet.UpwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
            }

            // sキーが押されたら
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyDown(KeyCode.J))  // jキーが押されたら
                {
                    standBullet.DownwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.L))  // lキーが押されたら
                {
                    cannonBullet.DownwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.K))  // kキーが押されたら
                {
                    moovStandBullet.DownwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
            }

            else
            {
                if (Input.GetKeyDown(KeyCode.J))  // jキーが押されたら
                {
                    standBullet.ForwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.L))  // lキーが押されたら
                {
                    cannonBullet.ForwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.K))  // kキーが押されたら
                {
                    moovStandBullet.ForwardFirinig(shootPoint, speed, direction);
                    lastAttack = Time.time;
                }
            }
        }
    }

    private bool IsGrounded()
    {
        // プレイヤーの位置から真下にレイを飛ばす
        RaycastHit2D hit = Physics2D.Raycast(checkGround.position, Vector2.down, checkGroundDistance, groundLayer);

        return hit.collider != null;
    }
}
