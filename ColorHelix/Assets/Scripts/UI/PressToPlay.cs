using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PressToPlay : MonoBehaviour
{
    private TextMeshProUGUI pressToPlay;

    private void Awake()
    {
        pressToPlay = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(Ball.GetZ() == 0)
        {
            pressToPlay.gameObject.SetActive(true);
        }
        else
        {
            pressToPlay.gameObject.SetActive(false);
        }
    }
}
