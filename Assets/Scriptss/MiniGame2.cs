using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame2 : MonoBehaviour
{
    public bool canUse = true;
    public GameObject doughWorld;
    public Transform spawnPoint;
    public Sprite doughReadySprite;
    public PlayerInputHandler player;

    private List<int> peetNumber = new List<int>();

    public GameObject dough;
    private Animation doughAnimation;
    public GameObject pattoune;
    private Animation pattouneAnimation;
    public Transform peets;
    public Transform face;
    public int slapNumber;
    private RectTransform pattouneTransform;
    public Transform peet1;
    public Transform peet2;
    public Transform peet3;
    public bool isGameDone = false;

    private float targetTime = 2f;
    
    // Start is called before the first frame update
    void Awake()
    {
        doughAnimation = dough.GetComponent<Animation>();
        pattouneAnimation = pattoune.GetComponent<Animation>();
        pattouneTransform = pattoune.GetComponent<RectTransform>();
        //doughAnimation.wrapMode = WrapMode.Once;

        peets.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        face.localScale = new Vector3(1f, 0f, 1f);
        
        slapNumber = 0;
        peetNumber.Add(0);
        peetNumber.Add(0);
        peetNumber.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(peetNumber[0] >= 8 
           && peetNumber[1] >= 8
           && peetNumber[2] >= 8)
        {
            isGameDone = true;
        }
        
        if (isGameDone)
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
        player.playing2 = false;
        SpawnDough();
        this.gameObject.SetActive(false);
    }

    void SpawnDough()
    {
        isGameDone = false;
        canUse = false;
        targetTime = 2f;
        slapNumber = 0;
        peets.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        face.localScale = new Vector3(1f, 0f, 1f);
        peetNumber[0] = 0;
        peetNumber[1] = 0;
        peetNumber[2] = 0;
        doughWorld.GetComponent<LiftedHandler>().doughSprite.sprite = doughReadySprite;
        doughWorld.GetComponent<LiftedHandler>().isReadyToBake = true;
        doughWorld.GetComponent<LiftedHandler>().isKneadedDough = false;
    }

    public void Smash()
    {

        if(doughAnimation.isPlaying)
        {
            doughAnimation.Stop("MiniGame2Dough");
        }
        doughAnimation.Play("MiniGame2Dough");

        if(pattouneAnimation.isPlaying)
        {
            pattouneAnimation.Stop("MiniGame2Hand");
        }
        pattouneAnimation.Play("MiniGame2Hand");

        float randomZ = Random.Range(-45f,45);
        pattoune.transform.eulerAngles = new Vector3(0, 0, randomZ);

        float randomX = Random.Range(-200f, 200f);
        pattouneTransform.anchoredPosition = new Vector3(randomX, -134, 0);
        
        slapNumber += 1;

        if(slapNumber < 32)
        {
            peets.localScale += new Vector3(0.02f, 0.02f, 0.02f);

            face.localScale += new Vector3(0f, 0.04f, 0f);
        }
    }
    
    public void RetractPeet1()
    {
        if(slapNumber>28)
        {
            peet1.localScale -= new Vector3(0.06f, 0.06f, 0.06f);
            peetNumber[0] += 1;
        }
        
    }
    public void RetractPeet2()
    {
        if(slapNumber>28)
        {
            peet2.localScale -= new Vector3(0.06f, 0.06f, 0.06f);
            peetNumber[1] += 1;
        }
    }
    public void RetractPeet3()
    {
        if(slapNumber>28)
        {
            peet3.localScale -= new Vector3(0.06f, 0.06f, 0.06f);
            peetNumber[2] += 1;
        }
    }

}
