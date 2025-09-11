using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RushEnemy : MonoBehaviour, IDamageable
{
    private enum Re_State
    {
        Patrol,
        Stun,
        Rush
    }

    [SerializeField] private GameObject targetPlayer;  // プレイヤーのオブジェクト
    [SerializeField] private float normalSpeed = 2f;  // 移動速度
    [SerializeField] private float rushSpeed = 4f;  // 突進時の移動速度
    [SerializeField] private float rushStartDistance = 5f;  // 突進を開始する距離
    [SerializeField] private float stunDuration = 4f;  // スタンした時間
    [SerializeField] private int maxHealth = 3;  // 最大HP
    [SerializeField] private int damageToPlayer = 1;  // プレイヤーに与えるダメージ

    private Re_State currentState = Re_State.Patrol;  // 初期状態はPatrol

    private float stateStartTime;  // 遷移後の時間を記録
    private float currentSpeed;  // 現在の速度
    private int direction = 1;  // 1:右へ移動, -1:左へ移動
    private int currentHealth; // 現在のHP
    private bool isWallhit = false;  // 壁にぶつかったかどうか
    private Rigidbody2D rb;  // Rigidbodyの変数

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbodyを取得
        currentHealth = maxHealth;  // HPを初期化
    }

    void Update()
    {
        // GameOverだったら処理しない
        if (GameManager.Instance.CurrentGameState == GameState.GameOver) return;

        switch (currentState)
        {
            case Re_State.Patrol:
                currentSpeed = normalSpeed;  // 速さを普通に設定

                // Patrol状態の処理
                StatePatrol();
                break;

            case Re_State.Stun:
                currentSpeed = 0;  // 速さを0に設定

                // Stun状態の処理
                StateStun();
                break;

            case Re_State.Rush:
                currentSpeed = rushSpeed;  // 速さを早めに設定

                // Rush状態の処理
                StateRush();
                break;
        }
    }

    private void StatePatrol()
    {
        Vector2 mypos = transform.position;  // 自分の位置を取得
        Vector2 playerpos = targetPlayer.transform.position;  // プレイヤーの位置を取得

        // プレイヤーの位置を取得
        float distance = Vector2.Distance(transform.position, targetPlayer.transform.position);


        Move();

        // プレイヤーとの距離が近かったら
        if (distance < rushStartDistance)
        {
            // プレイヤーの方向を取得
            float directionToPlayer = Mathf.Sign(playerpos.x - mypos.x);  // -1(左) or 1(右)

            // プレイヤーが後ろにいたら反転
            if ((directionToPlayer > 0 && transform.localScale.x < 0) || (directionToPlayer < 0 && transform.localScale.x > 0))
            {
                Flip();
            }
            StateChange(Re_State.Rush);
        }

        // 壁にぶつかったら
        if (isWallhit)
        {
            isWallhit = false;  // フラグをリセット
            Flip();
        }
    }

    private void StateStun()
    {
        Move();

        // スタンして数秒経過したら
        if (Time.time - stateStartTime >= stunDuration)
        {
            Debug.Log("スタンを抜けます");
            StateChange(Re_State.Patrol);
        }
    }

    private void StateRush()
    {
        Move();

        if (isWallhit)
        {
            isWallhit = false;  // フラグをリセット
            StateChange(Re_State.Stun);  // Stun状態に遷移
        }
    }

    private void StateChange(Re_State newState)
    {
        // 現在のStateを離れるときの処理
        if (currentState == Re_State.Patrol)
        {
            // Patrol状態を離れるとき
        }
        else if (currentState == Re_State.Stun)
        {
            // Stun状態を離れるとき

            // 振り向くべきか判定（プレイヤーの方向と向きが逆ならFlip）
            float directionToPlayer = targetPlayer.transform.position.x - transform.position.x;

            if ((directionToPlayer > 0 && transform.localScale.x < 0) || (directionToPlayer < 0 && transform.localScale.x > 0))
            {
                Flip();
            }
        }
        else if (currentState == Re_State.Rush)
        {
            // Rush状態を離れるとき
        }

        // 新しいStateに入るときの処理
        if (newState == Re_State.Patrol)
        {
            // Patrol状態に入るとき
        }
        else if (newState == Re_State.Stun)
        {
            // Stun状態に入るとき   
        }
        else if (newState == Re_State.Rush)
        {
            // Rush状態に入るとき
        }
        stateStartTime = Time.time;  // 状態開始時間を記録

        currentState = newState;  // 状態を更新
        Debug.Log("状態を更新");
    }

    private void Move()
    {
        // 現在の方向に移動
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
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
