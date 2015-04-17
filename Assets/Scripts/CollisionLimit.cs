using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 反発回数制限クラス
/// 指定時間内に特定回数以上オブジェクトに接触処理させないためのクラス
/// オブジェクトの名前を利用し、接触回数を制限する。
/// 同じ名前のオブジェクトは区別されない
/// 時間が経過すると削除されるQueueを作成することで処理している
/// </summary>
public class CollisionLimit : SingletonMonoBehaviour<CollisionLimit>
{
    public float deleteTime = 1.0f;
    public int maxCount = 2;

    /// <summary>
    /// すでにある場合は削除する。
    /// </summary>
    void Awake()
    {
        if(this != Instance)
        {
            Destroy(this);
            return;
        }
    }  

    /// <summary>
    /// アクティブになったら初期化を行う
    /// </summary>
    void OnEnable()
    {
        Clear ();
    }

    //二つの接触したオブジェクトの名前を入れる
    private class TwoCollisionObjectName
    {
        private string obj1;
        private string obj2;

        public TwoCollisionObjectName(string obj1, string obj2)
        {
            this.obj1 = obj1;
            this.obj2 = obj2;
        }

        public bool isEquals(TwoCollisionObjectName checkdata)
        {
            if (this.obj1 == checkdata.obj1) {
                if (this.obj2 == checkdata.obj2) {
                    return true;
                }
            }
            return false;
        }
    }
    private Queue<TwoCollisionObjectName> queue = new Queue<TwoCollisionObjectName>();

    /// <summary>
    /// 同じデータの個数を数えて返す
    /// </summary>
    /// <param name="data">Data.</param>
    private int Count(TwoCollisionObjectName data)
    {
        int count = 0;
        foreach(TwoCollisionObjectName index in queue)
        {
            if (data.isEquals(index)) {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// 時間が経過したら古いデータを削除する
    /// </summary>
    private void Delete()
    {
        queue.Dequeue ();
    }

    /// <summary>
    /// 当たっているかどうか判定する
    /// Queueにデータをため、同じものが指定数以上あるかチェックする
    /// </summary>
    /// <param name="data">Data.</param>
    public bool isHit(string obj1, string obj2)
    {
        TwoCollisionObjectName data = new TwoCollisionObjectName (obj1, obj2);
        if (Count (data) >= maxCount) {
            return false;
        }

        queue.Enqueue(data);
        Invoke ("Delete", deleteTime);
        return true;
    }

    /// <summary>
    /// データのリセット
    /// </summary>
    public void Clear()
    {
        queue.Clear ();
    }
}