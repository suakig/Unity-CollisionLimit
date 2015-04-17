using UnityEngine;
using System.Collections;

public class SampleObject : MonoBehaviour {

    public enum Way
    {
        Up      =   1,
        Down    =   2,
        Left    =   4,
        Right   =   8
    }

    public Way way = Way.Up;
    private float speed = 30;

    void Start ()
    {
        WayInit ();
    }

    /// <summary>
    /// 初期移動方向の初期化
    /// </summary>
    void WayInit()
    {
        switch(way){
        case Way.Up:
            rigidbody.AddForce (Vector3.up);
            break;
        case Way.Down:
            rigidbody.AddForce (Vector3.down);
            break;
        case Way.Left:
            rigidbody.AddForce (Vector3.left);
            break;
        case Way.Right:
            rigidbody.AddForce (Vector3.right);
            break;
        }
    }

    /// <summary>
    /// 等速移動
    /// </summary>
    void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude == 0.0f) {
            WayInit ();
            return;
        }

        float ratio = speed / rigidbody.velocity.magnitude;
        rigidbody.velocity *= ratio;
    }

    /// <summary>
    /// Enter時に判定を行う
    /// </summary>
    /// <param name="collision">Collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "RangeBox") {
            return;
        }

        if (CollisionLimit.Instance.isHit (collision.gameObject.name, gameObject.name)) {
            Debug.Log ("Hit " + collision.gameObject.name + " and " + gameObject.name);
        } else {
            Debug.Log ("Not " + collision.gameObject.name + " and "+ gameObject.name);
        }
    }
}