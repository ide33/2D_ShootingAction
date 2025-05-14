using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private Image hpGauge;

    void Start()
    {
        
    }

    public void DecreaseHp()
    {
        hpGauge.fillAmount -= 0.4f;
    }
}
