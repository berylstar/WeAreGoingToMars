using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

using Photon.Pun;

public enum StockType
{
    RANDOM,     // 巷拙是稽 痕疑
    WAVE,       // 滴惟 痕疑
    HEAVY,      // 旋惟 痕疑
    INCREASE,   // 繊繊 雌渋
    BIGUP,      // 笛賑 雌渋
    BIGDOWN,    // 笛賑 姶社
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

    [SerializeField] private List<int> costGraph = new List<int>(); // 蓄板 SerializeField 蒸蕉壱 readonly
    private int roundIndex;

    private readonly string symbols = "!@#$%^&*+=『』【】＋−≧∞∴♂♀⊆∋⊇⊂⊃∪⊥∠�暸擒國哂劬僊丐連裏�";

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
            return "『雌舌二走";
        }
        else if (changes > 0)
        {
            return $"({changes}＜)";
        }
        else if (changes < 0)
        {
            return $"({changes}≦)";
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
        else if (costNow > 0 && isDelisting)
        {
            isDelisting = false;
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
            return $"[働臓爽] {name},\n\n'XYZ 井薦 煽確' 穿庚亜級 脊 乞焼\n\"爽亜 痕疑 蒸聖 依.\" 森著";
        }
        else if (costGraph[roundIndex] > 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[働臓爽] {name},\n\n室域稽 燦嬢 蟹焼亜澗 'K-奄穣'\n尻紗吉 爽亜 雌渋室 穿諺";
            }
            else
            {
                return $"[働臓爽] {name},\n\n授牌 掻 森雌帖 公廃 蝕燃拭 番短\n雌渋梅揮 爽亜 戚腰殖 馬喰馬蟹";
            }
        }
        else if (costGraph[roundIndex] < 0)
        {
            if (costGraph[roundIndex + 1] > 0)
            {
                return $"[働臓爽] {name},\n\n'鉛側' 鋼去 重硲添 庶蟹...\n馬喰室 駅差 板 爽亜 雌渋 森著";
            }
            else
            {
                return $"[働臓爽] {name},\n\n戚嬢遭 井奄 徴端拭 降鯉 説粕...\n\"爽亜 暁 陥獣 馬喰拝 依\"";
            }
        }
        else
        {
            return $"[働臓爽] {name},\n\n戚嬢遭 徴幸 凹壱 '痕鉢' 森壱\n爽亜 {costGraph[roundIndex + 1]}p  痕疑 亜管失 薦奄";
        }

        // "[働臓爽] けけけけ,\n\n硲伐拭 蝕企厭 燈切切 侯形\n穿庚亜級 爽亜 +{0}$ 森著"
        // "[働臓爽] しししし,\n\n叔旋 降妊稽 爽亜 照舛鞠蟹?\n穿庚亜級 爽亜 疑衣 森著"
        // "[働臓爽] ≠≠≠≠,\n\n奄企戚馬 叔旋拭 馬喰室...\n穿庚亜級 爽亜 {0}$ 森著"
        // "[井薦]\n杉 什闘軒闘 煽確,\n\"井奄 徴端拭 濁郊寓災 依\"\n穿庚亜級 {0}鯵爽 馬喰 森著"
        // "[井薦]\n舛採 歯 舛奪 降妊,\n爽縦 獣舌 醗奄 災嬢隔蟹?\n穿庚亜級 {0}鯵爽 雌渋 森著"
        // "[働臓爽] −−−−,\n\n姥繕繕舛 戚嬢 社勺猿走...\n衣厩 督至 箭託 高焼"
        // "[働臓爽] けけけけ,\n\nK-奄穣税 毘 左食爽蟹...\n督至 是奄拭辞 社持"
    }

    public void MarketClosed()
    {
        buttonBuy.enabled = false;
    }
}
