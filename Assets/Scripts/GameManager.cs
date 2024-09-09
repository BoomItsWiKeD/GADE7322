using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int playerGold;
    public TMP_Text goldText;
    
    // Start is called before the first frame update
    void Start()
    {
        playerGold = 500;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = "" + playerGold;
    }
}
