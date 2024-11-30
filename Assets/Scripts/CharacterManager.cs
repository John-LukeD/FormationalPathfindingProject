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
    private float obstacleBumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 7f;
        radiusOfSatisfaction = 1.5f;
        obstacleBumpSpeed = 0.1f;
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

    private void OnCollisionEnter (Collision collision) {

		if (collision.gameObject.tag != "Sphere"){
			return;

		}

		// Calcualte vector from player to obstacle
		Vector3 toObstacle = (collision.gameObject.transform.position - myTransform.position).normalized;
		toObstacle.Normalize ();

        //I set 
		toObstacle.y = 0f;

		//Debug.DrawRay (trans.position + Vector3.up, toObstacle, Color.yellow);
		//Debug.DrawRay (trans.position + Vector3.up, trans.right, Color.cyan);

		float dot = Vector3.Dot (myTransform.right, toObstacle);
		

		//push character away from obstacle
		myTransform.position -= toObstacle * .02f;

		// Obstacle is on the left of the obstacle -> push player right
		if (dot < 0) {
			myTransform.position += myTransform.right * obstacleBumpSpeed;
		} else {
			myTransform.position += myTransform.right * -1f * obstacleBumpSpeed;
		}

	}
}
