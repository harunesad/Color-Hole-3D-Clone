using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BottomGround : MonoBehaviour
{
    public ParticleSystem particle;

    int objectsCount;
    float increase, valueInc;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("Level") && PlayerPrefs.HasKey("Level") == true)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }

        LevelControl.instance.LevelText();
        LevelControl.instance.filledImage.fillAmount = 0;
        LevelControl.instance.StartPanel();

        objectsCount = GameObject.FindGameObjectsWithTag("Objects").Length;
        increase = 1 / (float)objectsCount;
        valueInc = increase;
    }
    private void Update()
    {
        print(increase);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!GameControl.isCompleted && GameControl.isMoving)
        {
            if (other.CompareTag("Objects"))
            {
                LevelControl.instance.filledImage.DOFillAmount(increase, 0.4f);
                if (increase > 0.99f)
                {
                    GameControl.isCompleted = true;
                    particle.Play();

                    if (SceneManager.GetActiveScene().buildIndex == 2)
                    {
                        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
                        Invoke("RestartScene", 3);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
                        Invoke("LevelScene", 3);
                    }
                }

                increase += valueInc;
                Destroy(other.gameObject, 1.3f);
            }
            if (other.CompareTag("Obstacles"))
            {
                GameControl.isCompleted = true;
                Destroy(other.gameObject, 0.5f);

                Camera.main.transform.DOShakePosition(1, 0.3f, 20, 60).OnComplete(() =>
                {
                    LevelControl.instance.RestartLevel();
                });

                PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    void LevelScene()
    {
        LevelControl.instance.NextLevel();
    }
    void RestartScene()
    {
        LevelControl.instance.RestartLevel();
    }
}
