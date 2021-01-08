﻿using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class Effects : MonoBehaviour
{
    public GameObject maskBG;
    public GameObject circleMask;
    public GameObject cutsceneMask;
    public GameObject fadeoutMask;
    public GameObject LevelUI;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "UI";
        GetComponent<Canvas>().sortingOrder = 1;
        maskBG.SetActive(false);
        circleMask.SetActive(false);
        cutsceneMask.SetActive(false);
        fadeoutMask.SetActive(false);
    }
    public void Animate(string anim)
    {
        switch (anim)
        {
            default:
                break;

            case "death":
                GameManager.instance.pauseable = false;
                circleMask.SetActive(true);
                maskBG.SetActive(true);
                Camera.main.GetComponent<CameraController2D>().follow = false;
                Camera.main.GetComponent<CameraController2D>().followSpeed = 0;
                circleMask.GetComponent<RectTransform>().position = GameObject.Find("Player").transform.position;
                circleMask.transform.position = new Vector3(circleMask.transform.position.x, circleMask.transform.position.y + 0.5f, circleMask.transform.position.z);
                circleMask.GetComponent<Animator>().SetTrigger("dead"); 
                StartCoroutine(Wait(2.1f));
                break;

            case "cutsceneIn":
                cutsceneMask.SetActive(true);
                maskBG.SetActive(true);
                GameManager.instance.pauseable = false;
                LevelUI.SetActive(false);
                cutsceneMask.GetComponent<Animator>().SetTrigger("in");
                break;

            case "cutsceneOut":
                cutsceneMask.GetComponent<Animator>().SetTrigger("out");
                StartCoroutine(Wait2(0.51f));
                break;

            case "fadein":
                fadeoutMask.SetActive(true);
                GameManager.instance.pauseable = false;
                fadeoutMask.GetComponent<Animator>().SetTrigger("in");
                StartCoroutine(Wait3(1f));
                break;

            case "fadeout":
                fadeoutMask.SetActive(true);     
                fadeoutMask.GetComponent<Animator>().SetTrigger("out");
                StartCoroutine(Wait3(1f));
                break;
        }

        IEnumerator Wait(float t)
        {
            yield return new WaitForSeconds(t);
            circleMask.SetActive(false);
            maskBG.SetActive(false);
            GameManager.instance.pauseable = true;
        }

        IEnumerator Wait2(float t)
        {
            yield return new WaitForSeconds(t);
            cutsceneMask.SetActive(false);
            maskBG.SetActive(false);
            GameManager.instance.pauseable = true;
            LevelUI.SetActive(true);
        }

        IEnumerator Wait3(float t)
        {
            yield return new WaitForSeconds(t);
            Debug.Log("done fade");
            fadeoutMask.SetActive(false);
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
    }
}