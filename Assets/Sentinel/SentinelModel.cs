using System;
using UnityEngine;
using Activ.GOAP;
using static UnityEngine.Vector2;

[Serializable] public class SentinelModel : Agent{

    const int range = 3;
    public int x, y;
    public int dirX, dirY;
    public Target target;
    // The ground model is static so don't serialize it
    GroundModel ground;

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
        ground.Set(ahead, 0);
        ground.Set(here, 2);
        position = behind;
        return 1;
    }

    public Cost Shoot(){
        if(target.Dist(x, y) > range) return false;
        return (target = null, 1);
    }

    override public bool Equals(object other){
        var that = other as SentinelModel;
        return this.x == that.x
            && this.y == that.y
            && this.dirX == that.dirX
            && this.dirY == that.dirY
            && this.target.Equals(that.target)
            && this.ground.Equals(that.ground);
    }

    override public int GetHashCode()
    //=> dirX + dirY * 1000;
    => dirX + dirY + x*400 + y*800 + (target==null? 16 : 0);

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
            var that = other as Target;
            return this.x == that.x && this.y == that.y;
        }

        override public int GetHashCode() => x*100 + y;

    }

    public void Print(object x){
        UnityEngine.Debug.Log(x);
    }

    override public string ToString(){
        return $"M[{x}, {y} to {dirX}, {dirY}]";
    }

}
