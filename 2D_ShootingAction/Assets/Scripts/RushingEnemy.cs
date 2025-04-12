using Unity.VisualScripting;
using UnityEngine;

public class RushingEnemy : MonoBehaviour
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

    private Re_State currentState = Re_State.Patrol;  // 初期状態はPatrol

    private float stateStartTime;  // 遷移後の時間を記録
    private float currentSpeed;  // 現在の速度
    private int direction = 1;  // 1:右へ移動, -1:左へ移動
    private Rigidbody2D rb;  // Rigidbodyを取得

    void Update()
    {
        switch (currentState)
        {
            case Re_State.Patrol:
                currentSpeed = normalSpeed;

                // Patrol状態の処理
                StatePatrol();
                break;

            case Re_State.Stun:
                currentSpeed = 0;

                // Stun状態の処理
                StateStun();
                break;

            case Re_State.Rush:
                currentSpeed = rushSpeed;

                // Rush状態の処理
                StateRush();
                break;
        }
    }

    private void StatePatrol()
    {
        // プレイヤーの位置を取得
        float distance = Vector2.Distance(transform.position, targetPlayer.transform.position);
        Move();

        // プレイヤーとの距離が近かったら
        if (distance < rushStartDistance)
        {
            StateChange(Re_State.Rush);
        }
    }

    private void StateStun()
    {
        Move();
        
        // スタンして数秒経過したら
        if (Time.time - stateStartTime >= stunDuration)
        {
            StateChange(Re_State.Patrol);
        }
    }

    private void StateRush()
    {
        Move();

        if (gameObject.CompareTag("Wall"))
        {
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
        }
        else if (currentState == Re_State.Rush)
        {
            // Rush状態を離れるとき
        }

        // 新しいStateに入るときの処理
        if (newState == Re_State.Patrol)
        {
            // Patrol状態に入るとき
            Vector2 playerpos = targetPlayer.transform.position;
        }
        else if (newState == Re_State.Stun)
        {
            // Stun状態に入るとき   
        }
        else if (newState == Re_State.Rush)
        {
            // Rush状態に入るとき
        }
    }

    private void Move()
    {
        // 現在の方向に移動
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
    }
}
