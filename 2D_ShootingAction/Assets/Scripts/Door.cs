using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    [SerializeField] private GameObject doorVisual;  // 扉の見た目
    [SerializeField] private Collider2D doorCollider;  // 扉のcollider

    public void Open()
    {
        doorVisual.SetActive(false);  // 見た目を消す
        doorCollider.enabled = false;  // コライダーを無効化
    }

    public void Close()
    {
        doorVisual.SetActive(true);  // 扉を見せる
        doorCollider.enabled = true;  // コライダーを有効化
    }
}
