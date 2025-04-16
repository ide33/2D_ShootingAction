using UnityEngine;

public class StandEntity : MonoBehaviour
{
    [SerializeField] private GameObject Stand;
    [SerializeField] private float checkRadius = 0.5f; // 重なりチェック用の半径
    [SerializeField] private float offset = 0.1f; // ずらす距離

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundHitAmmo(collision);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            RushingEnemy enemy = collision.gameObject.GetComponent<RushingEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }

    void GroundHitAmmo(Collision2D collision)
    {
        Vector2 hitPosition = collision.contacts[0].point; // 衝突地点を取得
        Vector2 spawnPosition = hitPosition;

        // Standのサイズを取得
        BoxCollider2D standCollider = Stand.GetComponent<BoxCollider2D>();
        float halfWidth = standCollider.bounds.extents.x;  // オブジェクトサイズを取得、半分の幅を求める
        float halfHeight = standCollider.bounds.extents.y;  // オブジェクトサイズを取得、半分の高さを求める

        // 衝突した方向を取得
        Vector2 normal = collision.contacts[0].normal;

        if (normal.y > 0)  // 地面に着弾
        {
            spawnPosition.y += halfHeight;
        }
        else if (normal.y < 0)  // 天井に着弾
        {
            spawnPosition.y -= halfHeight;
        }
        else if (normal.x > 0)  // 左の壁に着弾
        {
            spawnPosition.x += halfWidth;
        }
        else if (normal.x < 0)  // 右の壁に着弾
        {
            spawnPosition.x -= halfWidth;
        }

        // 法線方向に適切にずらす
        Vector2 adjustmentDirection = normal.normalized;

        // 重ならない位置を探さす
        while (Physics2D.OverlapCircle(spawnPosition, checkRadius))
        {
            spawnPosition += adjustmentDirection; // 適切な方向にずらす
        }

        // Standを生成
        Instantiate(Stand, spawnPosition, Quaternion.identity);

        Destroy(gameObject);
    }
}
