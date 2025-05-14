using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 1;  // 与えるダメージ

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();  // IDamageableを実装していればインスタンスを取得

            if (damageable != null)
            {
                damageable.TakeDamage(damageToPlayer);  // ダメージを与える
            }
        }
    }
}
