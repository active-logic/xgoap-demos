using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class GroundModel{

    public int[] map = new int[]{
        1,1,0,0,0, 0, 2,0,0,0,0,
        0,0,0,1,1, 1, 1,0,0,0,0,
        0,0,0,1,0, 1, 1,1,1,0,1,
        1,0,0,0,0, 1, 1,1,0,1,1,
        1,2,1,1,1, 1, 1,1,1,0,0,
        //
        2,0,1,1,1, 0, 1,0,0,0,0,
        //
        0,0,1,0,0, 0, 1,0,0,0,0,
        0,0,2,0,0, 0, 1,0,0,0,0,
        0,0,1,1,1, 1, 1,0,0,0,0,
        0,0,1,0,0, 0, 0,0,0,0,0,
        0,0,1,0,0, 0, 0,0,0,0,0,
    };

    public void Clear()
    { for(int i = 0; i < map.Length; i++) map[i] = 0; }

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

    override public bool Equals(object other){
        var that = other as GroundModel;
        return this.map.SequenceEqual(that.map);
    }

}
