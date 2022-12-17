using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int power;
    public int earnAmouth;
    public int playerLevel; // player levellar 0 dan baþlayacak
    public float animationSpeed; // Ekrana týklanýldýðý zaman hýzlanma için (ekstra)

    public void AddForceNail()
    {
        BlockSpawn.instance.gameInBlocks[0].GetComponent<Block>().health -= power;
        if(BlockSpawn.instance.gameInBlocks[0].GetComponent<Block>().health <= 0)
        {
            BlockSpawn.instance.BlockBreaking();
            BlockSpawn.instance.AddBlock();
            // Ekstra para eklenecek
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
