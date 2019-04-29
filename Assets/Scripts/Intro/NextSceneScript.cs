using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NextSceneScript : MonoBehaviour
{
    private GameObject loadingText;

    private void Awake()
    {
        loadingText = GameObject.Find("Loading");
        loadingText.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            loadingText.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
