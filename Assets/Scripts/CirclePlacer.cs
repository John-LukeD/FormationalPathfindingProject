using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlacer : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Collider planeCollider;

    //WorldDecomposer variables below
    public static Node [,] worldData;
	private int nodeSize;

	private int terrainWidth;
	private int terrainLength;

	private int rows;
	private int cols;


    //Start is called before the first frame update
    void Start()
    {
        //Find the plane by its tag, or directly by finding the collider in the scene
        planeCollider = GameObject.FindGameObjectWithTag("Plane").GetComponent<Collider>();
        
        if (planeCollider == null)
        {
            Debug.LogError("Plane Collider not found! Make sure the plane has a tag 'Plane' and a Collider attached.");
        }

        //WorldDecomposer below
        terrainWidth = 50;
		terrainLength = 50;

		nodeSize = 1;

		rows = terrainWidth / nodeSize;
		cols = terrainLength / nodeSize;

		worldData = new Node [rows, cols];
    }

    // Update is called once per frame
    void Update()
    {
        //Check for right mouse button click (1 represents right click)
        if (Input.GetMouseButtonDown(1))
        {
            //Create a ray from the camera to the mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Check if the ray intersects with the plane's collider
            if (planeCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                //Get the position where the user clicked
                Vector3 spawnPosition = hit.point;
                spawnPosition.y = .5f;

                //Instantiate the circle prefab at the clicked position
                Instantiate(circlePrefab, spawnPosition, Quaternion.identity);
                
                //If a new circle is placed, Decompose world
                DecomposeWorld ();
            }
        }
    }

    void DecomposeWorld () {

		float startX = 0;
		float startZ = 0;

		float nodeCenterOffset = nodeSize / 2f;


		for (int row = 0; row < rows; row++) {

			for (int col = 0; col < cols; col++) {

				float x = startX + nodeCenterOffset + (nodeSize * col);
				float z = startZ + nodeCenterOffset + (nodeSize * row);

				Vector3 startPos = new Vector3 (x, 20f, z);

				// Does our raycast hit anything at this point in the map
				RaycastHit hit2;

				// Bit shift the index of the layer (8) to get a bit mask
				int layerMask = 1 << 8;

				// This would cast rays only against colliders in layer 8.
				// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
				layerMask = ~layerMask;

				// Does the ray intersect any objects excluding the player layer
				if (Physics.Raycast (startPos, Vector3.down, out hit2, Mathf.Infinity, layerMask)) {

					print ("Hit something at row: " + row + " col: " + col);
					Debug.DrawRay (startPos, Vector3.down * 20, Color.red, 50000);
					worldData [row, col] = new Node(row, col, 1);

				} else {
					Debug.DrawRay (startPos, Vector3.down * 20, Color.green, 50000);
					worldData [row, col] = new Node(row, col, 0);
				}
			}
		}

	}


































    //Calculate the grid node corresponding to the character's current position
    public Node GetStartNode(Vector3 characterPosition)
    {
        //Calculate the column index in the grid by dividing the character's X position by the node size
        int col = Mathf.FloorToInt(characterPosition.x / nodeSize);
        //Calculate the row index in the grid by dividing the character's Z position by the node size
        int row = Mathf.FloorToInt(characterPosition.z / nodeSize);

        //col = Mathf.Clamp(col, 0, cols - 1);
        //row = Mathf.Clamp(row, 0, rows - 1);


        return new Node(row, col, 0);
    }

    //Calculate the grid node corresponding to the character's target position
    public Node GetTargetNode(Vector3 targetPosition)
    {
        //Calculate the column index in the grid by dividing the target X position by the node size
        int col = Mathf.FloorToInt(targetPosition.x / nodeSize);
        //Calculate the row index in the grid by dividing the target Z position by the node size
        int row = Mathf.FloorToInt(targetPosition.z / nodeSize);

        //col = Mathf.Clamp(col, 0, cols - 1); // Ensure column is within grid bounds
        //row = Mathf.Clamp(row, 0, rows - 1); // Ensure row is within grid bounds

        return new Node(row, col, 0); // Return the target node as grid coordinates (row, col)
    }
}
