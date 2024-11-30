using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//including using radius of satusfaction and range of speed
public class NewBehaviourScript : MonoBehaviour
{

    private float moveSpeed;
    private float radiusOfSatisfaction;
    //tranform of character
    [SerializeField] private Transform myTransform;
    //transform of target (user left click)
    [SerializeField] private Transform targetTransform;
    // Reference to the plane's collider
    [SerializeField] private Collider planeCollider;
    //declare minHeap
    MinHeap<Node> minHeap = new MinHeap<Node>();

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 7f;
        radiusOfSatisfaction = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

        //check for user input to update the targetTransform based on click
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            //create a ray from the camera to the mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //store where the raycast hit
            RaycastHit hit;

            //check if the ray intersects with the plane's collider
            if (planeCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                //set intersection point as the target position
                targetTransform.position = hit.point;
            }
        }

        RunKinematicArrive();
    }

    private void RunAStarAlgorithm () {
        //add the start node to the open list (MinHeap)
        //currNode = startNode
        //everything run inside loop below
        //while (currNode != goalNode || openlist.size !=0 )
        //1)	Pop off node from min heap (automatically the node with the lowest F) and set as currNode
        //2)	Check if currNode is goalNode 
        //a) if yes {break - generate path back from parents} if no go to b
        //b)  if no Generate neighbors (all available moves that we can make from here 
        //(if you don’t want diagonal movement don’t generate diagonal tiles) do a 
        //nested for loop to generate the 9 tiles and set F, G, H, and Parent values. 
        //A neighbor is invalid if it is outside the bounds of the environment, if the node 
        //is an obstacle, 
        //3)	Add all generates neighbors to open list as they have been discovered
        //4)	Add currNode to closed list
        //5)	Go back to step 1
        
    }

    private void RunKinematicArrive () {

        //Create vector from character to target
        Vector3 towardsTarget = targetTransform.position - myTransform.position;

        //Check if the character is close enough to the target
        if (towardsTarget.magnitude <= radiusOfSatisfaction) {
            //close enough to stop!
            return;
        }

        // Normalize vector to only use the direction (shrink vector to length of 1)
        towardsTarget = towardsTarget.normalized;

        //face the target
        Quaternion targetRoatation = Quaternion.LookRotation (towardsTarget);
        myTransform.rotation = Quaternion.Lerp (myTransform.rotation, targetRoatation, 0.1f);

        //Move along our vector (the direction we're facing)
        Vector3 newPosition = myTransform.position;
        //add forward vector of the charcter (direction character is facing)
        //* the moveSpeed * regular time rather then time between update calls
        newPosition += myTransform.forward * moveSpeed * Time.deltaTime;
        myTransform.position = newPosition;
        
    }
}
