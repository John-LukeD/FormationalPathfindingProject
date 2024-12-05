using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    private float moveSpeed;
    private float radiusOfSatisfaction;
    //tranform of character
    [SerializeField] private Transform myTransform;
    //transform of target (user left click)
    [SerializeField] private Transform targetTransform;
    // Reference to the plane's collider to get target transform based off RaycastHit
    [SerializeField] private Collider planeCollider;
    //declare MinHeap of type node for our openList
    private static MinHeap<Node> minHeap = new MinHeap<Node>();
    //declare LinkedList of type node for our closedList
    private static LinkedList<Node> closedList = new LinkedList<Node>();
    // List to store the final path in reverse order.
    public static List<Node> reverseOrder = new List<Node>(); 

    //Variables
    private static int startCol;
    private static int startRow;
    private static int goalCol;
    private static int goalRow;
    private static Node currNode;
    //private static Node startNode;
    //private static Node goalNode;
    //private static Node tempNode;

    // Track the current target index -> this is for tarcing the path back
    // Using kinematicMovement to arrive at each node within reverseOrder List (path from A*)
    private int currentTargetIndex = 0; 

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 7f;
        radiusOfSatisfaction = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

        // check for user input to update the targetTransform based on click
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            // create a ray from the camera to the mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // store where the raycast hit
            RaycastHit hit;

            // check if the ray intersects with the plane's collider
            if (planeCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                reverseOrder.Clear();
                minHeap.Clear();
                closedList.Clear();
                currentTargetIndex = 0;
                // set intersection point as the target position
                targetTransform.position = hit.point;

                // Get startNode and GoalNode and pass it to RunAStarAlgorithm
                startCol = (int)(myTransform.position.x);
                startRow = (int)(myTransform.position.z);
                goalCol = (int)(targetTransform.position.x);
                goalRow = (int)(targetTransform.position.z);
                //Debug.Log("startx:" + startCol);
                //Debug.Log("startZ:" + startRow);
                //Debug.Log("goalx:" + goalCol);
                //Debug.Log("goalZ:" + goalRow);

                RunAStarAlgorithm();
            }
        }
        RunKinematicArrive();
    }

    private void RunAStarAlgorithm() 
    {
        // add the start node to the open list (MinHeap)
        minHeap.Add(CirclePlacer.worldData[startRow, startCol]);
        // currNode = startNode
        currNode = CirclePlacer.worldData[startRow, startCol];
        // everything run inside loop below
        while ((currNode.GetCol() != goalCol && currNode.GetRow() != goalRow) || minHeap.size != 0 )
        {
            // 1) Pop off node from min heap (automatically the node with the lowest F) and set as currNode
            currNode = minHeap.Remove();
            // 2) Check if currNode is goalNode
            if (currNode.GetCol() == goalCol && currNode.GetRow() == goalRow)
            {
                // a) if yes {break out of while loop - generate path back from parents } if no go to b
                // Reverse order to get the path from start to goal
                while (currNode != null) 
                {
                    //Debug.Log(currNode);
                    reverseOrder.Insert(0, currNode); // Add to the front of the list
                    currNode = currNode.GetParent();
                }
                break;

            }
            else
            {
                // b) if no Generate neighbors
                generateNeighbors(currNode);
                // Add currNode to closed list
                closedList.AddFirst(currNode);
                // Go back to step 1
            }
        }
    }

    // generate the 9 tiles that we can reach from here and set F, G, H, and Parent values. 
    // A neighbor is invalid if it is outside the bounds, the node is an obstacle, or in closedList
    public static void generateNeighbors(Node node) {
        // Check all neighboring nodes in a 3x3 grid around the current node (including diagonals)
        for (int i = node.GetRow() - 1; i <= node.GetRow() + 1; i++) {
            for (int j = node.GetCol() - 1; j <= node.GetCol() + 1; j++) {
                // Skip the current node itself
                if (i == node.GetRow() && j == node.GetCol()) {
                    continue;
                }

                // Check if the neighbor is within the bounds of the grid and is traversable
                if ((i >= 0 && i < 50) && (j >= 0 && j < 50) && (CirclePlacer.worldData[i,j].GetNodeType() == 0)) 
                    {
                    // Skip if the node is already in the closed list
                    if (closedList.Contains(CirclePlacer.worldData[i,j])) {
                        continue;
                    }

                    // Set the G value (G represents the cost from the start node to this node 
                    // 10 for non-diagonal, 14 for diagonal moves)
                    if (i == goalRow || j == goalCol) {
                        CirclePlacer.worldData[i,j].SetG(10);
                    } else {
                        CirclePlacer.worldData[i,j].SetG(14);
                    }

                    // Set the H value (represents the estimated cost to the goal) using Manhattan distance
                    int x = Math.Abs(i - goalRow);
                    int y = Math.Abs(j - goalCol);
                    CirclePlacer.worldData[i,j].SetH((x + y) * 10);
                    // Calculate and set the F value
                    CirclePlacer.worldData[i,j].SetF();
                    // Set the parent to the current node
                    CirclePlacer.worldData[i,j].SetParent(CirclePlacer.worldData[node.GetRow(),node.GetCol()]);

                    // Add the generated neighbors to the open list as they are discovered
                    minHeap.Add(CirclePlacer.worldData[i,j]);
                }
            }
        }
    }

    private void RunKinematicArrive()
    {
        if (currentTargetIndex >= reverseOrder.Count)
        {
            // Path traversal is complete
            return;
        }

        Node targetNode = reverseOrder[currentTargetIndex];
        Vector3 targetPosition = new Vector3((float)targetNode.GetCol(), targetTransform.position.y, (float)targetNode.GetRow());
        Debug.Log(targetPosition.x);
        Debug.Log(targetPosition.z);

        // Move toward the current target node
        Vector3 towardsTarget = targetPosition - myTransform.position;

        if (towardsTarget.magnitude <= radiusOfSatisfaction)
        {
            // Close enough to the target, move to the next node
            currentTargetIndex++;
            return;
        }

        // Normalize vector and move toward the target
        towardsTarget = towardsTarget.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(towardsTarget);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, 0.1f);
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
    }
}
