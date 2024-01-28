using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
    public List<PlayerInput> players;
    public List<LayerMask> playerLayers;
    public List<Transform> startingPoints;
    public List<RuntimeAnimatorController> playerAnimators;
    private int playerCount = 0;
    public int playerMax = 4;
    private PlayerInputManager playerInputManager;
    private bool playersHidden;

    public GameObject passageBlock;

    [Header("JoinScreen")] 
    public Canvas joinScreen;

    public TextMeshProUGUI joinText;
    public List<Image> playerImages;
    public List<Sprite> playerSprites;
    private float targetTime = 2;
    private bool playerSelected;
    

    [Header("Minigame")]
    public Canvas minigame;
    public Comptoir comptoir; 
    public Canvas minigame2;
    public MiniGame3 minigame3;

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void Update()
    {
        if (playerSelected)
        {
            targetTime -= Time.deltaTime;

            if (targetTime <= 0.0f)
            {
                timerEnded();
            }


            CheckHidden();

            if (playersHidden)
            {
                passageBlock.SetActive(false);
            }
            else
            {
                passageBlock.SetActive(true);
            }
        }
        
        

    }

    private void CheckHidden()
    {
        bool hidden = false;
        foreach (var player in  players)
        {
            PlayerController pController = player.gameObject.GetComponent<PlayerInputHandler>()._playerController;
            if (pController.hasHat && pController.isLifted)
            {
                hidden = true;
            }
        }

        if (hidden == false)
        {
            playersHidden = false;
        }
        else
        {
            playersHidden = true;
        }
    }
    private void timerEnded()
    {
        foreach (var player in players)
        {
            player.ActivateInput();
        }
        joinScreen.gameObject.SetActive(false);
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
        playerParent.GetComponent<PlayerInputHandler>().miniGame3 = minigame3;
        playerParent.GetComponent<PlayerInputHandler>().comptoir = comptoir;
        playerParent.GetComponentInChildren<Animator>().runtimeAnimatorController = playerAnimators[players.Count - 1];

        playerImages[players.Count - 1].sprite = playerSprites[players.Count - 1];
        players.LastOrDefault().DeactivateInput();
        if (players.Count >= 2)
        {
            joinText.text = "Starting Game!";
            playerSelected = true;
        }

    }
}
