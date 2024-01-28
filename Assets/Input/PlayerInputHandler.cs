using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerController _playerController;
    public MiniGame1 miniGame1;
    private Vector3 startPos = new Vector3(0, 0, 0);
    public float test;
    public Canvas sliderPref;
    public Slider slider;


    private void Awake()
    {
        if (playerPrefab != null)
        {
            _playerController = playerPrefab.GetComponent<PlayerController>();
            transform.parent = _playerController.transform;
        }

        slider = GameObject.Instantiate(sliderPref).GetComponentInChildren<Slider>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _playerController.OnMove(context);

            
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _playerController.Climb(context);
        }
    }

    public void Test(InputAction.CallbackContext context)
    {
        slider.value = context.ReadValue<float>();
    }
    
    
    
}
