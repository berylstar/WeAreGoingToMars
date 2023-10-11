using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public enum StockType
{
    RANDOM,     // �������� ����
    WAVE,       // ũ�� ����
    HEAVY,      // ���� ����
    INCREASE,   // ���� ���
    DECREASE,   // ���� ����
    BIGUP,      // ū�� ���
    BIGDOWN,    // ū�� ����
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
            return "(�ػ�������)";
        }
        else if (changes > 0)
        {
            return $"({changes}��)";
        }
        else if (changes < 0)
        {
            return $"({changes}��)";
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
