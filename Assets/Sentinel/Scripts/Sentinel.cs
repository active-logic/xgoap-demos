using System.Collections;
using System.Collections.Generic;
using Ex = System.Exception;
using UnityEngine;
using static UnityEngine.Vector3;
using static UnityEngine.Mathf;

public class Sentinel : MonoBehaviour{

    public float   speed = 1f;
    public GameObject explosionPrefab;
    float rotationalSpeed = 3;  // rads per second
    float charge = 0f;
    Vector3?  goal;             // navigation target
    Transform target;           // shooting target

    Transform hold;

    public string status;

    // Properties --------------------------------------------------

    public bool busy => goal.HasValue || target;

    Vector3 position{
        get => transform.position;
        set => transform.position = value;
    }

    Transform view => transform.GetChild(0);
    Renderer  beam => view.GetChild(1).GetComponent<Renderer>();

    // Methods -----------------------------------------------------

    public void Move(Vector3 dir)
    { goal = position + dir; LookAt(dir);  }

    public void Shoot(GameObject x){
        rotationalSpeed /= 4f; target = x.transform;
        LookAt(target.position - position);
    }

    public void Pull(GameObject prop){
        if(prop == null) throw new Ex("Prop is null");
        goal = position - transform.forward;
        (hold = prop.transform).SetParent(transform);
    }

    // Life cycle --------------------------------------------------

    void Start() => beam.enabled = false;

    void Update(){
        UpdateRotation();
        if(target) AimAndShoot();
        else       if(goal.HasValue) UpdatePosition();
    }

    // -------------------------------------------------------------

    void LookAt(Vector3 u){
        var v = view.forward;
        transform.forward = u; view.forward = v;
    }

    void AimAndShoot(){
        var α = Angle(view.forward, transform.forward);
        status = $"Aiming towards target: {α:0.#}";
        if(α > 0.01f) return;
        charge += 0.01f;
        if(charge >= 0.9f) beam.enabled = true;
        if(charge >= 1.0f){
            Destroy(target.gameObject);
            Instantiate(explosionPrefab, target.position, Quaternion.identity);
            beam.enabled = false;
        }
    }

    void UpdateRotation()
    => view.forward = RotateTowards(view.forward, transform.forward,
                      rotationalSpeed * Time.deltaTime, 1f);

    void UpdatePosition(){
        var u = (goal.Value - position).normalized;
        var s = Time.deltaTime * speed;
        if(s < Distance(goal.Value, position)){
            position += u * s;
        }else{
            position = goal.Value;
            goal = null;
            hold?.SetParent(null);
        }
    }

}
