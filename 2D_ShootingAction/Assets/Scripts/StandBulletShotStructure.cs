using UnityEngine;

[CreateAssetMenu(fileName = "StandBullet", menuName = "ShotStructure/StandBullet")] 
public class StandBulletShotStructure : BulletShotStructure
{
    public GameObject ScaffoldAmmo;  // 足場弾のプレハブ

    public override void UpwardFirinig(Transform firingPoint, float firingspeed)
    {
        Debug.Log("上入力が呼び出されました");
    }

    public override void ForwardFirinig(Transform firingPoint, float firingspeed)
    {
        Debug.Log("前入力が呼び出されました");
    }

    public override void DownwardFirinig(Transform firingPoint, float firingspeed)
    {
        Debug.Log("下入力が呼び出されました");
    }
}
