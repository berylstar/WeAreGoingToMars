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

    [Header("Wallet")]
    [SerializeField] private TextMeshProUGUI textWallet;
    [SerializeField] private List<TextMeshProUGUI> textAmountOfStacks;

    [Header("ScoreBoard")]
    [SerializeField] private Button buttonScoreBoard;
    public GameObject panelScoreBoard;
    private readonly string playerBoard = "PlayerBoard";

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
                return tf.gameObject.GetComponent<PlayerBoard>();
        }
        return null;
    }

    public void GetStock(int index)
    {
        FindMyBoard().stocks[index] += 1;
    }
}
