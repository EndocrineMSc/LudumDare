using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class VideoHelper : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private int _currentSprite = 0;
    [SerializeField] private Image _image;
    private bool _isOnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        _image.sprite = _sprites[_currentSprite];
        _image.DOFade(1, 0.5f);
        StartCoroutine(ClickCooldown());
    }

    private void Update()
    {
        if (Input.anyKeyDown) 
            NextImage();
    }

    private IEnumerator ClickCooldown()
    {
        if(!_isOnCooldown)
        {
            _isOnCooldown = true;
            yield return new WaitForSeconds(1);
            _isOnCooldown = false;
        }
    }

    private void NextImage()
    {
        if(!_isOnCooldown)
        {
            StartCoroutine(ClickCooldown());
            StartCoroutine(NextImageHelper());
        }
    }

    private IEnumerator NextImageHelper()
    {
        _image.DOFade(0, 0.25f);
        _currentSprite++;
        yield return new WaitForSeconds(0.25f);
        
        if (_currentSprite > _sprites.Length - 1)
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
        else
        {
            _image.sprite = _sprites[_currentSprite];
            _image.DOFade(1, 0.25f);
        }       
    }
}