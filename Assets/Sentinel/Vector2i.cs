using UnityEngine;
using static UnityEngine.Mathf;

[System.Serializable] public readonly struct Vector2i{

    public readonly int x, y;

    public Vector2i(float x, float y){
        this.x = RoundToInt(x); this.y = RoundToInt(y);
    }

    public Vector2i(int x, int y){
        this.x = x; this.y = y;
    }

    public static explicit operator Vector2i(Vector2 v)
    => new Vector2i(RoundToInt(v.x), RoundToInt(v.y));

    public static explicit operator Vector2(Vector2i v)
    => new Vector2(v.x, v.y);

    public bool Eq(Vector2i that) => x == that.x && y == that.y;

    public bool UnEq(Vector2i that) => x != that.x || y != that.y;

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

    override public bool Equals(object other) => this == other;

    override public int GetHashCode() => x * 31 + y;

    override public string ToString() => $"({x}, {y})";

}
