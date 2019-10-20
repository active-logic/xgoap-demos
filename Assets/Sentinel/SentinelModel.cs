using System;
using UnityEngine;
using Activ.GOAP;
using static UnityEngine.Vector2;

[Serializable] public class SentinelModel : Agent{

    const int range = 3;
    public int x, y;
    public Target target;
    // The ground model is static so don't serialize it
    GroundModel ground;

    public Vector2 position{
        get => new Vector2(x, y);
        set{ x = (int)value.x; y = (int)value.y; }
    }

    public SentinelModel(Transform t, Target target,
                                      GroundModel ground){
        position = (t != null)
            ? new Vector2((int)t.position.x, (int)t.position.z)
            : new Vector2(0, 0);
        this.ground = ground;
        this.target = target;
    }

    public Func<Cost>[] actions => new Func<Cost>[]
    { MoveLeft, MoveRight, MoveForward, MoveBack, Shoot };

    public Cost MoveLeft()
    { if(Move(left))  return 1; else return false; }

    public Cost MoveRight()
    { if(Move(right)) return 1; else return false; }

    public Cost MoveBack()
    { if(Move(down))  return 1; else return false; }

    public Cost MoveForward()
    { if(Move(up))    return 1; else return false; }

    public Cost Shoot(){
        if(target.Dist(x, y) > range) return false;
        return (target = null, 1);
    }

    override public bool Equals(object other){
        var that = other as SentinelModel;
        return this.x == that.x
            && this.y == that.y
            && this.target == that.target
            && this.ground == that.ground;
    }

    override public int GetHashCode()
    => (x + y * 1000) * (target==null? 2 : 1);

    bool Move(Vector2 dir)
    { position += dir; return !ground.IsObstructed(position); }

    [Serializable] public class Target{

        public int x, y;

        public Target(int x, int y){ this.x = x; this.y = y; }

        public float Dist(int x1, int y1){
            int a = x1 - x, b = y1 - y;
            return Mathf.Sqrt(a * a + b * b);
        }

    }

}
