using System.Linq;
using UnityEngine;
using Activ.GOAP;
using static UnityEngine.Vector3;

public class SentinelAI : GameAI<SentinelModel>{

    const float LARGE_VALUE = 1e6f;
    public Vector2 target;
    Ground ground;

    Sentinel actor => GetComponent<Sentinel>();

    GameObject nearestTarget{ get{
        var t = FindObjectsOfType<SentinelAI>()
                .Aggregate((x, y) => Dist(x) < Dist(y) ? x : y );
        return t == this ? null : t.gameObject;
    }}

    void Start() => ground = FindObjectOfType<Ground>();

    override public void Update()
    { busy = actor.moving; base.Update(); }

    public void MoveLeft()    => actor.Move(left);
    public void MoveRight()   => actor.Move(right);
    public void MoveForward() => actor.Move(forward);
    public void MoveBack()    => actor.Move(back);
    public void Shoot()       => actor.Shoot(nearestTarget);

    public void Pull(){
        var w = ground.WillMoveProp(transform);
        actor.Pull(w);
    }

    override public Goal<SentinelModel> Goal()
    => new Goal<SentinelModel>(
        m => m.target == null,
        m => m.target?.Dist(m.x, m.y) ?? 0
    );

    override public SentinelModel Model(){
        var t = nearestTarget;
        if(t == null) return null;
        var p = t.transform.position;
        return new SentinelModel(
            transform,
            new SentinelModel.Target(p.x, p.z),
            ground.model);
    }

    Goal<SentinelModel> Reach()
    => new Goal<SentinelModel>(
        m => m.position == target,
        m => Vector2i.Distance(m.position, (Vector2i)target)
    );

    float Dist(Component c) => c == this ? LARGE_VALUE
       : Vector3.Distance(transform.position, c.transform.position);

}
