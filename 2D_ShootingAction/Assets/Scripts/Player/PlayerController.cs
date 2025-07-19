using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum Player_State
    {
        Idle,
        Walk,
        Jump,
        UpAttack,
        Attack,
        DownAttack,
        Fall
    }

    [SerializeField] private float speed = 5f;  // プレイヤーの速度
    [SerializeField] private float jumpForce = 5f;  // ジャンプの強さ
    [SerializeField] private float shootCoolTime = 0.3f;  // 攻撃のクールタイム
    [SerializeField] private float checkGroundDistance = 0.1f;  // 地面までの距離
    [SerializeField] private string AttackSE_NAME;  // AttackSEの名前
    [SerializeField] private string JumpSE_NAME;  // JumpSEの名前
    [SerializeField] private LayerMask groundLayer;  // Groundレイヤー
    [SerializeField] Transform checkGround;  // 地面判定位置
    [SerializeField] Transform shootPoint;  // 弾の発射位置
    [SerializeField] BulletShotStructure standBullet;  // 足場弾のスクリプタブルオブジェクト
    [SerializeField] BulletShotStructure cannonBullet;  // 大砲弾のスクリプタブルオブジェクト
    [SerializeField] BulletShotStructure moovStandBullet;  // 移動足場弾のスクリプタブルオブジェクト

    private float lastAttack;  // 最後に攻撃した時間
    private Rigidbody2D rb2d;  // Rigidbodyの変数
    private SpriteRenderer spriteRenderer;  // スプライトの変数
    private Animator animator;  // アニメーターの変数
    private float stateExitTime = 0;
    private float jumpExitTime = 1.0f;
    private bool attackStarted = false;
    private float horizontal;

    [SerializeField] private Player_State currentState = Player_State.Idle;  // 初期状態はIdle

    void Start()
    {
        lastAttack = 0f;  // 最後の攻撃の値を初期化
        rb2d = GetComponent<Rigidbody2D>();  // Rigidbodyを取得
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // spriteRendererの初期化
        animator = GetComponentInChildren<Animator>();  // Animatorの取得
    }

    void Update()
    {
        switch (currentState)
        {
            case Player_State.Idle:
                StateIdle();
                break;

            case Player_State.Walk:
                StateWalk();
                break;

            case Player_State.Jump:
                StateJump();
                break;

            case Player_State.UpAttack:
                StateUpAttack();
                break;

            case Player_State.Attack:
                StateAttack();
                break;

            case Player_State.DownAttack:
                StateDownAttack();
                break;

            case Player_State.Fall:
                StateFall();
                break;

        }
    }

    private void StateIdle()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            StateChange(Player_State.Walk);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            StateChange(Player_State.Jump);
        }

        // クールタイムをチェック
        if (Time.time - lastAttack >= shootCoolTime)
        {
            // wキーが押されたら
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))  // jキーが押されたら
                {
                    StateChange(Player_State.UpAttack);
                }
            }

            // Sキーが押されたら
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))  // jキーが押されたら
                {
                    StateChange(Player_State.DownAttack);
                }
            }

            else
            {
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))  // jキーが押されたら
                {
                    StateChange(Player_State.Attack);
                }
            }
        }
    }

    private void StateWalk()
    {
        // 左右移動
        horizontal = Input.GetAxis("Horizontal");  // 左右移動の入力をhorizontalに格納
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);  // horizontalで取得した方向に速度を掛ける

        if (horizontal > 0)
        {
            spriteRenderer.flipX = false;  //スプライトを通常の向きで表示
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;  //スプライトを左右反転した向きで表示
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            StateChange(Player_State.Jump);
        }

        if (horizontal == 0)
        {
            StateChange(Player_State.Idle);
        }
    }

    private void StateJump()
    {
        stateExitTime += Time.deltaTime;

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

        if (stateExitTime >= jumpExitTime)
        {
            StateChange(Player_State.Fall);
        }
    }

    private void StateUpAttack()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        if (!attackStarted)
        {
            if (Input.GetKey(KeyCode.J))  // jキーが押されたら
            {
                standBullet.UpwardFirinig(shootPoint, speed, direction);
            }

            else if (Input.GetKey(KeyCode.L))  // lキーが押されたら
            {
                cannonBullet.UpwardFirinig(shootPoint, speed, direction);
            }

            else if (Input.GetKey(KeyCode.K))  // kキーが押されたら
            {
                moovStandBullet.UpwardFirinig(shootPoint, speed, direction);
            }

            animator.SetInteger("AttackType", 2);

            if (AttackSE_NAME != null)
            {
                SoundManager.Instance.PlaySE(AttackSE_NAME);
            }

            lastAttack = Time.time;
            attackStarted = true;
        }

        if (Time.time - lastAttack >= 0.3f)
        {
            animator.SetInteger("AttackType", 0);
            StateChange(Player_State.Idle);
        }
    }

    private void StateAttack()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        if (!attackStarted)
        {
            if (Input.GetKey(KeyCode.J))  // jキーが押されたら
            {
                standBullet.ForwardFirinig(shootPoint, speed, direction);
            }

            else if (Input.GetKey(KeyCode.L))  // lキーが押されたら
            {
                cannonBullet.ForwardFirinig(shootPoint, speed, direction);
            }

            else if (Input.GetKey(KeyCode.K))  // kキーが押されたら
            {
                moovStandBullet.ForwardFirinig(shootPoint, speed, direction);
            }

            animator.SetInteger("AttackType", 1);

            if (AttackSE_NAME != null)
            {
                SoundManager.Instance.PlaySE(AttackSE_NAME);
            }

            lastAttack = Time.time;
            attackStarted = true;
        }

        if (Time.time - lastAttack >= 0.3f)
        {
            animator.SetInteger("AttackType", 0);
            StateChange(Player_State.Idle);
        }
    }

    private void StateDownAttack()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        if (!attackStarted)
        {
            if (Input.GetKey(KeyCode.J))  // jキーが押されたら
            {
                standBullet.DownwardFirinig(shootPoint, speed, direction);
            }

            else if (Input.GetKey(KeyCode.L))  // lキーが押されたら
            {
                cannonBullet.DownwardFirinig(shootPoint, speed, direction);
            }

            else if (Input.GetKey(KeyCode.K))  // kキーが押されたら
            {
                moovStandBullet.DownwardFirinig(shootPoint, speed, direction);
            }

            animator.SetInteger("AttackType", 3);

            if (AttackSE_NAME != null)
            {
                SoundManager.Instance.PlaySE(AttackSE_NAME);
            }

            lastAttack = Time.time;
            attackStarted = true;
        }

        if (Time.time - lastAttack >= 0.3f)
        {
            animator.SetInteger("AttackType", 0);
            StateChange(Player_State.Idle);
        }
    }

    private void StateFall()
    {
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

        if (IsGrounded())
        {
            StateChange(Player_State.Idle);
        }
    }

    private void StateChange(Player_State newState)
    {
        // 現在のStateを離れるときの処理
        if (currentState == Player_State.Idle)
        {
            // Idle状態を離れるとき
        }
        else if (currentState == Player_State.Walk)
        {
            // Walk状態を離れるとき
            animator.SetBool("IsWalking", false);
        }
        else if (currentState == Player_State.Jump)
        {
            // Jump状態を離れるとき
            animator.SetBool("IsJumping", false);
            stateExitTime = 0f;
        }
        else if (currentState == Player_State.UpAttack)
        {
            // UpAttack状態を離れるとき
            attackStarted = false;
        }
        else if (currentState == Player_State.Attack)
        {
            // Attack状態を離れるとき
            attackStarted = false;
        }
        else if (currentState == Player_State.DownAttack)
        {
            // DownAttack状態を離れるとき
            attackStarted = false;
        }
        else if (currentState == Player_State.Fall)
        {
            // Fall状態を離れるとき
            animator.SetBool("IsFalling", false);
        }

        // 新しいStateに入るときの処理
        if (newState == Player_State.Idle)
        {
            // Idle状態に入るとき
        }
        else if (newState == Player_State.Walk)
        {
            // Walk状態に入るとき
            animator.SetBool("IsWalking", true);
        }
        else if (newState == Player_State.Jump)
        {
            // Jump状態に入るとき
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, 0);  // 上向きの速度をリセット
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);  // ジャンプの力を加える

            animator.SetBool("IsJumping", true);

            if (JumpSE_NAME != null)
            {
                SoundManager.Instance.PlaySE(JumpSE_NAME);
            }
        }
        else if (newState == Player_State.UpAttack)
        {
            // UpAttack状態に入るとき
        }
        else if (currentState == Player_State.Attack)
        {
            // Attack状態に入るとき
        }
        else if (currentState == Player_State.DownAttack)
        {
            // DownAttack状態に入るとき
        }
        else if (newState == Player_State.Fall)
        {
            // Fall状態に入るとき
            animator.SetBool("IsFalling", true);
        }

        currentState = newState;  // 状態を更新
        Debug.Log("状態を更新");
    }

    private bool IsGrounded()
    {
        // プレイヤーの位置から真下にレイを飛ばす
        RaycastHit2D hit = Physics2D.Raycast(checkGround.position, Vector2.down, checkGroundDistance, groundLayer);

        return hit.collider != null;
    }
}