using System;
using UnityEngine;

[Serializable]
public class GroundModel{

    public int[] map = new int[]{
        0,0,0,0,0, 0, 0,0,0,0,0,
        0,0,0,0,0, 0, 0,0,0,0,0,
        0,0,0,0,0, 0, 0,0,0,0,0,
        0,0,0,0,0, 1, 0,0,0,0,0,
        0,0,1,1,1, 1, 1,1,1,0,0,
        //
        0,0,1,1,1, 0, 1,0,0,0,0,
        //
        0,0,1,1,0, 0, 0,0,0,0,0,
        0,0,0,0,0, 0, 0,0,0,0,0,
        0,0,1,0,0, 0, 0,0,0,0,0,
        0,0,1,0,0, 0, 0,0,0,0,0,
        0,0,1,0,0, 0, 0,0,0,0,0,
    };

    public void Clear()
    { for(int i = 0; i < map.Length; i++) map[i] = 0; }

    public bool IsObstructed(Vector2 p){
        p += Vector2.one * 5;
        int x = (int)p.x, y = (int)p.y;
        return map[x * 11 + y] == 1;
    }

}
