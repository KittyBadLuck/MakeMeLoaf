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
    private PlayerController _playerController;
    
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
        miniGame2 = Mini2Prefab.gameObject.GetComponent<MiniGame2>();
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
            if (context.performed)
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
            miniGame1.canUse = false;
            _playerController.move = new Vector2(0, 0);
            playing1 = true;
        }

        if (_playerController.isNearMini3)
        {
            if (miniGame3.canUse)
            {
                if (_playerController.isLifted && _playerController.isLifting)
                {
                    if (_playerController.liftedObject.GetComponent<LiftedHandler>().isReadyToBake && (_playerController.liftedObject.GetComponent<LiftedHandler>().isBaked == false))
                    {
                    
                        miniGame3.Bake();
                        GameObject.Destroy(_playerController.liftedObject);
                        _playerController.liftedObject = null;
                        _playerController.isLifting = false;
                    }
                
                }
            }
            
                
        }
        

        miniGame2.Smash();
    }
    
    
    
}
