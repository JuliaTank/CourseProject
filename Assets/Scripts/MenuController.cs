using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider volume;
    [SerializeField] private UnityEngine.UI.Image volumeIcon;
    [SerializeField] private TextMeshProUGUI hintstext;
    public Button level2Button;
    public Button level3Button;
    public Button resetGame;
    private bool volumeHidden;
    
    public void Start()
    {
        //settings visibility + audio update==========================================================
        volumeHidden = true;
        volume.enabled = false;
        volume.interactable = false;
        volumeIcon.enabled = false;
        volume.gameObject.SetActive(false);
        resetGame.gameObject.SetActive(false);
        hintstext.text = "";
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        //============================================================================================
        var level = PlayerPrefs.GetInt("level");
        if (level == 0)
        {
            PlayerPrefs.SetInt("level",1);
            level2Button.interactable = false;
            level3Button.interactable = false;
            
        }
        else if (level == 1)
        {
            level2Button.interactable = false;
            level3Button.interactable = false;
            
        }
        else if (level == 2)
        {
            level2Button.interactable = true;
            level3Button.interactable = false;
            
        }
        else if (level == 3)
        {
            level2Button.interactable = true;
            level3Button.interactable = true;
            
        }
    }
    

    public void LoadClickedLevel(int sceneNo)
    {
        if (sceneNo == 1)
        {
              SceneManager.LoadScene("Level1");
              PlayerPrefs.SetInt("PickUpsNo",14);
        }
        else if (sceneNo == 2)
        {
            SceneManager.LoadScene("Level2");
            PlayerPrefs.SetInt("PickUpsNo",30);
        }
        else if (sceneNo == 3)
        {
           SceneManager.LoadScene("Level3"); 
           PlayerPrefs.SetInt("PickUpsNo",42);
        }
    }

    public void OnSettings()
    {
        if (volumeHidden)
        {
            hintstext.text = "";
            volume.enabled = true;
            volume.interactable = true;
            volume.gameObject.SetActive(true);
            volumeIcon.enabled = true;
            resetGame.gameObject.SetActive(true);
            volumeHidden = false;
        }
        else {
            
            volume.enabled = false;
            volume.interactable = false;
            volume.gameObject.SetActive(false);
            volumeIcon.enabled = false;
            resetGame.gameObject.SetActive(false);
            volumeHidden = true;
        }
    }

    public void onHints()
    {
        if (hintstext.text.Equals(""))
        {
            volume.enabled = false;
            volume.interactable = false;
            volume.gameObject.SetActive(false);
            volumeIcon.enabled = false;
            resetGame.gameObject.SetActive(false);
            volumeHidden = true;
            hintstext.text = "Shoot using left Ctrl, collect all snacks, bark away the vets and find the portal";
        }
        else
        {
            hintstext.text = "";
        }
    }

    public void UpdateVolume()
    {
        PlayerPrefs.SetFloat("volume", volume.value);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void ResetGameProgress()
    {
        PlayerPrefs.SetInt("level",1);
        Start();
    }
}
