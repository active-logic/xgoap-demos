using UnityEngine;
using System.Collections.Generic;

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
                    case 2: AddBlock(x, z); AddProp(x, z); break;
                }
            }
        }
    }

    void AddBlock(int x, int z){
        var cube =
            GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position =
            new Vector3(x - 5, -0.5f, z - 5);
        cube.transform.localScale = Vector3.one * 0.95f;
    }

    void AddProp(int x, int z){
        var prop = Instantiate(propPrefab).transform;
        prop.position =
            new Vector3(x - 5, 0.5f, z - 5);
        props.Add(prop.gameObject);
    }

    public GameObject WillMoveProp(Transform bot){
        var B = bot.position;
        var P = bot.position + bot.forward;
        foreach(var k in props){
            var d = Vector3.Distance(P, k.transform.position);
            if(d < 0.1){
                var here = new Vector2(B.x, B.z);
                var ahead = new Vector2(P.x, P.z);
                model.Set(ahead, 0);
                model.Set(here, 2);
                return k;
            }
        }
        return null;
    }

}
