using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public List<PlayerInput> players;
    public List<LayerMask> playerLayers;
    public List<Transform> startingPoints;
    public List<RuntimeAnimatorController> playerAnimators;
    private int playerCount = 0;
    public int playerMax = 4;
    private PlayerInputManager playerInputManager;

    [Header("Minigame")]
    public Canvas minigame;
    public Canvas comptoir; 
    public Canvas minigame2;

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        
    }
    
    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        //need to use the parent due to the structure of the prefab
        Transform playerParent = player.transform;
        playerParent.position = startingPoints[players.Count - 1].position;

        //convert layer mask (bit) to an integer 
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

        //set the layer
        playerParent.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        //add the layer
        playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
        playerParent.GetComponent<PlayerInputHandler>().Mini1Prefab = minigame;
        playerParent.GetComponent<PlayerInputHandler>().Mini2Prefab = minigame2;
        playerParent.GetComponent<PlayerInputHandler>().comptoirCanvas = comptoir;
        playerParent.GetComponentInChildren<Animator>().runtimeAnimatorController = playerAnimators[players.Count - 1];

    }
}
