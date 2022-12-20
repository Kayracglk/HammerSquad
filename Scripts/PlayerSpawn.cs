using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI totalCashText;
    [SerializeField] private TextMeshProUGUI addWorkText;
    [SerializeField] private TextMeshProUGUI mergeText;

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
        }
        for (int i = 0; i < totalPlayerCount; i++)
        {
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
        totalCashText.text = CashManager.instance.totalCash.ToString();
        addWorkText.text = addAmounth.ToString();
        mergeText.text= mergeAmounth.ToString();
    }

    public void PlayerAdd()
    {
        if (CashManager.instance.totalCash >= addAmounth)
        {
            if (playerInGame.Count <= totalPlayerCount - 1)
            {
                GameObject player = players[0, playerLevelList[0]];
                player.SetActive(true);
                player.GetComponent<Player>().spawnIndex= (byte)emptySpawnIndex[0];
                player.transform.position = playerSpawnPositions[emptySpawnIndex[0]].transform.position;
                player.transform.LookAt(nailTransform);
                emptySpawnIndex.RemoveAt(0);
                ChangeGravity(player, true);
                playerInGame.Add(player);
                playerLevelList[0]++;
                CashManager.instance.totalCash -= addAmounth;
                totalCashText.text = CashManager.instance.totalCash.ToString();
                addAmounth = (int) (addAmounth * 1.2);
                addWorkText.text = addAmounth.ToString();
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
                    
                    int a = playerInGame.Count;
                    //playerInGame = SortList(playerInGame);
                    playerInGame = playerInGame.OrderBy(o => o.GetComponent<Player>().playerLevel).ToList();
                    int b = playerLevelList[0];
                    for (int i = 0; i < a; i++)
                    {
                        GameObject player = playerInGame[0];
                        if (playerMergeCount > 0 && player.GetComponent<Player>().playerLevel == index) // player levellar 0 dan baþlýyor
                        {
                            emptySpawnIndex.Add(player.GetComponent<Player>().spawnIndex);
                            playerInGame.Remove(player);
                            //player.GetComponent<BoxCollider>().isTrigger = false;
                            ChangeGravity(player, false);
                            player.SetActive(false);
                            playerLevelList[index]--;
                            playerMergeCount--;
                        }
                    }
                    GameObject player0 = players[index + 1, playerLevelList[index + 1]];
                    player0.transform.position = playerSpawnPositions[emptySpawnIndex[0]].transform.position;
                    player0.SetActive(true);
                    player0.transform.LookAt(nailTransform);
                    player0.GetComponent<Player>().spawnIndex = (byte)emptySpawnIndex[0];
                    ChangeGravity(player0, true);
                    emptySpawnIndex.RemoveAt(0);
                    playerInGame.Add(player0);
                    playerLevelList[index + 1] = playerLevelList[index + 1] + 1;
                    CashManager.instance.totalCash -= mergeAmounth;
                    mergeAmounth = (int)(mergeAmounth * 1.3);
                    mergeText.text= mergeAmounth.ToString();
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
