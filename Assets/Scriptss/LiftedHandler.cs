using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftedHandler : MonoBehaviour
{
    public bool isLifted;
    public bool isKneadedDough;
    public bool isReadyToBake;
    public bool isBaked;
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
