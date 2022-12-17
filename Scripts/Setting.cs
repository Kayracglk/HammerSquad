using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject[] musicImages;
    [SerializeField] private GameObject[] vibratitonImages;
    private byte choosenMusic = 0,choosenVib = 0;
    
    public void OnClickMusicButton()
    {
        if(choosenMusic == 0)
        {
            musicImages[choosenMusic].SetActive(false);
            choosenMusic++;
            
        }
        else
        {
            musicImages[choosenMusic].SetActive(false);
            choosenMusic--;
        }
        musicImages[choosenMusic].SetActive(true);
    }
    

    public void OnClickVibrationButton()
    {
        if (choosenVib == 0)
        {
            vibratitonImages[choosenVib].SetActive(false);
            choosenVib++;

        }
        else
        {
            vibratitonImages[choosenVib].SetActive(false);
            choosenVib--;
        }
        vibratitonImages[choosenVib].SetActive(true);
    }
}
