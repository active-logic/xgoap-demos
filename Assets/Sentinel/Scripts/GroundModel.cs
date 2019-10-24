using System;
using System.Collections.Generic;
//using System.Linq;
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

    public bool IsObstructed(Vector2i pos){
        if(pos.x < -5 || pos.x > 5) return true;
        if(pos.y < -5 || pos.y > 5) return true;
        var w = pos + 5;
        return (this[w.x, w.y] != 0) || IsProp(pos);

    }

    public bool IsPropNearby(Vector2i P){
        return IsProp(P + Vector2i.right)
            || IsProp(P + Vector2i.left)
            || IsProp(P + Vector2i.up)
            || IsProp(P + Vector2i.down);
    }

    // Pull the prop at index to 'position' and return the vector
    // matching the vector matching this move.
    public Vector2i PullProp(int propIndex, Vector2i position){
        var p0 = props[propIndex];
        props[propIndex] = position;
        return position - p0;
    }

    public int PullablePropIndex(Vector2i position, Vector2i dir){
        if(IsObstructed(position-dir)) return -1;
        var P = (Vector2i)(position + dir);
        for(int i = 0; i < props.Length; i++)
            if(P.Eq(props[i])) return i;
        return -1;
    }

    public bool IsProp(Vector2i atPos){
        foreach(var p in props) if(p.Eq(atPos)) return true;
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
        throw new System.Exception(
                    $"Could not move prop: {src}, {(Vector2i)src}");
    }

    override public int GetHashCode(){
        var c = 0;
        for(int i = 0; i < props.Length; i++){
            c = c * 31 + props[i].GetHashCode();
        } return c;
    }

    // NOTE:
    // - Map is planning-static, ignore
    // - By design number of props won't change
    public bool IsEqual(GroundModel that){
        for(int i = 0; i < props.Length; i++)
            if(props[i].UnEq(that.props[i])) return false;
        return true;
    }

}
