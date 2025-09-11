using UnityEngine;

public class UnlockResetStage : MonoBehaviour
{
    [SerializeField] private string SceneName;
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
            SceneChangeManager.Instance.ChangeSceneLoad(SceneName);
        }
    }
}
