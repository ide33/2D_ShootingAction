using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingBulletShotStructure : BulletShotStructure
{
    [SerializeField] private GameObject FloatingAmmo;          // 浮遊移動弾のプレハブ
    [SerializeField] private float FloatingAmmoShotSpeed = 5f; // 移動速度
    [SerializeField] private GameObject ShotPoint;             // 発射位置
    [SerializeField] private float DestroyTime = 5f;          // オブジェクト消去までの時間
    [SerializeField] private float floatForce = 5f;            // 振れる速度
    [SerializeField] private float maxAmplitude = 0.5f;        // 最大の振れ幅

    [Header("上入力の発射構造のフィールド変数")]
    [SerializeField] private float UpwardFirinigAngle = 45f;    // 上入力の発射角度

    [Header("無入力の発射構造のフィールド変数")]
    [SerializeField] private float ForwardFirinigAngle = 0f;    // 無入力の発射角度

    [Header("下入力の発射構造のフィールド変数")]
    [SerializeField] private float DownwardFirinigAngle = -45f; // 下入力の発射角度



    // 上入力の発射構造
    public override void UpwardFirinig(Transform firingPoint, float firingSpeed, Vector2 direction)
    {
        FireFloat(firingPoint, firingSpeed, direction, UpwardFirinigAngle);
    }

    // 無入力の発射構造
    public override void ForwardFirinig(Transform firingPoint, float firingSpeed, Vector2 direction)
    {
        FireFloat(firingPoint, firingSpeed, direction, ForwardFirinigAngle);
    }

    // 下入力の発射構造
    public override void DownwardFirinig(Transform firingPoint, float firingSpeed, Vector2 direction)
    {
        FireFloat(firingPoint, firingSpeed, direction, DownwardFirinigAngle);
    }

    // オブジェクトを生成し、揺れながら移動する処理を開始する
    private void FireFloat(Transform firingPoint, float firingSpeed, Vector2 direction, float angle)
    {
        GameObject newFloatingAmmo = Instantiate(FloatingAmmo, firingPoint.position, FloatingAmmo.transform.rotation);
        Rigidbody2D rigidbody2D = newFloatingAmmo.GetComponent<Rigidbody2D>();

        // 揺れながら移動するコルーチン開始
        // StartCoroutine(FloatUpDownSwayMove(rigidbody2D, angle, direction, firingSpeed, floatForce, maxAmplitude));

        // 数秒後に削除
        Destroy(newFloatingAmmo, DestroyTime);
    }

    // オブジェクトを指定した角度方向に移動しながら、移動方向に垂直なベクトルで上下に揺らす
    private IEnumerator FloatUpDownSwayMove(Rigidbody2D rb, float angle, Vector2 direction, float moveSpeed, float floatForce, float maxAmplitude)
    {
        // 移動方向を角度から単位ベクトルに変換
        Vector2 moveDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * direction.x, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        // 移動方向に対して垂直な方向を求める（法線ベクトル）
        Vector2 perpendicularDirection = new Vector2(-moveDirection.y, moveDirection.x);
        
        // オブジェクトの初期回転を取得
        float initialRotation = rb.transform.eulerAngles.z;

        // 移動方向の角度を計算（ラジアンから度数に変換）
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // オブジェクトを移動方向に向ける（初期回転を考慮）
        rb.transform.rotation = Quaternion.Euler(0, 0, targetAngle + initialRotation);

        // 最初の移動方向にImpulseで力を与える（初速を設定）
        rb.linearVelocity = moveDirection * moveSpeed;

        // 初期時間
        float time = 0f;

        while (rb != null)
        {
            // 時間に基づいて正弦波で振動を生成
            float oscillation = Mathf.Sin(time * floatForce) * maxAmplitude;
            
            // 現在の速度を保持
            Vector2 currentVelocity = rb.linearVelocity;
            
            // 移動方向の速度を維持しながら、垂直方向の速度を更新
            float perpendicularSpeed = oscillation * floatForce;
            currentVelocity = moveDirection * moveSpeed + perpendicularDirection * perpendicularSpeed;
            
            // 速度を適用
            rb.linearVelocity = currentVelocity;

            // 時間を加算
            time += Time.deltaTime;
            yield return null;
        }
    }
}