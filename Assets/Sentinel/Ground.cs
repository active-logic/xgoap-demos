using UnityEngine;

public class Ground : MonoBehaviour{

    public GroundModel model;

    void Awake(){
        model = new GroundModel();
        int n = 0;
        for(int x = 0; x < 11; x++){
            for(int z = 0; z < 11; z++){
                if(model.map[n++] == 1) continue;
                var cube =
                    GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position =
                    new Vector3(x - 5, -0.5f, z - 5);
                cube.transform.localScale = Vector3.one * 0.95f;
            }
        }
    }

}
