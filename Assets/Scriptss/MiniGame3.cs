using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame3 : MonoBehaviour
{
    [Header("settings")]
    public float bakeTime = 10f;

    public bool canUse;
    
    
    [Header("refs")]
    public GameObject bakingIndicator;
    public GameObject bakedPrefab;
    public List<Sprite> bakedSprites;

    private bool finishedBaked;
    private float targetTime;
    private bool isBaking;

    // Start is called before the first frame update
    void Start()
    {
        targetTime = bakeTime;
    }

    void FinishedTimer()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bake()
    {
        Debug.Log("Bake");
    }
}
