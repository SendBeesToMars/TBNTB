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

    bool canMove = true;

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
        // if node is destroyed look for new one
        if (resourceNode == null)
        {
            resourceNode = GameObject.FindGameObjectWithTag("Node");
            resourceNodeTransform = resourceNode.GetComponent<Transform>();
            resourceNodeScript = resourceNode.GetComponent<NodeScript>();
        }
        // if distance from node to bug is short, stop, else move twards resource node
        if (canMove && Vector2.Distance(transform.position, resourceNodeTransform.position) >= stoppingDistance && !(inventory >= inventoryMax))
        {
            transform.position = Vector2.MoveTowards(transform.position, resourceNodeTransform.position, speed * Time.deltaTime);
        }
        else if(canMove && Vector2.Distance(transform.position, homeBaseTransform.position) >= stoppingDistance && inventory >= inventoryMax) {
            transform.position = Vector2.MoveTowards(transform.position, homeBaseTransform.position, speed * Time.deltaTime);
        }
        else if (inventory != inventoryMax && Vector2.Distance(transform.position, resourceNodeTransform.position) <= stoppingDistance)
        {
            canMove = false;
            StartCoroutine(harvest(inventoryMax - inventory));
        }
        else if (inventory != 0 && Vector2.Distance(transform.position, homeBaseTransform.position) <= stoppingDistance)
        {
            canMove = false;
            StartCoroutine(deposit(inventory));
        }
    }

    IEnumerator harvest(int resourceNum){
        resourceNodeScript.resources -= resourceNum;
        inventory += resourceNum;
        yield return new WaitForSeconds(resourceNum/2);
        canMove = true;
    }

    IEnumerator deposit(int resourceNum){
        homeBaseScript.resources += resourceNum;
        inventory -= resourceNum;
        yield return new WaitForSeconds(resourceNum/2);
        canMove = true;
    }
}
