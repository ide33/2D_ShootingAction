using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゴール地点の処理
/// </summary>
public class GOALPoint : MonoBehaviour
{
    // インスペクターから設定する変数
    [Header("現在のステージの番号を入力")]
    [SerializeField] private int UNLOCK_STAGE_NUMBER; // 解放するステージの番号

    [Header("ゴール時に再生するSE名を入力")]
    [SerializeField] private string SE_Name;
    private bool isGOAL;

    private void Start()
    {
        GameManager.Instance.currentGameState = GameState.Game; // ゲーム状態をゲームに設定
        isGOAL = false;
    }

    // オブジェクトすり抜け判定取得
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isGOAL) // プレイヤータグに接触し、ゴール前なら
        {
            if (StageManager.Instance != null && StageManager.Instance.IsStageUnlocked(UNLOCK_STAGE_NUMBER) == false)
            {
                StageManager.Instance.UnlockStage(UNLOCK_STAGE_NUMBER); // ステージを解放
            }
            Debug.Log($"GOALPoint: Stage {UNLOCK_STAGE_NUMBER} unlocked!"); // デバッグログ出力
            if (SE_Name != null)
            {
                SoundManager.Instance.PlaySE(SE_Name); // SEを再生
            }

            GameManager.Instance.CurrentGameState = GameState.Clear; // ゲーム状態をクリアに設定

            isGOAL = true;
        }
    }
}
