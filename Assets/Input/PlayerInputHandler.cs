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
    public Canvas Mini1Prefab;
    public Slider slider;
    public Slider slider2;
    public bool playing1;
    
    [Header("Comptoire")]
    public Canvas comptoirCanvas;


    private void Awake()
    {
        if (playerPrefab != null)
        {
            _playerController = playerPrefab.GetComponent<PlayerController>();
            transform.parent = _playerController.transform;
        }

        slider = Mini1Prefab.GetComponentsInChildren<Slider>()[0];
        slider2 = Mini1Prefab.GetComponentsInChildren<Slider>()[1];
        miniGame1 = Mini1Prefab.gameObject.GetComponent<MiniGame1>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!playing1)
        {
            _playerController.OnMove(context);
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (!playing1)
        {
            if (context.started)
            {
                _playerController.Climb(context);
            }
        }
    }

    public void LeftKnead(InputAction.CallbackContext context)
    {
        if (playing1)
        {
            slider.value = context.ReadValue<float>();
        }
        
    }

    public void RightKnead(InputAction.CallbackContext context)
    {
        if (playing1)
        {
            slider2.value = context.ReadValue<float>();
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        if (_playerController.isNearCounter)
        {
            comptoirCanvas.gameObject.SetActive(true);
        }

        if (_playerController.isNearMini1 && miniGame1.canUse)
        {
            Mini1Prefab.gameObject.SetActive(true);
            Mini1Prefab.worldCamera = this.gameObject.GetComponent<PlayerInput>().camera;
            miniGame1.player = this;
            playing1 = true;
        }
    }
    
    
    
}
