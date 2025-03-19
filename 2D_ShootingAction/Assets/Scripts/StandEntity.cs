using UnityEngine;

public class StandEntity : MonoBehaviour
{
    [SerializeField] private GameObject Stand;
    [SerializeField] private Vector2 checkSize = new Vector2(1f, 1f); // 重なりチェック用のサイズ
    [SerializeField] private LayerMask groundLayer; // 地面や他の足場のレイヤー
    [SerializeField] private float offset = 0.1f; // ずらす距離
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            GroundHitAmmo(collision);
        }
    }

    void GroundHitAmmo(Collision2D collision)
    {
        Vector2 hitPosition = collision.contacts[0].point; // 衝突地点を取得
        Vector2 spawnPosition = hitPosition;

        // もしその場所にオブジェクトがあるなら、少しずらして配置
        while (Physics2D.OverlapBox(spawnPosition, checkSize, 0, groundLayer))
        {
            spawnPosition.y += offset; // 0.1ずつ上にずらす
        }

        Instantiate(Stand, spawnPosition, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
