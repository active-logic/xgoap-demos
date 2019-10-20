using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Vector3;
using static UnityEngine.Mathf;

public class Sentinel : MonoBehaviour{

    public float speed = 0.1f;
    Vector3? target;
    Transform hold;

    public bool moving => target.HasValue;

    public void Move(Vector3 dir)
    => target = transform.position + dir;

    public void Shoot(GameObject target) => Destroy(target);

    public void Pull(GameObject prop){
        target = transform.position - transform.forward;
        (hold = prop.transform).SetParent(transform);
    }

    void Update(){ if(target.HasValue) UpdatePosition(); }

    void UpdatePosition(){
        var u = (target.Value - transform.position).normalized;
        var s = Time.deltaTime * speed;
        if(s < Distance(target.Value, transform.position)){
            transform.position += u * s;
        }else{
            transform.position = target.Value;
            target = null;
            hold?.SetParent(null);
        }
    }

}
