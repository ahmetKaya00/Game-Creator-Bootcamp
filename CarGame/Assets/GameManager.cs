using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform spawnPoint;

    public GameObject selectedCar;
    private CinemachineBasicMultiChannelPerlin cameranoise;
    private CinemachineTransposer transposer;

    private void Start()
    {
        string selectedPrefabName = PlayerPrefs.GetString("SelectedCarPrefab");

        GameObject carPrefab = Resources.Load<GameObject>(selectedPrefabName);

        if(carPrefab != null)
        {
            selectedCar = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
            virtualCamera.Follow = selectedCar.transform;
            virtualCamera.LookAt = selectedCar.transform;

            setCameraPositionandRotation();

            cameranoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        }
        else
        {
            Debug.Log("Seçilen araç bulunamadý.");
        }
    }

    public void setCameraPositionandRotation()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        if(transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, 5, -10);
            transposer.m_XDamping = 1f;
            transposer.m_YDamping = 1.5f;
            transposer.m_ZDamping = 0.5f;
           
        }

        var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();

        if(composer != null)
        {
            composer.m_TrackedObjectOffset = new Vector3(0, 2,0);

            composer.m_ScreenX = 0.5f;
            composer.m_ScreenY = 0.4f;
            composer.m_DeadZoneWidth = 0f;
            composer.m_DeadZoneHeight = 0f;
            composer.m_SoftZoneHeight = 0.8f;
            composer.m_SoftZoneWidth = 0.8f;
        }
    }
}
