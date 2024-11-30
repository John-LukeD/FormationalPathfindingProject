using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlacer : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Collider planeCollider;
    // Start is called before the first frame update
    void Start()
    {
        // Find the plane by its tag, or directly by finding the collider in the scene
        planeCollider = GameObject.FindGameObjectWithTag("Plane").GetComponent<Collider>();
        
        if (planeCollider == null)
        {
            Debug.LogError("Plane Collider not found! Make sure the plane has a tag 'Plane' and a Collider attached.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for right mouse button click (1 represents right click)
        if (Input.GetMouseButtonDown(1))
        {
            // Create a ray from the camera to the mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with the plane's collider
            if (planeCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Get the position where the user clicked
                Vector3 spawnPosition = hit.point;
                spawnPosition.y = .5f;

                // Instantiate the circle prefab at the clicked position
                Instantiate(circlePrefab, spawnPosition, Quaternion.identity);
                
            }
        }
    }
}
