using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Vector3;
using static UnityEngine.Mathf;

public class Sentinel : MonoBehaviour{

    public float speed = 0.1f;
    Vector3? target;

    public void Move(Vector3 dir)
    => target = transform.position + dir;

    void Update(){ if(target.HasValue) UpdatePosition(); }

    void UpdatePosition(){
        var u = (target.Value - transform.position).normalized;
        var s = Time.deltaTime * speed;
        if(s < Distance(target.Value, transform.position)){
            transform.position += u * s;
        }else{
            transform.position = target.Value;
            target = null;
        }
    }

}
