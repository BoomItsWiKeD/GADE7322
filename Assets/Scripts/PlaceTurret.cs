using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceTurret : MonoBehaviour
{
    private Ray myRay;
    private RaycastHit hit;

    public GameObject tower1; //Tower 1 (Normal tower)
    public GameObject tower2; //Tower 2 (Fast Tower)
    public GameObject tower3; //Tower 3 (Heavy Tower)
    
    public MapBuilder mapGenerator;
    
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
                
                //If tower is selected, check it and place tower when clicking:
                if (GameManager.selectedTower1 == true && GameManager.selectedTower2 == false && GameManager.selectedTower3 == false)
                {
                    if (hit.point.y >= 9 && hit.point.y <= 50 && GameManager.playerGold >= 250) // Check if height is within the desired range and if player can buy
                    {
                        GameManager.playerGold = GameManager.playerGold - 250;
                        Instantiate(tower1, hit.point, Quaternion.identity); // Instantiate object at click pos
                    }
                }
                if (GameManager.selectedTower1 == false && GameManager.selectedTower2 == true && GameManager.selectedTower3 == false)
                {
                    if (hit.point.y >= 9 && hit.point.y <= 50 && GameManager.playerGold >= 300)
                    {
                        GameManager.playerGold = GameManager.playerGold - 300;
                        Instantiate(tower2, hit.point, Quaternion.identity);
                    }
                }
                if (GameManager.selectedTower1 == false && GameManager.selectedTower2 == false && GameManager.selectedTower3 == true)
                {
                    if (hit.point.y >= 9 && hit.point.y <= 50 && GameManager.playerGold >= 350)
                    {
                        GameManager.playerGold = GameManager.playerGold - 350;
                        Instantiate(tower3, hit.point, Quaternion.identity);
                    }
                }
            }
        }
    }
}
