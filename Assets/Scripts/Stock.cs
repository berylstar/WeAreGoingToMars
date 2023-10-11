using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public enum StockType
{
    RANDOM,     // 무작위로 변동
    WAVE,       // 크게 변동
    HEAVY,      // 적게 변동
    INCREASE,   // 점점 상승
    DECREASE,   // 점점 감소
    BIGUP,      // 큰폭 상승
    BIGDOWN,    // 큰폭 감소
}

public class Stock : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textCost;
    [SerializeField] private TextMeshProUGUI textChange;
    [SerializeField] private Button buttonBuy;
    [SerializeField] private Button buttonSell;

    public int serialNumber;
    public StockType type;
    public int costNow;
    public bool isDelisting;

    private readonly List<int> costGraph = new List<int>();
    private int roundIndex;

    private void Start()
    {
        InitialStock();
        ShowStockStatus();
    }

    private void InitialStock()
    {
        roundIndex = 0;
        isDelisting = false;

        costGraph.Add(0);
        for (int i = 0; i < 13; i++)
        {
            costGraph.Add(Random.Range(-2, 2) * 100);
        }
    }

    private void ShowStockStatus()
    {
        textCost.text = costNow.ToString();
        textChange.text = ShowCostChange();
    }

    private string ShowCostChange()
    {
        int changes = costGraph[roundIndex];

        if (isDelisting)
        {
            return "(※상장폐지)";
        }
        else if (changes > 0)
        {
            return $"({changes}▲)";
        }
        else if (changes < 0)
        {
            return $"({changes}▼)";
        }
        else
        {
            return "(----)";
        }
    }

    public void ChangeStockCost()
    {
        roundIndex += 1;

        costNow += costGraph[roundIndex];

        CheckDelisting();

        ShowStockStatus();
    }

    private void CheckDelisting()
    {
        if (costNow <= 0)
        {

        }
    }

    [PunRPC]
    private void RPCSetStockGraph()
    {

    }
}
