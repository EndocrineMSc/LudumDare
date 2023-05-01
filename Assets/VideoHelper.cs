using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class VideoHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForVideo());

    }

    private IEnumerator WaitForVideo()
    {
        yield return new WaitForSeconds(31);     
        SceneManager.LoadSceneAsync("MainMenu");
    }    
}