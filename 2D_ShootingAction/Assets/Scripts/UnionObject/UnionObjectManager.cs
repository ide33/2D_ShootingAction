using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UnionObjectを管理するシングルトン
/// </summary>
public class UnionObjectManager : MonoBehaviour
{
    private static UnionObjectManager instance;
    public static UnionObjectManager Instance => instance;

     [SerializeField] private List<IUnionObject> unionObjectList = new List<IUnionObject>();
    [SerializeField] private int maxUnionDataCount = 5; // リストの最大数

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 追加
    public void AddToList(IUnionObject unionObject)
    {
        // リストの最大数を超えた場合、最初の要素を削除
        if (unionObjectList.Count >= maxUnionDataCount)
        {
            RemoveOldestData();
        }

        // 新しいデータをリストに追加
        unionObjectList.Add(unionObject);
    }

    // 削除
    public void RemoveFromList(IUnionObject unionObject)
    {
        unionObjectList.Remove(unionObject);
    }

    // リストの最大数を超えた場合、最初の要素を削除
    public void RemoveOldestData()
    {
        if (unionObjectList.Count > 0)
        {
            // 最初の要素を取得
            IUnionObject oldestObject = unionObjectList[0];

            // リストから削除
            unionObjectList.RemoveAt(0);

            // ゲームオブジェクトを削除
            if (oldestObject is MonoBehaviour monoBehaviour)
            {
                Destroy(monoBehaviour.gameObject);
            }
        }
    }
}
