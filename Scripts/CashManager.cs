using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public int totalCash = 100;
    public static CashManager instance;

    private void Awake()
    {
        instance= this;
    }
}
