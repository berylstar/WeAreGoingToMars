using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

public class Clock : MonoBehaviourPunCallbacks, IPunObservable
{
    private TextMeshProUGUI textClock;

    private int hh = 0;
    private int mm = 0;

    private readonly WaitForSeconds delay_1s = new WaitForSeconds(1f);
    private readonly WaitForSeconds delay_05s = new WaitForSeconds(0.5f);

    private void Start()
    {
        textClock = GameManager.Instance.textClock;

        if (photonView.AmOwner)
        {
            StartCoroutine(StartTime());
        }
    }

    private IEnumerator StartTime()
    {
        while (true)
        {
            mm += 1;

            if (mm == 20 || mm == 50)
            {
                photonView.RPC(nameof(TimeImminent), RpcTarget.All);
            }

            if (mm == 30 || mm == 60)
            {
                photonView.RPC(nameof(NextRound), RpcTarget.All);
            }

            if (mm == 60)
            {
                hh += 1;
                mm = 0;
            }

            yield return delay_1s;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hh);
            stream.SendNext(mm);
        }
        else
        {
            hh = (int)stream.ReceiveNext();
            mm = (int)stream.ReceiveNext();
        }

        if (textClock != null)
            textClock.text = $"{hh:D2} : {mm:D2}";
    }

    [PunRPC]
    private void NextRound()
    {
        GameManager.Instance.ChangeRound();
    }

    [PunRPC]
    private void TimeImminent()
    {
        StartCoroutine(CoClockBlink());
    }

    private IEnumerator CoClockBlink()
    {
        while ((20 <= mm && mm < 30) || (50 <= mm && mm < 60))
        {
            textClock.color = Color.black;
            yield return delay_05s;
            textClock.color = Color.yellow;
            yield return delay_05s;
        }

        textClock.color = Color.yellow;
    }
}
