using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

// Spawnlanma da sýkýntý var random olmuyor hep ayný yere spawnlanýyor
// bir oyuncuyu merge yaparken çýkarmýyor listeden ve oyunda kalýyor
public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] static int totalPlayerCount = 8;
    public static string[] playerTags = new string[3];
    public GameObject[,] players = new GameObject[playerTags.Length, totalPlayerCount];
    public GameObject[] playerSpawnPositions; // spawn noktalarýný
    public List<int> emptySpawnIndex= new List<int>(); // boþ olan spawn noktalarýnýn indexleri tutuluyor
    public static PlayerSpawn instance;

    [SerializeField] private int addAmounth; // ekleme butonun fiyatý
    public int[] playerLevelList = new int[totalPlayerCount]; // oyunda hangi levelda kaç oyuncu olduðunun listesi
    [SerializeField] private GameObject mergeButton;

    public List<GameObject> playerInGame; // platform üstündeki oyuncular

    [SerializeField] private int mergeAmounth;
    [SerializeField] private Transform nailTransform;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        playerTags[0] = "BluePlayer";
        playerTags[1] = "PinkPlayer";
        playerTags[2] = "RedPlayer";
        for (int i = 0; i < playerTags.Length; i++)
        {
            byte j = 0;
            foreach (var item in GameObject.FindGameObjectsWithTag(playerTags[i]))
            {
                players[i,j] = item;
                players[i,j].SetActive(false);
                j++;
            }
            
            emptySpawnIndex.Add(i);
        }
        playerSpawnPositions = GameObject.FindGameObjectsWithTag("playerSpawnPosition");
        players[0,0].transform.position = playerSpawnPositions[0].transform.position;
        emptySpawnIndex.Remove(0);
        playerLevelList[0] = 1;
        playerInGame.Add(players[0,0]);
        players[0,0].SetActive(true);
        players[0,0].transform.LookAt(nailTransform);
        ChangeGravity(players[0,0], true);
    }

    public void PlayerAdd()
    {
        if (CashManager.instance.totalCash >= addAmounth)
        {
            if (playerInGame.Count <= totalPlayerCount - 1)
            {
                byte random = (byte)UnityEngine.Random.Range(0, (emptySpawnIndex.Count - 1));
                players[0,playerLevelList[0]].SetActive(true);
                //players[0, playerLevelList[0]].GetComponent<BoxCollider>().isTrigger = true;
                players[0, playerLevelList[0]].transform.position = playerSpawnPositions[emptySpawnIndex[random]].transform.position;
                players[0, playerLevelList[0]].transform.LookAt(nailTransform);
                emptySpawnIndex.Remove(random);
                ChangeGravity(players[0, playerLevelList[0]], true);
                playerInGame.Add(players[0, playerLevelList[0]]);
                playerLevelList[0]++;
                CashManager.instance.totalCash -= addAmounth;
                addAmounth = (int) (addAmounth * 1.5);
                if (playerLevelList[0] >= 3) // 3 -> birleþme için gerekli minimum sayi
                {
                    mergeButton.SetActive(true);
                }
            }
            else
            {
                // max butonun görüntüsü set active true olacak diðer buton false olacak.
            }
        }
    }

    public void PlayerMerge()
    {
        bool check = true;
        if (CashManager.instance.totalCash >= mergeAmounth)
        {
            int index = 0; // 3 olan indexe sahip oyuncuyu bulmak için kullanýlýyor
            int playerMergeCount = 3;
            foreach (int item in playerLevelList)
            {
                if (item >= 3)
                {
                    for (int i = 0; i < playerInGame.Count; i++)
                    {
                        GameObject player = playerInGame[i];
                        if (playerMergeCount > 0 && player.GetComponent<Player>().playerLevel == index) // player levellar 0 dan baþlýyor
                        {
                            playerInGame.Remove(player);
                            //player.GetComponent<BoxCollider>().isTrigger = false;
                            ChangeGravity(player, false);
                            player.SetActive(false);
                            playerLevelList[index]--;
                            playerMergeCount--;
                        }
                        if (playerMergeCount <= 0)
                        {
                            break;
                        }
                    }
                    byte random = (byte)UnityEngine.Random.Range(0, (emptySpawnIndex.Count - 1));
                    players[index + 1,playerLevelList[index + 1]].transform.position = playerSpawnPositions[emptySpawnIndex[random]].transform.position;
                    players[index + 1, playerLevelList[index + 1]].SetActive(true);
                    players[index + 1, playerLevelList[index + 1]].transform.LookAt(nailTransform);
                    //players[index + 1, playerLevelList[index + 1]].GetComponent<BoxCollider>().isTrigger = true;
                    ChangeGravity(players[index + 1, playerLevelList[index + 1]], true);
                    emptySpawnIndex.Remove(random);
                    playerInGame.Add(players[index + 1, playerLevelList[index + 1]]);
                    playerLevelList[index + 1]++;
                    CashManager.instance.totalCash -= mergeAmounth;
                    mergeAmounth = (int)(mergeAmounth * 1.5);
                    break;
                }
                index++;
            }
        }
        foreach (int item in playerLevelList) // herhangi birinde 3 ten büyük oyuncu varsa tekrar basabilir butona
        {
            if (item >= 3)
            {
                check = false;
            }
        }
        if(check)
        {
            mergeButton.SetActive(false);
        }
    }
    private void ChangeGravity(GameObject other, bool gravity)
    {
        other.GetComponent<Rigidbody>().useGravity = gravity;
    }
}
