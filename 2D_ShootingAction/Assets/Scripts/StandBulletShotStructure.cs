using UnityEngine;

[CreateAssetMenu(fileName = "StandBullet", menuName = "ShotStructure/StandBullet")]
public class StandBulletShotStructure : BulletShotStructure
{
    public GameObject ScaffoldAmmo;  // 足場弾のプレハブ

    public override void UpwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction)
    {
        Debug.Log("上入力が呼び出されました");
    }

    public override void ForwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction)
    {
        Debug.Log("前入力が呼び出されました");
        GameObject newscaffoldAmmo = Instantiate(ScaffoldAmmo, firingPoint.position, firingPoint.transform.rotation);   // 足場弾を生成
        Rigidbody2D scaffoldrb = newscaffoldAmmo.GetComponent<Rigidbody2D>();
        scaffoldrb.AddForce(direction * firingspeed, ForceMode2D.Impulse);
    }

    public override void DownwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction)
    {
        Debug.Log("下入力が呼び出されました");
    }
}
