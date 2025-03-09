using UnityEngine;

public interface IBulletShotStructure
{
    void UpwardFirinig(Transform firingPoint, float firingspeed);
    void ForwardFirinig(Transform firingPoint, float firingspeed);
    void DownwardFirinig(Transform firingPoint, float firingspeed);
}
