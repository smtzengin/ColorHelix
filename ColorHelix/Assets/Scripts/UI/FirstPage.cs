using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstPage : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    
   
    private void Awake()
    {
        startButton = GetComponent<Button>();
        exitButton = GetComponent<Button>();
    }

   
    public void StartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitButtonClicked()
    {
        Application.Quit();
    }

}
