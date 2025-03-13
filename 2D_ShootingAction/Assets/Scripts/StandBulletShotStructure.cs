using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StandBullet", menuName = "ShotStructure/StandBullet")]
public class StandBulletShotStructure : BulletShotStructure
{
    public GameObject ScaffoldAmmo;  // 足場弾のプレハブ

    public override void UpwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction)
    {
        Debug.Log("上入力が呼び出されました");

        GameObject newscaffoldAmmo = Instantiate(ScaffoldAmmo, firingPoint.position, firingPoint.transform.rotation);   // 足場弾を生成
        Rigidbody2D scaffoldrb = newscaffoldAmmo.GetComponent<Rigidbody2D>();  // 足場弾のRigidbodyを取得

        // 向いている方向に対応した斜めベクトルの生成
        Vector2 diagonalDirection = direction + Vector2.up;
        diagonalDirection.Normalize();  // ベクトルを正規化

        scaffoldrb.AddForce(diagonalDirection * firingspeed, ForceMode2D.Impulse);  // 弾に力を加える
        
        Destroy(newscaffoldAmmo, 2);  // オブジェクトを破壊
    }

    public override void ForwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction)
    {
        Debug.Log("前入力が呼び出されました");

        GameObject newscaffoldAmmo = Instantiate(ScaffoldAmmo, firingPoint.position, firingPoint.transform.rotation);   // 足場弾を生成
        Rigidbody2D scaffoldrb = newscaffoldAmmo.GetComponent<Rigidbody2D>();  // 足場弾のRigidbodyを取得
        scaffoldrb.AddForce(direction * firingspeed, ForceMode2D.Impulse);  // 弾に力を加える

        Destroy(newscaffoldAmmo, 2);  // オブジェクトを破壊
    }

    public override void DownwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction)
    {
        Debug.Log("下入力が呼び出されました");
        
        GameObject newscaffoldAmmo = Instantiate(ScaffoldAmmo, firingPoint.position, firingPoint.transform.rotation);   // 足場弾を生成
        Rigidbody2D scaffoldrb = newscaffoldAmmo.GetComponent<Rigidbody2D>();  // 足場弾のRigidbodyを取得

        // 向いている方向に対応した斜めベクトルの生成
        Vector2 diagonalDirection = direction + Vector2.down;
        diagonalDirection.Normalize();  // ベクトルを正規化

        scaffoldrb.AddForce(diagonalDirection * firingspeed, ForceMode2D.Impulse);  // 弾に力を加える

        Destroy(newscaffoldAmmo, 2);  // オブジェクトを破壊
    }
}