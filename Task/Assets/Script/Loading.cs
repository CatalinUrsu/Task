using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] Slider ProgressSlider;

    AsyncOperation LoadingOperation;

    void Awake()
    {
        LoadingOperation = SceneManager.LoadSceneAsync(1);
    }

    void Update()
    {
        ProgressSlider.value = Mathf.Clamp01(LoadingOperation.progress);


    }
}
