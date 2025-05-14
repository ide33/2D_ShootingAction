using UnityEngine;

public class Door : MonoBehaviour, IActivatable
{
    [SerializeField] private Collider2D doorCollider;  // 扉のcollider
    private bool isOpen = false;

    public void Activate()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorCollider.enabled = false;  // コライダーを無効化
            
            Debug.Log("扉が開きました");
        }
        isOpen = false;
    }
}
