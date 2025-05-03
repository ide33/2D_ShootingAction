using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;  // IDoorを実装しているオブジェクト
    private IActivatable activatable;  // 扉のインターフェイスを保持する変数

    private void Start()
    {
        if (targetObject != null)
        {
            activatable = targetObject.GetComponent<IActivatable>();
        }
    }

    public void Open()
    {
        Debug.Log("Open() が呼ばれました");

        if (activatable != null)
        {
            Debug.Log("activatable が有効なので Activate() を呼びます");
            activatable.Activate();  // ボタンを押すと扉が開く
        }
        else
    {
        Debug.LogWarning("activatable が null です。targetObject の設定を確認してください。");
    }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Ammo"))
    //     {
    //         Open();
    //     }   
    // }
}
