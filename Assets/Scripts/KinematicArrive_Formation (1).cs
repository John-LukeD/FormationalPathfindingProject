using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive_Formation : MonoBehaviour
{

	[SerializeField] private Transform targetTransform;
	[SerializeField] private Transform myTransform;
	[SerializeField] private Rigidbody rb;

	[SerializeField] private float distanceOffset;
	[SerializeField] private float angleOffset;
	private Quaternion rotationOffset;

	private float radiusOfSatisfaction;
	private float moveSpeed;
	private float turnSpeed;
	private float obstacleBumpSpeed;

	private void Start () {
		radiusOfSatisfaction = 0.5f;
		moveSpeed = 7f;
		turnSpeed = 7f;
		obstacleBumpSpeed = 0.1f;

		// Convert angle offset to a quaternion
		rotationOffset = Quaternion.Euler (0f, angleOffset, 0f);
	}

	// Update is called once per frame
	void Update()
    {

		RunKinematicArrive ();
    }

	private void OnCollisionEnter (Collision collision) {

		if (collision.gameObject.tag != "Sphere"){
			return;

		}

		// Calcualte vector from player to obstacle
		Vector3 toObstacle = (collision.gameObject.transform.position - myTransform.position).normalized;
		toObstacle.Normalize ();
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

	private void RunKinematicArrive () {

		Vector3 targetPosition = targetTransform.position;

		// Extend target's forward vector equal to our distance offset
		Vector3 forward = targetTransform.forward * distanceOffset;

		// Rotate vector to find the position in the formation
		targetPosition += rotationOffset * forward;

		// Calculate vector towards our target position
		Vector3 towards = targetPosition - myTransform.position;

		// Face the direction we're moving
		Vector3 forwardDirection = targetTransform.forward;
		Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
		myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);


		// Don't move the character if they're close enough to their target
		if (towards.magnitude < radiusOfSatisfaction) {
			rb.velocity = Vector3.zero;
			return;
		}

		// Normalize towards to set it's length/magnitude to 1
		towards.Normalize ();

		towards *= moveSpeed;

		rb.velocity = towards;
		
	}

}
