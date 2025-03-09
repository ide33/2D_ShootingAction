using UnityEngine;

public abstract class BulletShotStructure : ScriptableObject, IBulletShotStructure
{
    public abstract void UpwardFirinig(Transform firingPoint, float firingspeed);
    public abstract void ForwardFirinig(Transform firingPoint, float firingspeed);
    public abstract void DownwardFirinig(Transform firingPoint, float firingspeed);
}
