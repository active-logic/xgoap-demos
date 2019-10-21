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
        int x = Mathf.RoundToInt(w.x),
            y = Mathf.RoundToInt(w.y);
        //
        var d = Vector2.Distance(w, new Vector2(x, y));
        if(d > 0.1) Debug.LogError("bad conversion");
        //
        if(x < 0 || x > 10) return true;
        if(y < 0 || y > 10) return true;
        if(q.x < -5.5)Debug.LogError("This should be obstructed: " + x+", "+y);
        return (this[x, y] != 0) || IsProp(q);

    }

    public bool IsPropNearby(Vector2 q){
        return IsProp(q + Vector2.right)
            || IsProp(q + Vector2.left)
            || IsProp(q + Vector2.up)
            || IsProp(q + Vector2.down);
    }

    public bool IsProp(Vector2 atPos){
        foreach(var p in props) if(p == atPos) return true;
        return false;
    }

    public void MoveProp(Vector2 src, Vector2 dst){
        for(int i = 0; i < props.Length; i++){
            var d = Vector2.Distance((Vector2)props[i], src);
            if(d < 0.1f){
                props[i] = (Vector2i)dst;
                return;
            }
        }
        for(int i = 0; i < props.Length; i++){
            Debug.Log($"Prop {i} - {props[i]}");
        }
        throw new System.Exception($"Could not move prop: {src}, {(Vector2i)src}");
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
