using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelControl : MonoBehaviour
{
    public static LevelControl instance = null;

    int scene = 1;

    public TMP_Text currentLevel, nextLevel;
    public Image filledImage;
    public Image panel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void LevelText()
    {
        int level = SceneManager.GetActiveScene().buildIndex + scene;

        currentLevel.text = (level).ToString();
        nextLevel.text = (level + 1).ToString();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartPanel()
    {
        panel.DOFade(0, 1.5f).From(0.9f);
    }
}
