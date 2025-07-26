using UnityEngine;

public class CannonAmmo : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);  // 敵にダメージを与える
            }
            Destroy(gameObject);  // 弾を削除
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);  // 弾を削除
        }
    }
}
