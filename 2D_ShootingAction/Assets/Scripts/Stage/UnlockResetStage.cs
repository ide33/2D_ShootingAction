using UnityEngine;

public class UnlockResetStage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StageManager.Instance.DeleteStageDate(); // Rキーが押されたらステージの解放状況をリセット
            Debug.Log("UnlockResetStage: Stage data reset!"); // デバッグログ出力
        }
    }
}
