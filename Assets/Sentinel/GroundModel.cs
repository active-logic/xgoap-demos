using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class GroundModel{

    int[] map;
    
    public GroundModel(){
        map = DefaultMap.data;
    }

    public int this[int i] => map[i];

    public void Clear() => map = new int[map.Length];

    public void ClearProps() => map.Replace(2, with: 0);

    public bool IsObstructed(Vector2 p){
        p += Vector2.one * 5;
        int x = (int)p.x, y = (int)p.y;
        if(x < 0 || x > 10) return true;
        if(y < 0 || y > 10) return true;
        return map[x * 11 + y] != 0;
    }

    public bool IsProp(Vector2 p){
        try{
            p += Vector2.one * 5;
            int x = (int)p.x, y = (int)p.y;
            return map[x * 11 + y] == 2;
        }catch(System.IndexOutOfRangeException){
            return false;
        }
    }

    public void Set(Vector2 p, int val){
        p += Vector2.one * 5;
        int x = (int)p.x, y = (int)p.y;
        map[x * 11 + y] = val;
    }

    override public int GetHashCode()
    => map.Aggregate(0, (x, y) => x + y);

    override public bool Equals(object other){
        var that = other as GroundModel;
        return this.map.SequenceEqual(that.map);
    }

}
