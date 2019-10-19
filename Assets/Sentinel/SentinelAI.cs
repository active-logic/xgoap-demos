using UnityEngine;
using Activ.GOAP;
using static UnityEngine.Vector3;

public class SentinelAI : GameAI<SentinelModel>{

    public Vector2 target;

    Sentinel actor => GetComponent<Sentinel>();

    public void MoveLeft()    => actor.Move(left);
    public void MoveRight()   => actor.Move(right);
    public void MoveForward() => actor.Move(forward);
    public void MoveBack()    => actor.Move(back);

    override public Goal<SentinelModel> Goal()
    => new Goal<SentinelModel>(
        m => m.position == target,
        m => Vector2.Distance(m.position, target)
    );

    override public SentinelModel Model()
    => new SentinelModel(transform);

}
