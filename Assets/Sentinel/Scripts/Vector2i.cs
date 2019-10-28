using UnityEngine;
using static UnityEngine.Mathf;
using System;

public readonly struct Vector2i{

    public static readonly Vector2i right = (1, 0), left = (-1, 0),
                                    up = (0, 1),    down = (0, -1);
    public readonly int x, y;

    public Vector2i(int x, int y){ this.x = x; this.y = y; }

    public Vector2i(float x, float y)
    { this.x = RoundToInt(x); this.y = RoundToInt(y); }

    public static implicit operator Vector2i(ValueTuple<int, int> P)
    => new Vector2i(P.Item1, P.Item2);

    public static implicit operator Vector2i(ValueTuple<float, float> P)
    => new Vector2i(P.Item1, P.Item2);

    public static explicit operator Vector2i(Vector2 v)
    => new Vector2i(RoundToInt(v.x), RoundToInt(v.y));

    public static explicit operator Vector2(Vector2i v)
    => new Vector2(v.x, v.y);

    public static Vector2i operator + (Vector2i self, int n)
    => new Vector2i(self.x + n, self.y + n);

    public static Vector2i operator + (Vector2i P, Vector2i Q)
    => new Vector2i(P.x + Q.x, P.y + Q.y);

    public static Vector2i operator - (Vector2i P, Vector2i Q)
    => new Vector2i(P.x - Q.x, P.y - Q.y);

    public static bool operator == (Vector2i self, object that){
        switch(that){
            case Vector2i a:
                return self.x == a.x && self.y == a.y;
            case Vector2 b:
                return Vector2.Distance((Vector2)self, b) < 0.1f;
            default: return false;
        }
    }

    public static bool operator != (Vector2i self, object that){
        switch(that){
            case Vector2i a:
                return self.x != a.x || self.y != a.y;
            case Vector2 b:
                //ebug.Log($"compare {self} and {b}");
                return self.x != b.x || self.y != b.y;
            default: return false;
        }
    }

    public static float Distance(Vector2i P, Vector2i Q){
        int a = P.x - Q.x, b = P.y - Q.y;
        return Mathf.Sqrt(a * a + b * b);
    }

    public bool Eq(Vector2i that) => x == that.x && y == that.y;

    public bool UnEq(in Vector2i that)
    => x != that.x || y != that.y;

    override public bool Equals(object other) => this == other;

    override public int GetHashCode() => x * 31 + y;

    override public string ToString() => $"({x}, {y})";

}
