using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;  // 最大HP
    [SerializeField] private float invincibleDuration = 2.0f;  // 無敵時間
    [SerializeField] private float flashInterval = 0.1f;  // 点滅間隔

    private int currentHealth; // 現在のHP
    private bool isInvincible = false;  // 無敵中かどうか
    private SpriteRenderer spriteRenderer;  // スプライトの変数
    private Coroutine invincibleCoroutine;  // 無敵処理のコルーチン

    void Start()
    {
        currentHealth = maxHealth;  // HPを初期化   
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        // まだ無敵時間中ならダメージを無効化
        if (isInvincible)
        {
            Debug.Log("無敵時間中");
            return;  // 無敵中なら処理を終了
        }

        currentHealth -= damage;

        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().DecreaseHp();  // HPゲージを減らす

        Debug.Log("プレイヤーがダメージを受けた");

        // 体力が0になったらゲームオーバー
        if (currentHealth <= 0)
        {
            Debug.Log("プレイヤーが倒されました");
            // ゲームオーバー処理
        }

        // 無敵時間を開始
        if (invincibleCoroutine != null)  // 無敵コルーチンが動いていたら
        {
            StopCoroutine(invincibleCoroutine);  // 無敵コルーチンを停止
        }
        invincibleCoroutine = StartCoroutine(Invincible());  // 無敵コルーチンスタート
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;  // 無敵中
        float elapsed = 0f;  // 経過時間

        while (elapsed < invincibleDuration)  // 無敵時間が終わるまで繰り返す
        {
            // スプライトの透明度を切り替え
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);  // 一定時間停止
            elapsed += flashInterval;  // 一時停止した時間をelapsedに足す
        }

        spriteRenderer.enabled = true;  // 表示を戻す
        isInvincible = false;  // 無敵解除
    }
}
