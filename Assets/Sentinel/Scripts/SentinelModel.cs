using System;
using UnityEngine;
using Activ.GOAP;
using static Vector2i;
using static UnityEngine.Mathf;

[Serializable] public class SentinelModel : Agent, Clonable{

    const int range = 3;
    public int x, y;
    public Target target;
    GroundModel ground;
    int propIndex = -1;  // index of prop that can be pulled, or -1

    Func<Cost>[] actions;

    public object Clone() => new SentinelModel(){
        x = x, y = y,
        propIndex = propIndex,
        target = target,
        ground = (GroundModel)ground.Clone()
    };

    public Vector2i position{
        get => new Vector2i(x, y);
        set{ x = value.x; y = value.y; }
    }

    public SentinelModel(Transform t, Target target,
                                      GroundModel ground) : this(){
        position = (t != null)
            ? new Vector2i(t.position.x, t.position.z) : (0, 0);
        //
        var dir = (t != null)
            ? new Vector2i(t.forward.x, t.forward.z) : (0, 0);
        propIndex = ground.PullablePropIndex(position, dir);
        //
        this.ground = ground;
        this.target = target;
    }

    SentinelModel() => actions = new Func<Cost>[]
    { MoveLeft, MoveBack, MoveRight, MoveForward, Shoot, Pull };

    public Func<Cost>[] Actions() => actions;

    public Cost MoveLeft()    => Move(left);
    public Cost MoveRight()   => Move(right);
    public Cost MoveBack()    => Move(down);
    public Cost MoveForward() => Move(up);

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

    // Note: target compares by ref because it can be nulled but
    // never changes
    override public bool Equals(object other){
        var that = (SentinelModel)other;
        return this.x == that.x
            && this.y == that.y
            && this.ground.IsEqual(that.ground)
            && this.propIndex == that.propIndex
            && this.target == that.target;
    }

    override public int GetHashCode()
    => (((ground.GetHashCode()*31 + propIndex)*31) + x)*31 + y;

    bool Move(Vector2i dir){
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
