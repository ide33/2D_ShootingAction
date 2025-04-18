using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonBulletShotStructure : BulletShotStructure
{
    [SerializeField] private GameObject CannonAmmo;            // 大砲弾のプレハブ
    [SerializeField] private float CannonAmmoShotSpeed = 5f;   // 大砲弾の移動速度
    [SerializeField] private GameObject ShotPoint;             // 発射位置
    [SerializeField] private float DestroyTime = 4f;           // オブジェクト消去までの時間

    [Header("上入力の発射構造のフィールド変数")] 
    [SerializeField] private float UpwardForce = 10f;          // 発射力
    [SerializeField] private float UpwardFirinigAngle = 80f;   // 発射角度

    [Header("無入力の発射構造のフィールド変数")] 
    [SerializeField] private float ForwardForce = 8f;          // 発射力
    [SerializeField] private float ForwardFirinigAngle = 45f;  // 発射角度

    [Header("下入力の発射構造のフィールド変数")] 
    [SerializeField] private float DownwardForce = 5f;         // 発射力
    [SerializeField] private float DownwardFirinigAngle = 20f; // 発射角度


    // 上入力の発射構造　　（UpwardForceは一時的な変数　実装時にfiringSpeedに変更する）
    public override  void UpwardFirinig(Transform firingPoint, float firingSpeed, Vector2 direction)
    {
        FireProjectile(firingPoint, UpwardFirinigAngle, UpwardForce, direction);
    }

    // 無入力の発射構造　　（ForwardForceは一時的な変数　実装時にfiringSpeedに変更する）
    public override  void ForwardFirinig(Transform firingPoint, float firingSpeed, Vector2 direction)
    {
        FireProjectile(firingPoint, ForwardFirinigAngle, ForwardForce, direction);
    }

    // 下入力の発射構造　　（DownwardForceは一時的な変数　実装時にfiringSpeedに変更する）
    public override  void DownwardFirinig(Transform firingPoint, float firingSpeed, Vector2 direction)
    {
        FireProjectile(firingPoint, DownwardFirinigAngle, DownwardForce, direction);
    }

    // 角度と力を指定してAddForceでオブジェクトを移動させる
    private void FireProjectile(Transform firingPoint, float firingAngle, float forceMagnitude, Vector2 Turndirection)
    {
        GameObject newCannonAmmo = Instantiate(CannonAmmo, firingPoint.position, Quaternion.identity);
        Rigidbody2D rb = newCannonAmmo.GetComponent<Rigidbody2D>();

        // 向きが左の場合
        if(Turndirection.x == -1)
        {
            // 角度を左右対象になるように反転させる
            firingAngle = 180 - firingAngle;
        }

        // 角度をラジアンに変換
        float radian = firingAngle * Mathf.Deg2Rad;

        // 単位ベクトルを計算（角度方向の単位ベクトル）
        Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        // 指定された力の大きさを適用
        Vector2 force = direction * forceMagnitude;

        // AddForceで力を加えて放物運動させる
        rb.AddForce(force, ForceMode2D.Impulse);

        // 移動方向に回転するコルーチン開始
        // StartCoroutine(RotateToMovementDirection(newCannonAmmo.transform, rb));

        // 4秒後に削除
        Destroy(newCannonAmmo, DestroyTime);
    }

    // 指定されたオブジェクトの角度がリジッドボディの移動方向を向き続ける
    private IEnumerator RotateToMovementDirection(Transform projectile, Rigidbody2D rb)
    {
        // オブジェクトが消去されるまで継続
        while (projectile != null)
        {
            Vector2 velocity = rb.linearVelocity;
            if (velocity.sqrMagnitude > 0.01f) // ほぼ静止状態でなければ回転
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                projectile.rotation = Quaternion.Euler(0, 0, angle);
            }
            yield return null; // 毎フレーム更新
        }
    }
}
