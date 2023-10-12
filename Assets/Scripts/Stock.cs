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

        if (costGraph[roundIndex + 1] == 0)
        {
            return $"[특징주] {name},\n\n'XYZ 경제 저널' 전문가들 입 모아\n\"주가 변동 없을 것.\" 예측";
        }
        else if (costGraph[roundIndex] > 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[특징주] {name},\n\n세계로 뻗어 나아가는 'K-기업'\n연속된 주가 상승세 전망";
            }
            else
            {
                return $"[특징주] {name},\n\n순항 중 예상치 못한 역풍에 휘청\n상승했던 주가 이번엔 하락하나";
            }
        }
        else if (costGraph[roundIndex] < 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[특징주] {name},\n\n'깜짝' 반등 신호탄 쏘나...\n하락세 극복 후 주가 상승 예측";
            }
            else
            {
                return $"[특징주] {name},\n\n이어진 경기 침체에 발목 잡혀...\n\"주가 또 다시 하락할 것\"";
            }
        }
        else
        {
            return $"[특징주] {name},\n\n이어진 침묵 깨고 '변화' 예고\n주가 {costGraph[roundIndex + 1]}p  변동 가능성 제기";
        }

        // "[특징주] ㅁㅁㅁㅁ,\n\n호황에 역대급 투자자 몰려\n전문가들 주가 +{0}$ 예측"
        // "[특징주] ㅇㅇㅇㅇ,\n\n실적 발표로 주가 안정되나?\n전문가들 주가 동결 예측"
        // "[특징주] △△△△,\n\n기대이하 실적에 하락세...\n전문가들 주가 {0}$ 예측"
        // "[경제]\n월 스트리트 저널,\n\"경기 침체에 찬바람불 것\"\n전문가들 {0}개주 하락 예측"
        // "[경제]\n정부 새 정책 발표,\n주식 시장 활기 불어넣나?\n전문가들 {0}개주 상승 예측"
        // "[특징주] ◎◎◎◎,\n\n구조조정 이어 소송까지...\n결국 파산 절차 밟아"
        // "[특징주] ㅁㅁㅁㅁ,\n\nK-기업의 힘 보여주나...\n파산 위기에서 소생"
    }
}
