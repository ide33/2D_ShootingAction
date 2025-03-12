using UnityEngine;

public interface IBulletShotStructure
{
    void UpwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction);
    void ForwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction);
    void DownwardFirinig(Transform firingPoint, float firingspeed, Vector2 direction);
}
