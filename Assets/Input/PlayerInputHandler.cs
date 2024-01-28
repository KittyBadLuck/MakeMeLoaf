using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerController _playerController;
    public MiniGame1 miniGame1;
    private Vector3 startPos = new Vector3(0, 0, 0);
    private Camera camera;

    private void Awake()
    {
        if (playerPrefab != null)
        {
            _playerController = playerPrefab.GetComponent<PlayerController>();
            transform.parent = _playerController.transform;
            camera = playerPrefab.GetComponentInChildren<Camera>();
            this.GetComponent<PlayerInput>().camera = camera;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _playerController.OnMove(context);
    }

    public void LeftTrigger(InputAction.CallbackContext context)
    {
        miniGame1.LeftHand(context);
        Debug.Log( context.ReadValue<float>());
    }
    
}
