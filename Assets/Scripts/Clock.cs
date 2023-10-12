using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

public class Clock : MonoBehaviourPunCallbacks, IPunObservable
{
    private readonly WaitForSeconds delay_1s = new WaitForSeconds(1f);
    private readonly WaitForSeconds delay_05s = new WaitForSeconds(0.5f);

    private int hh = 9;
    private int mm = 0;

    private TextMeshProUGUI textClock;

    private void Start()
    {
        textClock = GameManager.Instance.textClock;

        if (photonView.AmOwner)
        {
            StartCoroutine(CoStartTime());
        }
    }

    private IEnumerator CoStartTime()
    {
        while (true)
        {
            mm += 1;

            switch (mm)
            {
                case 20:
                case 50:
                    photonView.RPC(nameof(RPCTimeImminent), RpcTarget.All);
                    break;

                case 30:
                    photonView.RPC(nameof(RPCNextRound), RpcTarget.All);
                    break;

                case 60:
                    hh += 1;
                    mm = 0;
                    photonView.RPC(nameof(RPCNextRound), RpcTarget.All);
                    break;
            }

            if (hh == 15 && mm == 30)
            {
                photonView.RPC(nameof(RPCGameOver), RpcTarget.All);
                break;
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
    private void RPCNextRound()
    {
        GameManager.Instance.NextRound();
    }

    [PunRPC]
    private void RPCGameOver()
    {
        GameManager.Instance.GameOver();
    }

    [PunRPC]
    private void RPCTimeImminent()
    {
        StartCoroutine(CoClockBlink());
    }

    private IEnumerator CoClockBlink()
    {
        while ((15 <= mm && mm < 30) || (45 <= mm && mm < 60))
        {
            textClock.color = Color.black;
            yield return delay_05s;
            textClock.color = Color.yellow;
            yield return delay_05s;
        }

        textClock.color = Color.yellow;
    }
}
