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
    public float climbMoveLimit = 0.5f;
    
    [Header("Ref")]
    public Camera camera;
    private Rigidbody2D _rigidbody2D;
    public SpriteRenderer renderer;
    private Vector2 move;
    public bool isNearPlayer;
    public List<GameObject> climbablePlayer;
    public bool isLifting;
    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        if (move != Vector2.zero)
        {
            gameObject.transform.right = Vector2.Lerp(transform.right, move, 0.1f);
        }

        _rigidbody2D.velocity = move * playerSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        movement.Normalize();
        move = new Vector2(movement.x, movement.y);
    }

    public void Climb(InputAction.CallbackContext context)
    {

        if (isNearPlayer && climbablePlayer.Count > 0)
        {
            GameObject closest = GetClosestPlayer(climbablePlayer);

            if (closest != null)
            {
                closest.GetComponent<PlayerController>().isLifting = true;
                renderer.sortingOrder += 1;
                Transform t = this.transform;
                Transform climbT = closest.transform;
                transform.position = climbT.position + new Vector3(0, yClimbOffset, 0);
            }
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
    }
}
