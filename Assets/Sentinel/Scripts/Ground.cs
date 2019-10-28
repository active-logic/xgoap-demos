using UnityEngine;
using System.Collections.Generic;
using Ex = System.Exception;

public class Ground : MonoBehaviour{

    public GroundModel model;
    public GameObject propPrefab;
    public List<GameObject> props;
    public bool useProps = true;

    void Awake(){
        model = new GroundModel();
        int n = 0;
        if(!useProps) model.ClearProps();
        for(int x = 0; x < 11; x++){
            for(int z = 0; z < 11; z++){
                switch(model[n++]){
                    case 0: AddBlock(x, z); break;
                    case 1: /* no-op */     break;
                    default: throw new Ex($"Bad val: {model[n-1]}");
                }
            }
        }
        foreach(var k in model.props) AddProp(k);
    }

    void AddBlock(int x, int z){
        var cube =
            GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position =
            new Vector3(x - 5, -0.5f, z - 5);
        cube.transform.localScale = Vector3.one * 0.95f;
    }

    void AddProp(Vector2i pos){
        var prop = Instantiate(propPrefab).transform;
        prop.position =
            new Vector3(pos.x, 0.5f, pos.y);
        props.Add(prop.gameObject);
    }

    public GameObject WillMoveProp(Transform bot){
        var B = bot.position;
        var P = bot.position + bot.forward;
        foreach(var k in props){
            var u = P - k.transform.position; u.y = 0;
            if(u.magnitude < 0.1){
                model.MoveProp((P.x, P.z), to: (B.x, B.z));
                return k;
            }
        }
        return null;
    }

}
