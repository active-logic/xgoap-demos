using UnityEngine;

public class Explosion : MonoBehaviour{

    public float maxSize = 3f;
    public float factor = 1.1f;

    void Update(){
        var t = transform;
        t.localScale *= factor;
        if(t.localScale.x > maxSize) Destroy(gameObject);
    }

}
