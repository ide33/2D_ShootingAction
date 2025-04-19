using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;  // 最大HP
    [SerializeField] private float invincibleDuration = 1.0f;  // 無敵時間

    private int currentHealth; // 現在のHP
    private float lastDamageTime = -Mathf.Infinity;  // 最後にダメージを受けた時間
    void Start()
    {
        currentHealth = maxHealth;  // HPを初期化   
    }

    public void TakeDamage(int damage)
    {
        // まだ無敵時間中ならダメージを無効化
        if (Time.time - lastDamageTime < invincibleDuration)
        {
            Debug.Log("無敵時間中");
            return;
        }

        currentHealth -= damage;
        lastDamageTime = Time.time;  // ダメージを受けた時間を更新
        Debug.Log("プレイヤーがダメージを受けた");

        // 体力が0になったらゲームオーバー
        if (currentHealth <= 0)
        {
            Debug.Log("プレイヤーが倒されました");
            // ゲームオーバー処理
        }
    }
}
