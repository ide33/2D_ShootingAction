using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;  // 最大HP
    [SerializeField] private float invincibleDuration = 2.0f;  // 無敵時間
    [SerializeField] private float flashInterval = 0.1f;  // 点滅間隔
    [SerializeField] private string HurtSE_NAME;  // HurtSEの名前

    private int currentHealth; // 現在のHP
    private bool isInvincible = false;  // 無敵中かどうか
    private SpriteRenderer[] spriteRenderers;  // スプライトの変数
    private Coroutine invincibleCoroutine;  // 無敵処理のコルーチン

    void Start()
    {
        currentHealth = maxHealth;  // HPを初期化

        // 子オブジェクトすべてからSpriteRendererを取得
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // ステージ開始時にGameStateをリセット
        if (GameManager.Instance.CurrentGameState == GameState.GameOver)
        {
            GameManager.Instance.CurrentGameState = GameState.Game;
            GameManager.Instance.GameTimeStart();
        }
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

        if (HurtSE_NAME != null)
        {
            SoundManager.Instance.PlaySE(HurtSE_NAME);
        }

        Debug.Log("プレイヤーがダメージを受けた");

        // 体力が0になったらゲームオーバー
        if (currentHealth <= 0)
        {
            Debug.Log("プレイヤーが倒されました");

            // ゲームオーバー処理
            GameManager.Instance.CurrentGameState = GameState.GameOver;

            // GameManager.Instance.GameTimeStop();

            return;
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
            // 全てのSpriteRendererの表示状態を切り替え
            foreach (var sr in spriteRenderers)
            {
                // 表示非表示の切り替え
                sr.enabled = !sr.enabled;
            }

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        // 無敵終了時に全て表示ONに戻す
        foreach (var sr in spriteRenderers)
        {
            sr.enabled = true;
        }
        isInvincible = false;  // 無敵解除
    }
}
