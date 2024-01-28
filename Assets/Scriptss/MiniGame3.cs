using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame3 : MonoBehaviour
{
    [Header("settings")]
    public float bakeTime = 10f;

    public bool canUse = false;
    
    
    [Header("refs")]
    public GameObject bakingIndicator;
    public GameObject bakedPrefab;
    public List<Sprite> bakedSprites;
    public GameObject bakedDough;
    
    private bool finishedBaked;
    private float targetTime;
    private bool isBaking;

    // Start is called before the first frame update
    void Start()
    {
        targetTime = bakeTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isBaking)
        {
            targetTime -= Time.deltaTime;

            if (targetTime <= 0.0f)
            {
                timerEnded();
            }
        }
    }
    void timerEnded()
    {
        bakingIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
        bakedDough = GameObject.Instantiate(bakedPrefab);
        bakedDough.GetComponent<LiftedHandler>().isBaked = true;
        int i = Random.Range(0, 2);
        bakedDough.GetComponent<LiftedHandler>().doughSprite.sprite = bakedSprites[i];
        bakedDough.SetActive(false);
        isBaking = false;
        finishedBaked = true;

        targetTime = bakeTime;
    }

    public void GetDough()
    {
        bakingIndicator.SetActive(false);
        bakedDough = null;
        canUse = true;
        finishedBaked = false;
    }
    public void Bake()
    {
        bakingIndicator.SetActive(true);
        bakingIndicator.GetComponent<SpriteRenderer>().color = Color.red;
        isBaking = true;
        canUse = false;
        Debug.Log("Bake");
    }
}
