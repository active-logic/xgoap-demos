using UnityEngine;

[System.Serializable] public struct Vector2i{

    public int x, y;

    public Vector2i(int x, int y){
        this.x = x; this.y = y;
    }

    public static explicit operator Vector2i(Vector2 v)
    => new Vector2i((int)v.x, (int)v.y);

    public bool UnEq(Vector2i that) => x != that.x || y != that.y;

    public static bool operator == (Vector2i self, object that){
        switch(that){
            case Vector2i a:
                return self.x == a.x && self.y == a.y;
            case Vector2 b:
                //ebug.Log($"compare {self} and {b}");
                return self.x == b.x && self.y == b.y;
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
