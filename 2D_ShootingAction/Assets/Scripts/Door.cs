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
            Debug.Log("扉が開きました");
            doorCollider.enabled = false;  // コライダーを無効化
            Debug.Log("コライダーを無効化");
        }
    }

    // public void Deactivate()
    // {
    //     doorVisual.SetActive(true);  // ドア表示
    //     doorCollider.enabled = true;  // コライダーを有効化
    // }

    // public void Open()
    // {
    //     doorVisual.SetActive(false);  // 見た目を消す
    //     doorCollider.enabled = false;  // コライダーを無効化
    // }

    // public void Close()
    // {
    //     doorVisual.SetActive(true);  // 扉を見せる
    //     doorCollider.enabled = true;  // コライダーを有効化
    // }
}
