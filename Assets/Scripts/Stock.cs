using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

using Photon.Pun;

public enum StockType
{
    RANDOM,     // �������� ����
    WAVE,       // ũ�� ����
    HEAVY,      // ���� ����
    INCREASE,   // ���� ���
    BIGUP,      // ū�� ���
    BIGDOWN,    // ū�� ����
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

    [SerializeField] private List<int> costGraph = new List<int>(); // ���� SerializeField ���ְ� readonly
    private int roundIndex;

    private readonly string symbols = "!@#$%^&*+=�ء١ڡۡܡݡ����ꢼ�����������ݢܢۢڢԢӢҢѢТϢ΢�";

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
            return "�ػ�������";
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
            return $"[Ư¡��] {name},\n\n'XYZ ���� ����' �������� �� ���\n\"�ְ� ���� ���� ��.\" ����";
        }
        else if (costGraph[roundIndex] > 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[Ư¡��] {name},\n\n����� ���� ���ư��� 'K-���'\n���ӵ� �ְ� ��¼� ����";
            }
            else
            {
                return $"[Ư¡��] {name},\n\n���� �� ����ġ ���� ��ǳ�� ��û\n����ߴ� �ְ� �̹��� �϶��ϳ�";
            }
        }
        else if (costGraph[roundIndex] < 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[Ư¡��] {name},\n\n'��¦' �ݵ� ��ȣź �...\n�϶��� �غ� �� �ְ� ��� ����";
            }
            else
            {
                return $"[Ư¡��] {name},\n\n�̾��� ��� ħü�� �߸� ����...\n\"�ְ� �� �ٽ� �϶��� ��\"";
            }
        }
        else
        {
            return $"[Ư¡��] {name},\n\n�̾��� ħ�� ���� '��ȭ' ����\n�ְ� {costGraph[roundIndex + 1]}p  ���� ���ɼ� ����";
        }

        // "[Ư¡��] ��������,\n\nȣȲ�� ����� ������ ����\n�������� �ְ� +{0}$ ����"
        // "[Ư¡��] ��������,\n\n���� ��ǥ�� �ְ� �����ǳ�?\n�������� �ְ� ���� ����"
        // "[Ư¡��] �����,\n\n������� ������ �϶���...\n�������� �ְ� {0}$ ����"
        // "[����]\n�� ��Ʈ��Ʈ ����,\n\"��� ħü�� ���ٶ��� ��\"\n�������� {0}���� �϶� ����"
        // "[����]\n���� �� ��å ��ǥ,\n�ֽ� ���� Ȱ�� �Ҿ�ֳ�?\n�������� {0}���� ��� ����"
        // "[Ư¡��] �ݡݡݡ�,\n\n�������� �̾� �Ҽ۱���...\n�ᱹ �Ļ� ���� ���"
        // "[Ư¡��] ��������,\n\nK-����� �� �����ֳ�...\n�Ļ� ���⿡�� �һ�"
    }
}
