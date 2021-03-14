using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{

    public int resources = 3;

    public GameObject node;

    public Vector2 size;
    // Start is called before the first frame update

    // Update is called once per frame

    void Update()
    {
        if (resources == 0)
        {
            spawnNewNode();
        }
    }

    void spawnNewNode(){
        Vector2 pos = new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
        resources = 3;
        Instantiate( node, pos, Quaternion.identity);
        Destroy(gameObject);
    }
}
