using UnityEngine;

public class Ground : MonoBehaviour{

    public GroundModel model;
    public GameObject propPrefab;

    void Awake(){
        model = new GroundModel();
        int n = 0;
        for(int x = 0; x < 11; x++){
            for(int z = 0; z < 11; z++){
                switch(model.map[n++]){
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
    }

}
