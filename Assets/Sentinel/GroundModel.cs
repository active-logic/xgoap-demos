using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Activ.GOAP;

[Serializable]
public class GroundModel : Clonable{

    public Vector2i[] props;
    int[] map;

    public object Clone()
    => new GroundModel(map, (Vector2i[])props.Clone());

    public GroundModel(){
        map = DefaultMap.data;
        var list = new List<Vector2i>();
        for(int x = 0; x < 11; x++){
            for(int z = 0; z < 11; z++){
                if(this[x, z] == 2) {
                    this[x, z] = 0;
                    list.Add(new Vector2i(x - 5, z - 5));
                }
            }
        } props = list.ToArray();
    }

    GroundModel(int[] map, Vector2i[] props){
        this.map = map;
        this.props = props;
    }

    public int this[int i] => map[i];

    public int this[int x, int z]{
        get => map[x * 11 + z];
        set => map[x * 11 + z] = value;
    }

    public void Clear() => map = new int[map.Length];

    public void ClearProps() => props = new Vector2i[0];

    public bool IsObstructed(Vector2 q){
        var w = q + Vector2.one * 5;
        int x = (int)w.x, y = (int)w.y;
        if(x < 0 || x > 10) return true;
        if(y < 0 || y > 10) return true;
        return (this[x, y] != 0) || IsProp(q);
;
    }

    public bool IsProp(Vector2 atPos){
        foreach(var p in props) if(p == atPos) return true;
        return false;
    }

    public void MoveProp(Vector2 src, Vector2 dst){
        for(int i = 0; i < props.Length; i++){
            if(props[i] == src){
                props[i] = (Vector2i)dst;
                return;
            }
        }
        throw new System.Exception("Could not move prop");
    }

    override public int GetHashCode()
    => map.Aggregate(0, (x, y) => x + y);

    // NOTE:
    // - Map is planning-static, ignore
    // - By design number of props won't change
    public bool IsEqual(GroundModel that){
        for(int i = 0; i < props.Length; i++)
            if(props[i].UnEq(that.props[i])) return false;
        return true;
    }

}
