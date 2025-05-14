using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のクラス
/// </summary>
/*
    * このクラスは、合成オブジェクトの弾を表します。
    * 弾は、敵に命中すると、ダメージを与えます。
    * 地面に命中すると、指定したオブジェクトを生成します。
    * 合成オブジェクトに命中すると、設定された合成ルールに基づいた合成処理を行います。
    *
    * また、デリゲート(Action)を使用して、移動の更新アクションを実行することができます。
*/
public class Ammo : MonoBehaviour
{
    [SerializeField] private GameObject UnionObject; // 生成するオブジェクト
    [SerializeField] private float checkRadius = 0.5f; // 重なりチェック用の半径
    // [SerializeField] private float AttackPower = 10f; // 攻撃力
    [SerializeField] private UnionData AmmoUnionData; // この弾オブジェクトの合成要素
    [SerializeField] private CombinationRule combinationRule; // 合成ルールを設定

    public Action<float> OnUpdateMoveAction; // 移動の更新デリゲート

    private SpriteRenderer _spriteRenderer; // 向き確認用

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        OnUpdateMoveAction?.Invoke(Time.deltaTime); // 移動の更新デリゲートを呼び出す
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 相手のオブジェクトが `UnionObject` を持っているか確認
        IUnionObject stationary = collision.gameObject.GetComponent<IUnionObject>();
        if (stationary != null)
        {
            Debug.Log("UnionObject");
            TryCombine(stationary, collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("GroundHitAmmo");
            GroundHitAmmo(collision); // 衝突したオブジェクトが地面の場合、地面に着弾処理を実行
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // 敵にダメージを与える
            }
            Destroy(gameObject); // 弾を削除
        }
    }

    /// <summary>
    /// 合成処理を試みるメソッド
    /// </summary>
    /// <param name="stationary"></param>
    /// <param name="unionObject"></param>
    private void TryCombine(IUnionObject stationary, GameObject unionObject)
    {
        // Nullチェック
        if (AmmoUnionData == null)
        {
            Debug.LogError("AmmoUnionData is not set.");
            return;
        }

        if (stationary == null)
        {
            Debug.LogError("Stationary object is null.");
            return;
        }

        if (stationary.unionData == null)
        {
            Debug.LogError("Stationary object's unionData is null.");
            return;
        }

        // 自分と相手の要素を取得
        string elementA = AmmoUnionData.unionDataNeme;
        string elementB = stationary.unionData.unionDataNeme;

        // 合成可能かチェック
        GameObject resultObject = combinationRule.GetCombinationResult(elementA, elementB);
        if (resultObject != null)
        {
            // 新しいオブジェクトを生成
            GameObject newresultObject = Instantiate(resultObject, transform.position, resultObject.transform.rotation);
            SpriteRenderer newSpriteRenderer = newresultObject.GetComponent<SpriteRenderer>();

            // 弾の向きに応じてスプライトを反転させる
            newSpriteRenderer.flipX = _spriteRenderer.flipX;

            // 現在のオブジェクトと設置オブジェクトを削除
            Destroy(gameObject);
            Destroy(unionObject);
        }
    }

    void GroundHitAmmo(Collision2D collision)
    {
        Vector2 hitPosition = collision.contacts[0].point; // 衝突地点を取得
        Vector2 spawnPosition = hitPosition;

        Collider2D standCollider = UnionObject.GetComponent<Collider2D>();
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
        GameObject newUnionObject = Instantiate(UnionObject, spawnPosition, UnionObject.transform.rotation);

        SpriteRenderer newSpriteRenderer = newUnionObject.GetComponent<SpriteRenderer>();

        // 弾の向きに応じてスプライトを反転させる
        newSpriteRenderer.flipX = _spriteRenderer.flipX;

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