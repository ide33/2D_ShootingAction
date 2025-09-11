using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Escapeキーで指定したシーンに移動するスクリプト
/// </summary>
public class InputSceneChangeESC : MonoBehaviour
{
    // インスペクターから設定する変数
    [SerializeField] private string SCENE_CHANGE_NAME; // 移動するシーン名
    // [SerializeField] private string SE_NAME;           // SEの名前

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if (SE_NAME != null)
            // {
            //     // SoundManager.Instance.PlaySE(SE_NAME); // SEを再生
            // }
            SceneChangeManager.Instance.ChangeSceneLoad(SCENE_CHANGE_NAME);
        }
    }
}
