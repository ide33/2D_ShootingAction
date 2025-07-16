using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポーズウィンドウのアクティブ状態切り替え
/// </summary>
public class UIPose : MonoBehaviour
{
    // インスペクターから設定する変数
    [Header("アクティブ状態を切り替えるオブジェクト")]
    [SerializeField] GameObject SwitchingObject; // 切り替えるオブジェクト

    [Header("Escキーが押された時のSE")]
    [SerializeField] string EscKeyDownSE;        // キーが押された時のSEの名前

    // 内部処理する変数
    private bool isPose;                         // ポーズ(ゲームが止まった)状態の判定

    void Awake()
    {
        // フラグを初期化
        isPose = false;
    }

    void Update()
    {
        // Escapeキーが押された時フラグを切り替える
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPose = !isPose;

            // SEを鳴らす
            SoundManager.Instance.PlaySE(EscKeyDownSE);
        }

        // フラグに応じてオブジェクトのアクティブ状態を切り替える
        SetActiveSwitch(isPose);

        // ゲーム時間をフラグに応じて変更
        GameStop(isPose);
    }

    // オブジェクトのアクティブ状態を切り替える処理
    private void SetActiveSwitch(bool trigger)
    {
        SwitchingObject.SetActive(trigger);
    }

    // GameManagerのゲームの時間を変更する判定処理を呼び出す
    private void GameStop(bool trigger)
    {
        if(trigger)
        {
            GameManager.Instance.GameTimeStop();
        }
        else
        {
            GameManager.Instance.GameTimeStart();
        }
        
    }
}
