using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toogleFollow : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject player1, player2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(virtualCamera.Follow == player1.transform)
            {
                virtualCamera.Follow = player2.transform;
            }
            else if(virtualCamera.Follow == player2.transform)
            {
                virtualCamera.Follow = player1.transform;
            }
        }
    }
}
