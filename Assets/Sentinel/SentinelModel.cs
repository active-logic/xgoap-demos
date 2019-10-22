using System;
using UnityEngine;
using Activ.GOAP;
using static UnityEngine.Vector2;
using static UnityEngine.Mathf;

[Serializable] public class SentinelModel : Agent, Clonable{

    const int range = 3;
    public int x, y;
    public Target target;
    GroundModel ground;
    int propIndex = -1;  // index of prop that can be pulled, or -1

    Func<Cost>[] _actions;

    public object Clone() => new SentinelModel(){
        x = x, y = y,
        propIndex = propIndex,
        target = target,
        ground = (GroundModel)ground.Clone()
    };

    public Vector2 position{
        get => new Vector2(x, y);
        set{ x = RoundToInt(value.x); y = RoundToInt(value.y); }
    }

    public SentinelModel(Transform t, Target target,
                                      GroundModel ground) : this(){
        position = (t != null)
            ? new Vector2(t.position.x, t.position.z)
            : new Vector2(0, 0);
        //
        var dir = (t != null)
            ? new Vector2(t.forward.x, t.forward.z)
            : new Vector2(0, 0);
        propIndex = ground.PullablePropIndex(position, dir);
        //
        this.ground = ground;
        this.target = target;
    }

    SentinelModel(){
        _actions = new Func<Cost>[]
        { MoveLeft, MoveBack, MoveRight, MoveForward, Shoot, Pull };
    }

    public Func<Cost>[] actions => _actions;

    public Cost MoveLeft()
    { if(Move(left))  return 1; else return false; }

    public Cost MoveRight()
    { if(Move(right)) return 1; else return false; }

    public Cost MoveBack()
    { if(Move(down))  return 1; else return false; }

    public Cost MoveForward()
    { if(Move(up))    return 1; else return false; }

    public Cost Pull(){
        if(propIndex == -1) return false;
        var u = ground.PullProp(propIndex, position);
        position += u;
        if(ground.IsObstructed(position + u)) propIndex = -1;
        return 1;
    }

    public Cost Shoot(){
        if(target.Dist(x, y) > range) return false;
        return (target = null, 1);
    }

    override public bool Equals(object other){
        var that = other as SentinelModel;
        if(that == null) Print("Why is that null?");
        //if(that.ground == null) Print("Why is that ground null?");
        //if(that.target == null) Print("Why is that target null?");
        return this.ground.IsEqual(that.ground)
            // by ref because target itself never changes
            && this.target == that.target
            && this.x == that.x
            && this.y == that.y
            && this.propIndex == that.propIndex;
    }

    override public int GetHashCode() => propIndex*31*31 + x*31 + y;

    bool Move(Vector2 dir){
        var p = position + dir;
        if(ground.IsObstructed(p)) return false;
        position = p;
        propIndex = ground.PullablePropIndex(position, dir);
        return true;
    }

    public void Print(object x) => UnityEngine.Debug.Log(x);

    override public string ToString()
    => $"M[{x}, {y} /p:{propIndex}]";

    // -------------------------------------------------------------

    [Serializable] public class Target{

        public int x, y;

        public Target(float x, float y){
            this.x = RoundToInt(x);
            this.y = RoundToInt(y);
        }

        public Target(int x, int y){ this.x = x; this.y = y; }

        public float Dist(int x1, int y1){
            int a = x1 - x, b = y1 - y;
            return Mathf.Sqrt(a * a + b * b);
        }

        override public bool Equals(object other){
            if(other == null) return false;
            var that = other as Target;
            return this.x == that.x && this.y == that.y;
        }

        override public int GetHashCode() => x*31 + y;

    }

}
