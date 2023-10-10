using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Clock")]
    [SerializeField] private TextMeshProUGUI textClock;
    private readonly WaitForSeconds oneSecond = new WaitForSeconds(1f);
    private int hh = 0;
    private int mm = 0;

    [Header("Wallet")]
    [SerializeField] private TextMeshProUGUI textWallet;
    [SerializeField] private List<TextMeshProUGUI> textAmountOfStacks;

    [Header("ScoreBoard")]
    [SerializeField] private Button buttonScoreBoard;
    [SerializeField] private GameObject panelScoreBoard;
    [SerializeField] private Transform playerBoardHolder;
    [SerializeField] private GameObject playerBoard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //StartCoroutine(StartTime());
        GameObject obj = PhotonNetwork.Instantiate("PlayerBoard", Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(playerBoardHolder);
    }

    private void Update()
    {
        ShowPlayersStatus();
    }

    private void ShowPlayersStatus()
    {
        foreach (Transform child in panelScoreBoard.transform.GetChild(0))
        {
            print(child.name);
        }
    }

    //private IEnumerator StartTime()
    //{
    //    while (true)
    //    {
    //        mm += 1;

    //        if (mm == 60)
    //        {
    //            hh += 1;
    //            mm = 0;
    //        }

    //        textClock.text = $"{hh:D2} : {mm:D2}";
    //        yield return oneSecond;
    //    }
    //}

    public void ToggleScoreBoard()
    {
        panelScoreBoard.SetActive(!panelScoreBoard.activeInHierarchy);
    }
}
