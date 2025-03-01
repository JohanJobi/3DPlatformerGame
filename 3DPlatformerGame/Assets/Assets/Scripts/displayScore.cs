using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class displayScore : MonoBehaviour
{
    public InventoryObject inventory1;
    public InventoryObject inventory2;
    public Item bones;
    public int score;
    public TextMeshProUGUI text;

    void Start()
    {
        //displays the score which is the amount of bones the players get
        bones.Id = 0;
        score = inventory1.AmountOfItem(bones) + inventory2.AmountOfItem(bones);
        text.text = score.ToString();
    }

}
