using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class PlayerBoard : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private List<TextMeshProUGUI> textWallets;

    public int money;
    public readonly List<int> stockHoldings = new List<int>() { 0, 0, 0, 0, 0 };

    private readonly int startMoney = 2000;

    private void Start()
    {
        gameObject.transform.SetParent(GameManager.Instance.panelScoreBoard.transform);
        gameObject.transform.localScale = Vector3.one;

        textName.text = gameObject.GetPhotonView().Owner.NickName;

        money = startMoney;
        for (int i = 0; i < 5; i++)
        {
            stockHoldings[i] = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(money);

            for (int i = 0; i < 5; i++)
            {
                stream.SendNext(stockHoldings[i]);
            }
        }
        else
        {
            money = (int)stream.ReceiveNext();

            for (int i = 0; i < 5; i++)
            {
                stockHoldings[i] = (int)stream.ReceiveNext();
            }
        }

        ShowPlayerStatus();
    }

    private void ShowPlayerStatus()
    {
        textMoney.text = money.ToString();

        for (int i = 0; i < 5; i++)
        {
            textWallets[i].text = stockHoldings[i].ToString();
        }
    }

    public void TryBuyStock(Stock stock)
    {
        if (money >= stock.costNow && !stock.isDelisting)
        {
            money -= stock.costNow;
            stockHoldings[stock.serialNumber] += 1;
        }
    }

    public void TrySellStock(Stock stock)
    {
        if (stockHoldings[stock.serialNumber] > 0)
        {
            money += stock.costNow;
            stockHoldings[stock.serialNumber] -= 1;
        }
    }
}
