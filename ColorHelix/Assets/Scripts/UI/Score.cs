using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI bestScoreText;


    void Awake()
    {
        scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        bestScoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

   
    void Update()
    {
        if(Ball.GetZ() == 0)
        {
            bestScoreText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(false);
        }
        else
        {
            bestScoreText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
        }

        scoreText.text = GameController.instance.score.ToString();

        if (GameController.instance.score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", GameController.instance.score);


        bestScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}
