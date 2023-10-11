using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    [Header("Clock")]
    public TextMeshProUGUI textClock;
    private readonly string clock = "Clock";

    [Header("News")]
    [SerializeField] private TextMeshProUGUI textNews;

    [Header("Wallet")]
    [SerializeField] private TextMeshProUGUI textWallet;
    [SerializeField] private List<TextMeshProUGUI> textStockHoldings;

    [Header("ScoreBoard")]
    [SerializeField] private Button buttonScoreBoard;
    public GameObject panelScoreBoard;
    private readonly string playerBoard = "PlayerBoard";

    [Header("Stock")]
    public List<Stock> stocks;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.Instantiate(playerBoard, Vector3.zero, Quaternion.identity);

        if (photonView.AmOwner)
        {
            PhotonNetwork.Instantiate(clock, Vector3.zero, Quaternion.identity);
        }
    }

    public void ToggleScoreBoard()
    {
        panelScoreBoard.transform.localScale = (panelScoreBoard.transform.localScale == Vector3.zero)? Vector3.one : Vector3.zero;
    }

    private PlayerBoard FindMyBoard()
    {
        foreach (Transform tf in panelScoreBoard.transform)
        {
            if (tf.gameObject.GetPhotonView().IsMine)
                return tf.GetComponent<PlayerBoard>();
        }
        return null;
    }

    private void ShowMyStatus()
    {
        PlayerBoard player = FindMyBoard();

        if (player == null)
            return;

        textWallet.text = $"{player.money} $";

        for (int i = 0; i < 5; i++)
        {
            textStockHoldings[i].text = player.stockHoldings[i].ToString();
        }
    }

    public void ChangeRound()
    {
        foreach (Stock stock in stocks)
        {
            stock.ChangeStockCost();
        }
    }

    public void OnBuyStockButton(int index)
    {
        FindMyBoard().TryBuyStock(stocks[index]);
        ShowMyStatus();
    }

    public void OnSellStockButton(int index)
    {
        FindMyBoard().TrySellStock(stocks[index]);
        ShowMyStatus();
    }
}
