using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // プレイヤーの速度
    [SerializeField] private float jumpForce = 5f;  // ジャンプの強さ
    [SerializeField] private float ammoSpeed = 5f;  // 弾の速度
    [SerializeField] private float shootCoolTime = 0.5f;  // 攻撃のクールタイム
    [SerializeField] private float lastAttack = 0.0f;  // 最後に攻撃した時間
    [SerializeField] GameObject ScaffoldAmmo;  // 足場のプレハブ
    [SerializeField] GameObject MoveScaffoldAmmo;  // 移動足場のプレハブ
    [SerializeField] GameObject CannonAmmo;  // 大砲のプレハブ
    [SerializeField] Transform shootPoint;  // 弾の発射位置
    [SerializeField] BulletShotStructure standBullet;  // 足場弾のスクリプタブルオブジェクト
    [SerializeField] BulletShotStructure cannonBullet;  // 大砲弾のスクリプタブルオブジェクト
    [SerializeField] BulletShotStructure moovStandBullet;  // 移動足場弾のスクリプタブルオブジェクト

    private bool isGround = false;  // 地面にいるかどうか
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

        // wキーが押されたら
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.J))  // jキーが押されたら
            {
                Instantiate(ScaffoldAmmo, shootPoint.position, transform.rotation);   // 足場弾を生成
                standBullet.UpwardFirinig(shootPoint, speed, direction);
            }
            else if (Input.GetKeyDown(KeyCode.L))  // lキーが押されたら
            {
                Instantiate(CannonAmmo, shootPoint.position, transform.rotation);  // 大砲弾を生成
                cannonBullet.UpwardFirinig(shootPoint, speed, direction);
            }
            else if (Input.GetKeyDown(KeyCode.K))  // kキーが押されたら
            {
                Instantiate(MoveScaffoldAmmo, shootPoint.position, transform.rotation);  // 移動足場弾を生成
                moovStandBullet.UpwardFirinig(shootPoint, speed, direction);
            }
        }

        // sキーが押されたら
        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.J))  // jキーが押されたら
            {
                Instantiate(ScaffoldAmmo, shootPoint.position, transform.rotation);   // 足場弾を生成
                standBullet.DownwardFirinig(shootPoint, speed, direction);
            }
            else if (Input.GetKeyDown(KeyCode.L))  // lキーが押されたら
            {
                Instantiate(CannonAmmo, shootPoint.position, transform.rotation);  // 大砲弾を生成
                cannonBullet.DownwardFirinig(shootPoint, speed, direction);
            }
            else if (Input.GetKeyDown(KeyCode.K))  // kキーが押されたら
            {
                Instantiate(MoveScaffoldAmmo, shootPoint.position, transform.rotation);  // 移動足場弾を生成
                moovStandBullet.DownwardFirinig(shootPoint, speed, direction);
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.J))  // jキーが押されたら
            {
                standBullet.ForwardFirinig(shootPoint, speed, direction);
            }
            else if (Input.GetKeyDown(KeyCode.L))  // lキーが押されたら
            {
                Instantiate(CannonAmmo, shootPoint.position, transform.rotation);  // 大砲弾を生成
                cannonBullet.ForwardFirinig(shootPoint, speed, direction);
            }
            else if (Input.GetKeyDown(KeyCode.K))  // kキーが押されたら
            {
                Instantiate(MoveScaffoldAmmo, shootPoint.position, transform.rotation);  // 移動足場弾を生成
                moovStandBullet.ForwardFirinig(shootPoint, speed, direction);
            }
        }
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
