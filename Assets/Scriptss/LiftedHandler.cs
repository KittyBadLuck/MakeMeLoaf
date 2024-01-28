using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftedHandler : MonoBehaviour
{
    public bool isLifted;
    public PlayerController liftingPlayer;

    // Update is called once per frame
    void Update()
    {
        if (isLifted)
        {
            this.transform.position = liftingPlayer.transform.position + new Vector3(0, liftingPlayer.yClimbOffset, 0);
        }
    }
}
