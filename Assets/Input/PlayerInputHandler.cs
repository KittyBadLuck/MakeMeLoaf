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
    
    [Header("Minigame")]
    public MiniGame1 miniGame1;
    public Canvas sliderPref;
    public Slider slider;
    public Slider slider2;
    
    [Header("Comptoire")]
    public Canvas comptoirCanvas;


    private void Awake()
    {
        if (playerPrefab != null)
        {
            _playerController = playerPrefab.GetComponent<PlayerController>();
            transform.parent = _playerController.transform;
        }

        slider = sliderPref.GetComponentsInChildren<Slider>()[0];
        slider2 = sliderPref.GetComponentsInChildren<Slider>()[1];
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

    public void LeftKnead(InputAction.CallbackContext context)
    {

        slider.value = context.ReadValue<float>();
    }

    public void RightKnead(InputAction.CallbackContext context)
    {
        slider2.value = context.ReadValue<float>();
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        comptoirCanvas.gameObject.SetActive(true);
        
    }
    
    
    
}
