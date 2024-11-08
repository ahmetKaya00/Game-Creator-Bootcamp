using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenesManager : MonoBehaviour
{
    [SerializeField] float waitTime = 4f;

    private void Start()
    {
        StartCoroutine(WaitAndLoadMainMenu());
    }

    private IEnumerator WaitAndLoadMainMenu()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("MainMenu");
    }
}
