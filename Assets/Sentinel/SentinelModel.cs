using System;
using UnityEngine;
using Activ.GOAP;
using static UnityEngine.Vector2;

[Serializable] public class SentinelModel : Agent, Clonable{

    const int range = 3;
    public int x, y;
    public int dirX, dirY;
    public Target target;
    // The ground model is static so don't serialize it
    GroundModel ground;

    public object Clone() => new SentinelModel(){
        x = x, y = y,
        dirX = dirX, dirY = dirY,
        target = target,
        ground = (GroundModel)ground.Clone()
    };

    public Vector2 position{
        get => new Vector2(x, y);
        set{ x = (int)value.x; y = (int)value.y; }
    }

    public Vector2 direction{
        get => new Vector2(dirX, dirY);
        set{ dirX = (int)value.x; dirY = (int)value.y; }
    }

    public SentinelModel(Transform t, Target target,
                                      GroundModel ground){
        position = (t != null)
            ? new Vector2((int)t.position.x, (int)t.position.z)
            : new Vector2(0, 0);
        direction = (t != null)
            ? new Vector2((int)t.forward.x, (int)t.forward.z)
            : new Vector2(0, 0);
        this.ground = ground;
        this.target = target;
    }

    SentinelModel(){}

    public Func<Cost>[] actions => new Func<Cost>[]
    { MoveLeft, MoveBack, MoveRight, MoveForward, Shoot, Pull };

    public Cost MoveLeft()
    { if(Move(left))  return 1; else return false; }

    public Cost MoveRight()
    { if(Move(right)) return 1; else return false; }

    public Cost MoveBack()
    { if(Move(down))  return 1; else return false; }

    public Cost MoveForward()
    { if(Move(up))    return 1; else return false; }

    public Cost Pull(){
        var here   = position;
        var ahead  = here + direction;
        var behind = here - direction;
        if(!ground.IsProp(ahead) || ground.IsObstructed(behind))
            return false;
        //ebug.Log($"Move prop {ahead} => {here}");
        ground.MoveProp(ahead, here);
        position = behind;
        return 1;
    }

    public Cost Shoot(){
        if(target.Dist(x, y) > range) return false;
        return (target = null, 1);
    }

    override public bool Equals(object other){
        var that = other as SentinelModel;
        return this.ground.IsEqual(that.ground)
            && this.target.Equals(that.target)
            && this.x == that.x
            && this.y == that.y
            && this.dirX == that.dirX
            && this.dirY == that.dirY;
    }

    override public int GetHashCode()
    //=> dirX + dirY * 1000;
    => dirX*31*31*31 + dirY*31*31 + x*31 + y; //+ (target==null? 16 : 0);

    bool Move(Vector2 dir){
        position += dir;
        direction = dir;
        return !ground.IsObstructed(position);
    }

    [Serializable] public class Target{

        public int x, y;

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

    public void Print(object x){
        UnityEngine.Debug.Log(x);
    }

    override public string ToString(){
        return $"M[{x}, {y} to {dirX}, {dirY}]";
    }

}
