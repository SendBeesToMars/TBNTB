using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour
{

    public float speed;

    public int inventory = 0;
    public int inventoryMax = 1;
    public float stoppingDistance = 0.25f;

    Vector2 direction = new Vector2(1, 0).normalized;
    GameObject resourceNode;
    Transform resourceNodeTransform;
    NodeScript resourceNodeScript;
    GameObject homeBase;
    Transform homeBaseTransform;
    BaseScript homeBaseScript;

    
    private void Start() {
        // get resource node
        resourceNode = GameObject.FindGameObjectWithTag("Node");
        resourceNodeTransform = resourceNode.GetComponent<Transform>();
        resourceNodeScript = resourceNode.GetComponent<NodeScript>();

        homeBase = GameObject.FindGameObjectWithTag("Base");
        homeBaseTransform = homeBase.GetComponent<Transform>();
        homeBaseScript = homeBase.GetComponent<BaseScript>();
        
    }
    private void Update() {
        // if distance from node to bug is short, stop, else move twards resource node
        if (Vector2.Distance(transform.position, resourceNodeTransform.position) > stoppingDistance && inventory < inventoryMax)
        {
            transform.position = Vector2.MoveTowards(transform.position, resourceNodeTransform.position, speed * Time.deltaTime);
        }
        else if(Vector2.Distance(transform.position, homeBaseTransform.position) > stoppingDistance && inventory == inventoryMax) {
            transform.position = Vector2.MoveTowards(transform.position, homeBaseTransform.position, speed * Time.deltaTime);
        }
        else{
            if (inventory != inventoryMax)
            {
                while(inventory != inventoryMax){
                resourceNodeScript.resources--;
                inventory++;
                }
            }
            else if (inventory != 0)
            {
                while(inventory != 0){
                inventory--;
                homeBaseScript.resources++;
                }
            }

        }
    }
}
