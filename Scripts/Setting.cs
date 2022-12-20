using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject[] musicImages;
    [SerializeField] private GameObject[] vibratitonImages;
    private byte choosenMusic = 0,choosenVib = 0;
    public bool isOpenVib = true;
    public static Setting instance;
    [SerializeField] private GameObject[] players;

    void Awake()
    {
        instance= this;
    }
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
            isOpenVib = false;
            choosenVib++;

        }
        else
        {
            vibratitonImages[choosenVib].SetActive(false);
            isOpenVib = true;
            choosenVib--;
        }
        vibratitonImages[choosenVib].SetActive(true);
    }
    public void OnClickPower()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<Player>().PowerUp();
        }
    }
}
