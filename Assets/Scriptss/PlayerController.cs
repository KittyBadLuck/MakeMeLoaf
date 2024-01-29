
using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool hasHat;
    public bool isNearDisguise;
    public bool inCounterZone;

    [Header("Minigame")] 
    public bool isNearCounter;

    public bool isNearMini1;
    public bool isNearMini2;
    public bool isNearMini3;
    
    [Header("Ref")]
    public Camera camera;
    private Rigidbody2D _rigidbody2D;
    public SpriteRenderer renderer;
    public GameObject hat;
    public SpriteRenderer hatRenderer;
    public Vector2 move;
    private Animator _animator;
    
    public bool isNearPlayer;
    public List<GameObject> climbablePlayer;
    public bool isLifting;
    public GameObject liftedObject;
    public bool isLifted;
    public GameObject playerClimbed;

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
            if (hasHat && isLifted)
            {
                if (move.y >= 0)
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatAboveSide"))
                    {
                        _animator.Play("CatAboveSide");
                    }
                }
                else
                { 
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatAboveDown"))
                    {
                        _animator.Play("CatAboveDown");
                    }

                }
                
            }
            else
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
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatWalSide"))
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
                hatRenderer.flipX = true;
            }
            else
            {
                renderer.flipX = false;
                hatRenderer.flipX = false;
            }
            
        }
        else
        {
            if (isLifted && hasHat)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("CatAboveDown"))
                {
                    _animator.Play("CatAboveDown");
                }
                
            }
            else if (isLifting)
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
        renderer.sortingOrder -= 1;
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

            if (isNearMini1 && inputHandler.miniGame1.canUse)
            {
                if (liftHandler.isKneadedDough)
                {
                    inputHandler.miniGame1.canUse = false;
                    inputHandler.miniGame1.dough = liftHandler.gameObject;
                    liftHandler.gameObject.transform.position = inputHandler.miniGame1.doughSpawn.position;
                    liftHandler.isLifted = false;
                    liftHandler.liftingPlayer = null;
                    isLifting = false;
                    liftedObject = null;
                    
                }
            }
            else if (isNearMini2 && inputHandler.miniGame2.doughWorld == null)
            {
                if (liftHandler.isKneadedDough || liftHandler.isReadyToBake)
                {
                    if (liftHandler.isKneadedDough) {inputHandler.miniGame2.canUse = true;}
                    else {inputHandler.miniGame2.canUse = false;}
                    inputHandler.miniGame2.doughWorld = liftHandler.gameObject;
                    liftHandler.gameObject.transform.position = inputHandler.miniGame2.spawnPoint.position;
                    liftHandler.isLifted = false;
                    liftHandler.liftingPlayer = null;
                    isLifting = false;
                    liftedObject = null;
                }

            }
            else if (isNearMini3 && inputHandler.miniGame3.canUse)
            {
                if (isLifted)
                {
                    if (liftHandler.isReadyToBake &&
                        (liftHandler.isBaked == false))
                    {
                        inputHandler.miniGame3.Bake();
                        GameObject.Destroy(liftedObject);
                        liftedObject = null;
                        isLifting = false;
                    }

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

    public void Climb()
    {
        if (isLifted)
        {
            Fall();
        }
        else
        {
            if (isNearPlayer && climbablePlayer.Count > 0)
            {
                GameObject closest = GetClosestPlayer(climbablePlayer);
                PlayerController closestController = closest.GetComponent<PlayerController>();

                if (closest && closestController.isLifting)
                {
                    if (!closestController.liftedObject.CompareTag("Player"))
                    {
                        isLifting = true;
                        closestController.isLifted = true;
                        closestController.renderer.sortingOrder += closest.GetComponent<PlayerController>().renderer.sortingOrder + 1;
                        closest.transform.position = transform.position + new Vector3(0, yClimbOffset, 0);
                        closestController.playerClimbed = this.gameObject;
                        liftedObject = closest;
                    }
                }
                else if (closest)
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

    public void Lift(GameObject dough)
    {
        dough.transform.position = this.transform.position + new Vector3(0, yClimbOffset, 0);
        dough.GetComponent<LiftedHandler>().liftingPlayer = this;
        dough.GetComponent<LiftedHandler>().isLifted = true;
        isLifting = true;
        liftedObject = dough;
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
            if (dist < minDist)
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
        if (other.CompareTag("MiniGame3"))
        {
            isNearMini3 = true;
        }
        if (other.CompareTag("Counter"))
        {
            isNearCounter = true;
        }
        if (other.CompareTag("Disguise"))
        {
            isNearDisguise= true;
        }
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPlayer = false;
            climbablePlayer.Remove(other.gameObject);
        }
        if (other.CompareTag("MiniGame1"))
        {
            isNearMini1 = false;
        }
        
        if (other.CompareTag("MiniGame2"))
        {
            isNearMini2 = false;
        }
        
        if (other.CompareTag("MiniGame3"))
        {
            isNearMini3 = false;
        }
        if (other.CompareTag("Counter"))
        {
            isNearCounter = false;
        }
        if (other.CompareTag("Disguise"))
        {
            isNearDisguise= false;
        }
    }
}
