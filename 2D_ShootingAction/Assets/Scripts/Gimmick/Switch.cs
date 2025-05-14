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
        if (activatable != null)
        {
            activatable.Activate();  // ボタンを押すと扉が開く
        }
    }
}
