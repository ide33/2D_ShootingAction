using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Image _image;
    private GameObject _player;
    private float maxHealth;
    private int currengHealth;
    private PlayerHealth _playerHealth;

    void Awake()
    {
        _image = GetComponent<Image>();

    }

    void Start()
    {
        if (Player != null)
        {
            _player = Player;
        }

        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }

        _playerHealth = _player.GetComponent<PlayerHealth>();
        maxHealth = _playerHealth.GetPlayerHealth();
        currengHealth = _playerHealth.GetPlayerHealth();
        Debug.Log($"MaxHP: {maxHealth}");
    }

    void Update()
    {
        int hp = _playerHealth.GetPlayerHealth();
        Debug.Log($"currentHP: {hp}");
        if (hp != currengHealth)
        {
            // HP割合でFillAmountを更新
            Debug.Log($"UIHP: {Mathf.Clamp01(hp / maxHealth)}");
            _image.fillAmount = Mathf.Clamp01(hp / maxHealth);
            currengHealth = hp;
        }
    }
}