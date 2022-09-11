using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Canvas : MonoBehaviour
{
   [SerializeField] private GameObject settingsButton;
   [SerializeField] private GameObject settingsPanel;
   [SerializeField] private Slider audioSlider;
   [SerializeField] private TextMeshProUGUI sliderText;
   [SerializeField] private int sliderValue;
   [SerializeField] private Button resetLevel;


    private void Start()
    {
        LoadValue();
    }

    private void Update()
    {
        printSaveCurrentSliderValue();
    }

    public void openSettingsPanel()
    {
        settingsPanel.SetActive(true);
        settingsButton.SetActive(false);
        
    }

    public void closeSettingsPanel()
    {
        settingsButton.SetActive(true);
        settingsPanel.SetActive(false);
        
    }


    public void printSaveCurrentSliderValue()
    {
        float audioValue = audioSlider.value;

        PlayerPrefs.SetFloat("Volume", audioValue);

        LoadValue();

        sliderValue = Mathf.RoundToInt(audioSlider.value * 100);

        sliderText.text = "%" + sliderValue;

    }

    public void LoadValue()
    {
        float audioValue = PlayerPrefs.GetFloat("Volume");
        audioSlider.value = audioValue;

        AudioListener.volume = audioValue;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void ResetLevel()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        StartCoroutine(ResetLevelAsync());
    }

    IEnumerator ResetLevelAsync()
    {
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<CameraFollow>().Flash();
        GameController.instance.GenerateLevel();
    }
}
