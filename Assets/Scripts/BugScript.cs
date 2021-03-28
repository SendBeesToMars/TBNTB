using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour
{
    private enum State{
        Idle,
        MovingToResource,
        MovingToBase,
        Gathering,
        Depositing,
    }
    [SerializeField]
    private State state;
    public float speed;
    public int inventory = 0;
    public int inventoryMax = 1;
    public float stoppingDistance = 0.25f;

    bool canGather = false;
    bool canMove = true;

    Vector2 direction = new Vector2(1, 0).normalized;
    GameObject resourceNode;
    Transform resourceNodeTransform;
    NodeScript resourceNodeScript;
    GameObject homeBase;
    Transform homeBaseTransform;
    BaseScript homeBaseScript;

    
    private void Start() {
        state = State.Idle;
        // get resource node
        resourceNode = GameObject.FindGameObjectWithTag("Resource");
        resourceNodeTransform = resourceNode.GetComponent<Transform>();
        resourceNodeScript = resourceNode.GetComponent<NodeScript>();

        homeBase = GameObject.FindGameObjectWithTag("Base");
        homeBaseTransform = homeBase.GetComponent<Transform>();
        homeBaseScript = homeBase.GetComponent<BaseScript>();
    }
    private void Update() {

        switch (state){
            case State.Idle:
                resourceNode = GameObject.FindGameObjectWithTag("Resource");
                resourceNodeTransform = resourceNode.GetComponent<Transform>();
                resourceNodeScript = resourceNode.GetComponent<NodeScript>();
                state = State.MovingToResource;
                break;
            case State.MovingToResource:
                if (!canMove) break;
                moveTo(resourceNodeTransform, 0.25f, () => {
                    canGather = true;
                    state = State.Gathering;
                });
                break;
            case State.Gathering:
                canMove = false;
                if (canGather) StartCoroutine(harvest( (inventoryMax - inventory) > (resourceNodeScript.resources) ? resourceNodeScript.resources : (inventoryMax - inventory)));
                state = State.MovingToBase;
                break;
            case State.MovingToBase:
                if (!canMove) break;
                canGather = false;
                moveTo(homeBaseTransform, 0.4f, () => {
                    state = State.Depositing;
                });
                break;
            case State.Depositing:
                canMove = false;
                StartCoroutine(deposit(inventory));
                state = State.Idle;
                break;

        }
        // if node is destroyed look for new one
        // if (resourceNode == null)
        // {
        //     resourceNode = GameObject.FindGameObjectWithTag("Node");
        //     resourceNodeTransform = resourceNode.GetComponent<Transform>();
        //     resourceNodeScript = resourceNode.GetComponent<NodeScript>();
        // }
        // // if distance from node to bug is short, stop, else move twards resource node
        // if (canMove && Vector2.Distance(transform.position, resourceNodeTransform.position) >= stoppingDistance && !(inventory >= inventoryMax))
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, resourceNodeTransform.position, speed * Time.deltaTime);
        // }
        // else if(canMove && Vector2.Distance(transform.position, homeBaseTransform.position) >= stoppingDistance && inventory >= inventoryMax) {
        //     transform.position = Vector2.MoveTowards(transform.position, homeBaseTransform.position, speed * Time.deltaTime);
        // }
        // else if (inventory != inventoryMax && Vector2.Distance(transform.position, resourceNodeTransform.position) <= stoppingDistance)
        // {
        //     canMove = false;
        //     StartCoroutine(harvest(inventoryMax - inventory));
        // }
        // else if (inventory != 0 && Vector2.Distance(transform.position, homeBaseTransform.position) <= stoppingDistance)
        // {
        //     canMove = false;
        //     StartCoroutine(deposit(inventory));
        // }
    }

    private void moveTo(Transform destination, float stopDistance, System.Action arrivedAtPosition){
        if (Vector2.Distance(transform.position, destination.position) > stopDistance){
            transform.position = Vector2.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
        }
        else{
            arrivedAtPosition.Invoke();
        }
    }

    IEnumerator harvest(int resourceNum){
        if (resourceNodeScript.resources >= resourceNum){
            resourceNodeScript.resources -= resourceNum;
            inventory += resourceNum;
        }
        else{
            inventory += resourceNodeScript.resources;
            resourceNodeScript.resources = 0;
        }
        yield return new WaitForSeconds((float)resourceNum/2);
        canMove = true;
    }

    IEnumerator deposit(int resourceNum){
        homeBaseScript.resources += resourceNum;
        inventory -= resourceNum;
        yield return new WaitForSeconds((float)resourceNum/2);
        canMove = true;
    }
}
