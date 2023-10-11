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
    public int prevChange;
    public int nextChange;
    public bool isDelisting;

    private readonly List<int> costGraph = new List<int>();

    private void Start()
    {
        prevChange = 0;
        isDelisting = false;

        SetStock();
        ShowStockStatus();
    }

    private void SetStock()
    {
        costGraph.Add(0);

        for (int i = 0; i < 10; i++)
        {
            costGraph.Add((i + 1) * 100);
        }

        nextChange = costGraph[0];
    }

    private void ShowStockStatus()
    {
        textCost.text = costNow.ToString();
        textChange.text = ShowCostChange();
    }

    public void ChangeStockCost()
    {
        costNow += nextChange;
        costGraph.RemoveAt(0);
        nextChange = costGraph[0];

        ShowStockStatus();
    }

    private string ShowCostChange()
    {
        if (isDelisting)
        {
            return "(상장폐지)";
        }
        else if (prevChange > 0)
        {
            return $"({prevChange}▲)";
        }
        else if (prevChange < 0)
        {
            return $"({prevChange}▼)";
        }
        else
        {
            return "(----)";
        }
    }
}
