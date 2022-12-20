using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{
    private static int blockCount = 10; // tek renkte olan block sayýsý 
    public Transform lastBlockTransform;
    [SerializeField] private float blockHeight;
    [SerializeField] private static string[] blockTags = new string[4];
    public GameObject[,] blocks = new GameObject [blockTags.Length, blockCount];
    public byte blockIndex = 0;
    public byte colourIndex = 0;
    public List<GameObject> gameInBlocks;
    [SerializeField] private GameObject[] dots;
    [SerializeField] private GameObject nail;
    [SerializeField] private Transform[] blockReset;

    public static BlockSpawn instance;

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        byte j = 0;
        blockTags[0] = "YellowBlock";
        blockTags[1] = "GreenBlock";
        blockTags[2] = "OrangeBlock";
        blockTags[3] = "BrownBlock";
        for (byte i = 0; i < blockTags.Length; i++)
        {
            j = 0;
            foreach (GameObject item in GameObject.FindGameObjectsWithTag(blockTags[i]))
            {
                blocks[i, j] = item;
                j++;
            }
        }
        gameInBlocks.Add(blocks[0, 1]);
        gameInBlocks.Add(blocks[0, 2]);
        gameInBlocks.Add(blocks[0, 3]);
        gameInBlocks.Add(blocks[0, 4]);
    }

    public void AddBlock() // üstteki kýrýlýnca sonuncu blockun bir altýna havuzdaki blocku getirecek block index max olunca bir sonraki renke geçecek renkler bitince ilk renge dönecek
    {
        if (blockIndex >= blockCount)
        {
            if(colourIndex >= blockTags.Length)
            {
                colourIndex = 0;
            }
            blockIndex= 0;
            colourIndex++;
        }
        blocks[colourIndex,blockIndex].transform.position = new Vector3(lastBlockTransform.position.x, lastBlockTransform.position.y - blockHeight, lastBlockTransform.position.z);
        nail.transform.position -= new Vector3(0,blockHeight,0);
        lastBlockTransform = blocks[colourIndex,blockIndex].transform;
        blocks[colourIndex, blockIndex].GetComponent<BoxCollider>().enabled = true;
        blocks[colourIndex, blockIndex].SetActive(true);
        gameInBlocks.Add(blocks[colourIndex,blockIndex]);
        blockIndex++;
    }

    public void BlockBreaking() // Caný sýfýrlandýðý zaman ne olacaðýnýn fonsiyonu (parçalanýp 1 saniye sonra parçalarý havuzdaki konuma gidecek kinematic true olacak ) (kinematic sonraki iþ)
    {
        ChangeTrigger(gameInBlocks[0], false);
        gameInBlocks[0].GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position -= new Vector3(0, blockHeight, 0);    
        }
        StartCoroutine(enumerator());

    }
    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1);
        ChangeTrigger(gameInBlocks[0], true);
        TransformReset(gameInBlocks[0]);
        gameInBlocks[0].SetActive(false);
        gameInBlocks.Remove(gameInBlocks[0]);
    }

    private void TransformReset(GameObject block)
    {
        /*byte i = 0;
        foreach (Transform piece in block.GetComponentsInChildren<Transform>())
        {
            piece.transform.position = blockReset[i].position;
            i++;
        }*/
        
    }

    private void ChangeTrigger(GameObject block , bool isTrigger)
    {
        byte i = 0;
        foreach (var piece in block.GetComponentsInChildren<MeshCollider>())
        {
            piece.GetComponent<MeshCollider>().isTrigger = isTrigger;
            i++;
        }
    }

}
