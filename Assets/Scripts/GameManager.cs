using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

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

    [Header("PanelLoading")]
    [SerializeField] private GameObject panelLoading;
    private readonly string loadingProperty = "LOAD_SCENE";

    [Header("Stock")]
    public List<Stock> stocks;

    public PlayerBoard MyPlayer { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.Instantiate(playerBoard, Vector3.zero, Quaternion.identity);

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { loadingProperty, true } });

        if (photonView.AmOwner)
        {
            StartCoroutine(CoLoading());
        }
    }

    private IEnumerator CoLoading()
    {
        while (!AllHasTag(loadingProperty))
        {
            yield return null;
        }

        photonView.RPC(nameof(RPCOnGame), RpcTarget.All);

        PhotonNetwork.Instantiate(clock, Vector3.zero, Quaternion.identity);

        foreach (Stock stock in stocks)
        {
            stock.SetStockInAdvance();
        }
    }

    private bool AllHasTag(string key)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties[key] == null)
                return false;
        }

        return true;
    }

    [PunRPC]
    private void RPCOnGame()
    {
        panelLoading.SetActive(false);

        MyPlayer = FindMyBoard();
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
        textWallet.text = $"{MyPlayer.money} $";

        for (int i = 0; i < 5; i++)
        {
            textStockHoldings[i].text = MyPlayer.stockHoldings[i].ToString();
        }
    }

    public void NextRound()
    {
        foreach (Stock stock in stocks)
        {
            stock.ChangeStockCost();
        }

        ShowNews();
    }

    public void OnBuyStockButton(int index)
    {
        MyPlayer.TryBuyStock(stocks[index]);
        ShowMyStatus();
    }

    public void OnSellStockButton(int index)
    {
        MyPlayer.TrySellStock(stocks[index]);
        ShowMyStatus();
    }

    public void ApplyDelistedStock(Stock stock)
    {
        MyPlayer.stockHoldings[stock.serialNumber] = 0;
        ShowMyStatus();
    }

    private void ShowNews()
    {
        textNews.text = stocks[Random.Range(0, stocks.Count)].ReturnNews();
    }
}
