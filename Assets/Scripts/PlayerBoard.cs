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
    public List<int> stocks;

    private void Start()
    {
        gameObject.transform.SetParent(GameManager.Instance.panelScoreBoard.transform);

        textName.text = gameObject.GetPhotonView().Owner.NickName;
    }

    private void Update()
    {
        textMoney.text = money.ToString();

        for (int i = 0; i < 5; i++)
        {
            textWallets[i].text = stocks[i].ToString();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(money);

            for (int i = 0; i < 5; i++)
            {
                stream.SendNext(stocks[i]);
            }
        }
        else
        {
            money = (int)stream.ReceiveNext();

            for (int i = 0; i < 5; i++)
            {
                stocks[i] = (int)stream.ReceiveNext();
            }
        }
    }
}
