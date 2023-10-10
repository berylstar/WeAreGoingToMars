using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class PlayerBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private List<TextMeshProUGUI> textWallets;

    public int money;
    private List<int> stocks = new List<int>();

    public void UpdateStatus()
    {
        textName.text = gameObject.GetPhotonView().Owner.NickName;
        textMoney.text = money.ToString();

        for (int i = 0; i < 5; i++)
        {
            textWallets[i].text = stocks[i].ToString();
        }
    }
}
