using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDCount : MonoBehaviour
{
    private int currentCount = 0;
    public int score;
    public TextMeshProUGUI timer;
    public GameObject[] orders;

    public void AddOrder()
    {
        orders[currentCount].SetActive(true);
        currentCount += 1;
    }
    public void RemoveOrder()
    {
        orders[currentCount-1].SetActive(false);
        currentCount -= 1;
    }
    
}
