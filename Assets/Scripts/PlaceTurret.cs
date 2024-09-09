using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceTurret : MonoBehaviour
{
    private Ray myRay;
    private RaycastHit hit;

    public GameObject objectToInstantiate;
    public MapGenerator mapGenerator;
    
    public float minHeight = 0.6f; // Minimum height for placing turrets
    public float maxHeight = 0.7f; // Maximum height for placing turrets
    
    public float scaleFactor = 1f; // Scaling factor for the terrain mesh

    // Update is called once per frame
    void Update()
    {
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Casts from camera center to mouse pos vector.

        if (Physics.Raycast(myRay, out hit))
        {
            if (Input.GetMouseButtonDown(0)) //If left click...
            {
                Debug.Log("x: " + hit.point.x + "\ny: " + hit.point.y + "\nz: " + hit.point.z);

                if (hit.point.y >= 9 && hit.point.y <= 50 && GameManager.playerGold >= 250) // Check if height is within the desired range
                {
                    GameManager.playerGold = GameManager.playerGold - 250;
                    Instantiate(objectToInstantiate, hit.point, Quaternion.identity); // Instantiate object at click pos
                }
            }
        }
    }
}
