using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;
using Slider = UnityEngine.UI.Slider;


public class UIController : MonoBehaviour
{
    private bool volumeHidden;
    [SerializeField] private Slider volume;

    [SerializeField] private UnityEngine.UI.Image volumeIcon;
    // Start is called before the first frame update
    void Start()
    {
        volumeHidden = true;
        volume.enabled = false;
        volume.interactable = false;
        volume.gameObject.SetActive(false);
        volumeIcon.enabled = false;
    }

    public void OpenMenu()
    {
        //save scene before? to come back to same state? 
        SceneManager.LoadScene("Menu");

    }

    public void OpenSettings()
    {
        if (volumeHidden) {
                volume.enabled = true;
                volume.interactable = true;
                volume.gameObject.SetActive(true);
                volumeIcon.enabled = true;
                volumeHidden = false;
        }
        else {
            
                volume.enabled = false;
                volume.interactable = false;
                volume.gameObject.SetActive(false);
                volumeIcon.enabled = false;
                volumeHidden = true;
        }
    }

    public void UpdateVolume()
    {
        PlayerPrefs.SetFloat("volume", volume.value);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
