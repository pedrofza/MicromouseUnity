﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VeraoMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject escMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMenu.SetActive(true);
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadWallLevel()
    {
        LoadLevel(0);
    }

    public void LoadSpeedLevel()
    {
        LoadLevel(1);
    }

    public void LoadTrackLevel()
    {
        LoadLevel(2);
    }

    public void Resume()
    {
        escMenu.SetActive(false);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
