using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    public PlayerController _playerController;
    
    [Header("Minigame")]
    public MiniGame1 miniGame1;
    public Canvas Mini1Prefab;
    public Slider slider;
    public Slider slider2;
    public bool playing1;
    public MiniGame2 miniGame2;
    public Canvas Mini2Prefab;
    public bool playing2;
    public MiniGame3 miniGame3;
    
    [Header("Comptoire")]
    public Comptoir comptoir;


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
        miniGame2 = Mini2Prefab.gameObject.GetComponent<MiniGame2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!playing1 && !playing2)
        {
            _playerController.OnMove(context);
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!playing1 && !playing2)
            {
                if (!_playerController.isLifting)
                {
                    if (_playerController.isNearMini1)
                    {
                        if (miniGame1.dough != null)
                        {
                            _playerController.Lift(miniGame1.dough);
                            miniGame1.dough = null;
                            miniGame1.canUse = true;
                        }
                    }
                    else if (_playerController.isNearMini2)
                    {
                        if (miniGame2.doughWorld != null)
                        {
                            _playerController.Lift(miniGame2.doughWorld);
                            miniGame2.doughWorld = null;
                            miniGame2.canUse = true;
                        }
                    }
                    else if (_playerController.isNearMini3)
                    {
                        if (miniGame3.bakedDough != null)
                        {
                            _playerController.Lift(miniGame3.bakedDough);
                            miniGame3.bakedDough.SetActive(true);
                            miniGame3.GetDough();
                        }
                    }
                    else
                    {
                        _playerController.Climb();
                    }
                }
                else
                {
                    _playerController.Drop(this);
                }
            }
            if(playing2)
            {
                miniGame2.RetractPeet1();
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
        if (context.performed)
        {
            if (_playerController.isNearCounter && !comptoir.isOpen)
            {
                comptoir.Open();
            }

            if (_playerController.isNearMini1 && miniGame1.canUse)
            {
                Mini1Prefab.gameObject.SetActive(true);
                Mini1Prefab.worldCamera = this.gameObject.GetComponent<PlayerInput>().camera;
                miniGame1.player = this;
                miniGame1.canUse = false;
                _playerController.move = new Vector2(0, 0);
                playing1 = true;
            }

            if (_playerController.isNearMini2 && miniGame2.canUse)
            {
                if (miniGame2.doughWorld)
                {
                    _playerController.move = new Vector2(0, 0);
                    Mini2Prefab.gameObject.SetActive(true);
                    miniGame2.player = this;
                    Mini2Prefab.worldCamera = this.gameObject.GetComponent<PlayerInput>().camera;
                    playing2 = true;
                    miniGame2.canUse = true;
                }
              
            }
            if (playing2)
            {
                miniGame2.Smash();
            }

        }
    }

    public void North(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (playing2)
            {
                miniGame2.RetractPeet3();
            }
           
        }
    }

    public void East(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (playing2)
            {
                miniGame2.RetractPeet2();
            }
        }
    }
    
}

    
    
    

