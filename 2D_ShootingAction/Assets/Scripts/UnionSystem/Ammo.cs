using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のクラス
/// </summary>
public class Ammo : MonoBehaviour
{
    [SerializeField] private GameObject UnionObject; // 足場オブジェクト
    [SerializeField] private float checkRadius = 0.5f; // 重なりチェック用の半径
    // [SerializeField] private float AttackPower = 10f; // 攻撃力
    [SerializeField] private UnionData AmmoUnionData; // このオブジェクトの要素
    [SerializeField] private CombinationRule combinationRule; // 合成ルールを設定

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 相手のオブジェクトが `UnionObject` を持っているか確認
        IUnionObject stationary = other.GetComponent<IUnionObject>();
        if (stationary != null)
        {
            Debug.Log("UnionObject");
            TryCombine(stationary, other.gameObject);
        }
    }

    /// <summary>
    /// 合成処理を試みるメソッド
    /// </summary>
    /// <param name="stationary"></param>
    /// <param name="unionObject"></param>
    private void TryCombine(IUnionObject stationary, GameObject unionObject)
    {
        // 自分と相手の要素を取得
        string elementA = AmmoUnionData.unionDataNeme;
        string elementB = stationary.unionData.unionDataNeme;

        // 合成可能かチェック
        GameObject resultObject = combinationRule.GetCombinationResult(elementA, elementB);
        if (resultObject != null)
        {
            // 新しいオブジェクトを生成
            Instantiate(resultObject, transform.position, Quaternion.identity);

            // 現在のオブジェクトと設置オブジェクトを削除
            Destroy(gameObject);
            Destroy(unionObject);
        }
    }

    void GroundHitAmmo(Collision2D collision)
    {
        Vector2 hitPosition = collision.contacts[0].point; // 衝突地点を取得
        Vector2 spawnPosition = hitPosition;

        // Standのサイズを取得
        BoxCollider2D standCollider = UnionObject.GetComponent<BoxCollider2D>();
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
        Instantiate(UnionObject, spawnPosition, Quaternion.identity);

        Destroy(gameObject);
    }



    /// <summary>
    /// 弾の攻撃処理
    /// </summary>
    private void Attack()
    {
        // 弾の攻撃処理をここに実装
        // 例: ダメージを与える、エフェクトを表示するなど
    }
}