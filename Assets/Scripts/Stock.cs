using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

using Photon.Pun;

public enum StockType
{
    RANDOM,     // 무작위로 변동
    WAVE,       // 크게 변동
    HEAVY,      // 적게 변동
    INCREASE,   // 점점 상승
    BIGUP,      // 큰폭 상승
    BIGDOWN,    // 큰폭 감소
}

public class Stock : MonoBehaviourPunCallbacks
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

    [SerializeField] private List<int> costGraph = new List<int>(); // 추후 SerializeField 없애고 readonly
    private int roundIndex;

    private readonly string symbols = "!@#$%^&*+=※☆★○●◎→←↑↓↔♠♤♡♥♧♣♬♪♩♭‡†¶☞☜☎☏♨";

    private void Start()
    {
        InitialStock();
        ShowStockStatus();
    }

    private void InitialStock()
    {
        roundIndex = 0;
        isDelisting = false;

        for (int i = 0; i < 15; i++)
        {
            costGraph.Add(0);
        }
    }

    public void SetStockInAdvance()
    {
        for (int i = 1; i < 14; i++)
        {
            costGraph[i] = Random.Range(-3, 3) * 100;

            photonView.RPC(nameof(RPCSetStockGraph), RpcTarget.All, i, costGraph[i]);
        }        
    }

    [PunRPC]
    private void RPCSetStockGraph(int index, int value)
    {
        costGraph[index] = value;
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
            return "※상장폐지";
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
            isDelisting = true;
            costNow = 0;
            GameManager.Instance.ApplyDelistedStock(this);
        }
    }

    public string ReturnNews()
    {
        StringBuilder name = new StringBuilder();

        for (int i = 0; i < 4; i++)
        {
            name.Append(symbols[Random.Range(0, symbols.Length)]);
        }

        if (costGraph[roundIndex] > 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[특징주] {name},\n\n세계로 나아가는 'K-기업'\n주가 또 다시 상승 예측";
            }
            else
            {
                return $"[특징주] {name},\n\n갑작스러운 경기 침체에 주춤\n상승했던 주가 재차 하락";
            }
        }
        else
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[특징주] {name},\n\n드디어 반등 기회 잡나..\n하락했던 주가 이번엔 상승 예측";
            }
            else
            {
                return $"[특징주] {name},\n\n경기 침체 극복 실패...\n주가 또 다시 하락 예측";
            }
        }
    }
}
