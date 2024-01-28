using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    public Transform doughShape;
    public Sprite firstDough;
    public Sprite secondDough;
    public Sprite finalDough;
    public Image doughSprite;
    public Vector3 startingScale = new Vector3(0,0,0);
    public bool isGameDone = false;
    public int kneadNumber = 0;
    public bool isTouching;
    public bool isTouching2;
    public Slider slider;
    public Slider slider2;

    // Start is called before the first frame update
    void Start()
    {
        kneadNumber = 0;
        startingScale = doughShape.localScale;
        doughSprite.sprite = firstDough;
    }

    // Update is called once per frame
    void Update()
    {
         if(slider.value > 0.9f && isTouching == false)
         {
             isTouching = true;
             kneadNumber += 1;
             Debug.Log(kneadNumber);
         }

         if(slider.value < 0.5f)
         {
             isTouching = false;
         }

         if(slider2.value > 0.9f && isTouching2 == false)
         {
             isTouching2 = true;
             kneadNumber += 1;
             Debug.Log(kneadNumber);
         }

         if(slider2.value < 0.5f)
         {
             isTouching2 = false;
         }

         if(slider.value > 0.5f || slider2.value > 0.5f)
         {
             doughShape.localScale = startingScale + (new Vector3(slider.value/4, -slider.value/4, 0) + new Vector3(-slider2.value/5, slider2.value/8, 0));
         }
         
         if(kneadNumber == 20)
         {
             doughSprite.sprite = secondDough;
         }

         if(kneadNumber == 40)
         {
             doughSprite.sprite = finalDough;
             isGameDone = true;
         }


    }

}
