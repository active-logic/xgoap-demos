using System;
using UnityEngine;
using Activ.GOAP;

[Serializable]
public class SentinelModel : Agent{

    public int x, y;

    public Vector2 position{
        get => new Vector2(x, y);
        set{ x = (int)value.x; y = (int)value.y; }
    }

    public SentinelModel(Transform t)
    => position = new Vector2((int)t.position.x, (int)t.position.z);

    public Func<Cost>[] actions => new Func<Cost>[]
    { MoveLeft, MoveRight, MoveForward, MoveBack };

    public Cost MoveLeft    () => (position += Vector2.left, 1);
    public Cost MoveRight   () => (position += Vector2.right, 1);
    public Cost MoveBack    () => (position += Vector2.down, 1);
    public Cost MoveForward () => (position += Vector2.up, 1);

    override public bool Equals(object other){
        var that = other as SentinelModel;
        return this.x == that.x && this.y == that.y;
    }

    override public int GetHashCode() => x + y * 1000;

}
