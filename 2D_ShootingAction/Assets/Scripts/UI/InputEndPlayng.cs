using UnityEngine;

public class InputEndPlayng : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
#if UNITY_EDITOR // エディタ起動中の処理
        UnityEditor.EditorApplication.isPlaying = false; // ゲーム実行を終了

#else // それ以外 ＝ ビルドしたゲーム起動時の処理
        Application.Quit(); // ゲームプレイ終了
#endif
    }
}
