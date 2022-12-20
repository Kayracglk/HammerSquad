using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int power;
    public int earnAmouth;
    public int playerLevel; // player levellar 0 dan baþlayacak
    public float animationSpeed = 2; // Ekrana týklanýldýðý zaman hýzlanma için (ekstra)
    public byte spawnIndex;
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] int powerCash = 100;
    [SerializeField] private Animator anim;
    private void Start()
    {
        powerText.text = powerCash.ToString();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.speed = animationSpeed;
        }
    }
    public void AnimationSpeed()
    {
        anim.speed = 1;
    }
    public void AddForceNail()
    {
        if(Setting.instance.isOpenVib)
        {
            Handheld.Vibrate();
            Debug.Log("titredi");
        }
        BlockSpawn.instance.gameInBlocks[0].GetComponent<Block>().health -= power;
        CashManager.instance.totalCash += earnAmouth;
        cashText.text = CashManager.instance.totalCash.ToString();
        if (BlockSpawn.instance.gameInBlocks[0].GetComponent<Block>().health <= 0)
        {
            BlockSpawn.instance.BlockBreaking();
            BlockSpawn.instance.AddBlock();
            // Ekstra para eklenecek
        }

    }
    public void PowerUp()
    {
        if(CashManager.instance.totalCash >= powerCash)
        {
            Debug.Log("Týklandý");
            earnAmouth = (int)(earnAmouth * 1.2);
            powerCash = (int)(powerCash * 1.3);
            powerText.text = powerCash.ToString();
            CashManager.instance.totalCash -= powerCash;
        }
        
    }
    /*private void OnTriggerEnter(Collider other)
    {
        BlockSpawn.instance.gameInBlocks[0].GetComponent<Block>().health -= power;
        if (BlockSpawn.instance.gameInBlocks[0].GetComponent<Block>().health <= 0)
        {
            BlockSpawn.instance.BlockBreaking();
            BlockSpawn.instance.AddBlock();
            // Ekstra para eklenecek
        }
    }*/

}
