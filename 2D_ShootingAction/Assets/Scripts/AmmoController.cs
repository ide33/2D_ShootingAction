using UnityEngine;
using UnityEngine.UIElements;

public class AmmoController : MonoBehaviour
{
    [SerializeField] private float ammoSpeed = 5f;  // 弾の速度
    private Vector2 direction = Vector2.right;  // デフォルトの発射方向
    void Start()
    {

    }

    void Update()
    {
        transform.Translate(direction * ammoSpeed * Time.deltaTime);  // 弾の移動
    }
}
