using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]private float playerSpeed = 2.0f;
    public float yClimbOffset = 0.5f;
    public float fallDistance = 1f;
    public float climbMoveLimit = 0.5f;

    [Header("Minigame")] 
    public bool isNearCounter;

    public bool isNearMini1;
    public bool isNearMini2;
    
    [Header("Ref")]
    public Camera camera;
    private Rigidbody2D _rigidbody2D;
    public SpriteRenderer renderer;
    public Vector2 move;
    private Animator _animator;
    
    public bool isNearPlayer;
    public List<GameObject> climbablePlayer;
    public bool isLifting;
    public GameObject liftedObject;
    private bool isLifted;
    private GameObject playerClimbed;
    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        
        if (isLifted)
        {
            transform.position = playerClimbed.transform.position + new Vector3(0,yClimbOffset,0 );
        }

        if (isLifting)
        {
            _rigidbody2D.velocity = move * (playerSpeed/2);
        }
        else if(!isLifted)
        {
            _rigidbody2D.velocity = move * playerSpeed;
        }

        if (move != Vector2.zero)
        {
            if (!isLifted)
            {
                if (move.y >= 0)
                {
                    if (isLifting)
                    {
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatHoldSide"))
                        {
                            _animator.Play("CatHoldSide");
                        }
                    }
                    else
                    {
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatWalkSide"))
                        {
                            _animator.Play("CatWalkSide");
                        }
                    }
                    
                }
                else
                { 
                    if (isLifting)
                    {
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatHoldDown"))
                        {
                            _animator.Play("CatHoldDown");
                        }
                    }
                    else
                    {
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatWalkDown"))
                        {
                            _animator.Play("CatWalkDown");
                        }
                    }
                  
                
                }
            }

            if (move.x >= 0)
            {
                renderer.flipX = true;
            }
            else
            {
                renderer.flipX = false;
            }
            
        }
        else
        {
            if (isLifting)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatHoldIdle"))
                {
                    _animator.Play("CatHoldIdle");
                }
            }
            else
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatIdle"))
                {
                    _animator.Play("CatIdle");
                }
            }
           
           
        }

    }

    public void Fall()
    {
        renderer.sortingOrder += 0;
        isLifted = false;
        playerClimbed.GetComponent<PlayerController>().isLifting = false;
        transform.position = playerClimbed.transform.position + new Vector3(move.x, move.y , 0)* fallDistance;

        if (isLifting)
        {
            print("Make Lifted player fall too");
        }
    }

    public void Drop(PlayerInputHandler inputHandler)
    {
        if (liftedObject.CompareTag("Player"))
        {
            liftedObject.GetComponent<PlayerController>().Fall();
        }
        else
        {
            LiftedHandler liftHandler = liftedObject.GetComponent<LiftedHandler>();
            if (liftHandler.isBaked)
            {
                
            }
            else if (liftHandler.isReadyToBake)
            {
               
            }
            else if (liftHandler.isKneadedDough)
            {
                if (isNearMini1 && inputHandler.miniGame1.canUse)
                {
                    inputHandler.miniGame1.canUse = false;
                    inputHandler.miniGame1.dough = liftHandler.gameObject;
                    liftHandler.isLifted = false;
                    liftHandler.liftingPlayer = null;
                    liftHandler.gameObject.transform.position = inputHandler.miniGame1.doughSpawn.position;
                    isLifting = false;
                }
                else if (isNearMini2 && inputHandler.miniGame2.canUse)
                {
                    inputHandler.miniGame2.canUse = false;
                    inputHandler.miniGame2.dough = liftHandler.gameObject;
                    liftHandler.isLifted = false;
                    liftHandler.liftingPlayer = null;
                    liftHandler.gameObject.transform.position = inputHandler.miniGame2.spawnPoint.position;
                    isLifting = false;
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        movement.Normalize();
        move = new Vector2(movement.x, movement.y);
    }

    public void Climb(InputAction.CallbackContext context)
    {
        if (isLifted)
        {
            Fall();
        }
        else
        {
            if ( !isLifting) 
            {
                if (isNearPlayer && climbablePlayer.Count > 0)
                {
                    GameObject closest = GetClosestPlayer(climbablePlayer);

                    if (closest)
                    {
                        closest.GetComponent<PlayerController>().isLifting = true;
                        renderer.sortingOrder += closest.GetComponent<PlayerController>().renderer.sortingOrder + 1;
                        Transform t = this.transform;
                        Transform climbT = closest.transform;
                        transform.position = climbT.position + new Vector3(0, yClimbOffset, 0);
                        playerClimbed = closest;
                        playerClimbed.GetComponent<PlayerController>().liftedObject = this.gameObject;
                        isLifted = true;
                    }
                }
               
                
            }
        }

    }

    public bool Lift(GameObject dough)
    {
        if (isLifting)
        {
            return false;
        }
        else
        {
            dough.transform.position = this.transform.position + new Vector3(0, yClimbOffset, 0);
            dough.GetComponent<LiftedHandler>().liftingPlayer = this;
            dough.GetComponent<LiftedHandler>().isLifted = true;
            isLifting = true;
            liftedObject = dough;
            return true;
        }
    }
    
    GameObject GetClosestPlayer(List<GameObject> players)
    {
        GameObject pMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject player in players)
        {
            Transform t = player.transform;
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist && player.GetComponent<PlayerController>().isLifting == false)
            {
                pMin = player;
                minDist = dist;
            }
        }
        return pMin;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPlayer = true;
            if (! climbablePlayer.Contains(other.gameObject))
            {
                climbablePlayer.Add(other.gameObject);
            }

        }

        if (other.CompareTag("MiniGame1"))
        {
            isNearMini1 = true;
        }
        
        if (other.CompareTag("MiniGame2"))
        {
            isNearMini2 = true;
        }
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            climbablePlayer.Remove(other.gameObject);
            if (climbablePlayer.Count == 0 && climbablePlayer.Contains(other.gameObject))
            {
                isNearPlayer = false;
            }
        }
        if (other.CompareTag("MiniGame1"))
        {
            isNearMini1 = false;
        }
        
        if (other.CompareTag("MiniGame2"))
        {
            isNearMini2 = false;
        }
    }
}
