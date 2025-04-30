using UnityEngine;

public class Switch : MonoBehaviour, ISwitch
{
    [SerializeField] private MonoBehaviour targetObject;  // IDoorを実装しているオブジェクト
    private IDoor door;  // 扉のインターフェイスを保持する変数

    private void Awake()
    {
        door = targetObject as IDoor;  // targerObjectに代入されたオブジェクトをIDoorとして型変換

        if (door == null)  // IDoorが実装されていない場合
        Debug.LogError("targetObjectはIDoorを実装していません");
    }

    public void Activate()
    {
        door.Open();  // ボタンを押すと扉が開く
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ammo"))
        {
            Activate();
        }   
    }
}
