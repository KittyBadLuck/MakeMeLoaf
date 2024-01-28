using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    private float leftIntensity;
    public Slider paw;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("the left trigger is at "+leftIntensity);
       paw.value = leftIntensity;
       Debug.Log("paw is at "+paw.value);
       //Debug.Log(leftIntensity);  
       if(paw.value.GetType() == typeof(float))
       {
           Debug.Log("ta mere");
       }
    }

    public void LeftHand(InputAction.CallbackContext context)
    {
        leftIntensity = context.ReadValue<float>();
          
        // paw.value = leftIntensity;
    }


}
