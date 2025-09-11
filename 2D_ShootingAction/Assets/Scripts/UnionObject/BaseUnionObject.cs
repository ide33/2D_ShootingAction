using UnityEngine;

/// <summary>
/// 合成オブジェクトの基底クラス
/// </summary>
/*
    * このクラスは、合成オブジェクトの基底クラスです。
    * すべての合成オブジェクトはこのクラスを継承して作成します。
    * 
    * - UnionObjectManager.Instance.AddToList(this); // 生成時にリストに追加
    * - UnionObjectManager.Instance.RemoveFromList(this); // 消去時にリストから削除
*/
public abstract class BaseUnionObject : MonoBehaviour, IUnionObject
{
    public UnionData unionData { get; set; } // 継承プロパティ
    protected virtual void Start()
    {
        if (UnionObjectManager.Instance == null)
        {
            Debug.LogError("UnionObjectManager is not initialized.");
            return;
        }
        // 生成時にリストに追加
        UnionObjectManager.Instance.AddToList(this);
        OnObjectCreate(); // 基底クラスで生成時の処理を呼び出し
    }

    protected virtual void OnDestroy()
    {
        if (UnionObjectManager.Instance == null)
        {
            Debug.LogError("UnionObjectManager is not initialized.");
            return;
        }
        // 消去時にリストから削除
        UnionObjectManager.Instance.RemoveFromList(this);
        OnObjectDestroy(); // 基底クラスで消去時の処理を呼び出し
    }

    // データ設定用のメソッド
    public void SetUnionData(UnionData data)
    {
        if (data == null)
        {
            Debug.LogError("UnionData is null.");
            return;
        }
        unionData = data;
    }

    // IUnionObject のメソッド
    /* 共通する処理をここに記述
     * 必要に応じてオーバーライドして使用
     */
    public virtual void OnObjectCreate()
    {
        // Debug.Log($"{gameObject.name} has been created.");
    }

    public virtual void OnObjectDestroy()
    {
        // Debug.Log($"{gameObject.name} has been destroyed.");
    }

}
