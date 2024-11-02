using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPhaseLogger : MonoBehaviour
{
    void Update()
    {
        
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("Dokunma Ba�lad�: " + touch.position);
                    break;
                case TouchPhase.Moved:
                    Debug.Log("Dokunma hareket etti: " + touch.position);
                    break;
                case TouchPhase.Stationary:
                    Debug.Log("Dokunma sabit: " + touch.position);
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Dokunma sona erdi: " + touch.position);
                    break;
            }
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            if(touch.position.x < screenWidth / 2)
            {
                Debug.Log("Sol tarafa bas�l�yor");
            }
            else
            {
                Debug.Log("Sa� tarafa bas�l�yor");
            }
            if (touch.position.y < screenHeight / 2)
            {
                Debug.Log("Alt tarafa bas�l�yor");
            }
            else
            {
                Debug.Log("�st tarafa bas�l�yor");
            }
        }
    }
}
