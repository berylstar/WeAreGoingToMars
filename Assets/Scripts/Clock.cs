using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Clock : MonoBehaviourPunCallbacks, IPunObservable
{
    private int hh = 0;
    private int mm = 0;

    private readonly WaitForSeconds oneSecond = new WaitForSeconds(1f);

    private void Start()
    {
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

            if (mm == 30 || mm == 60)
            {
                photonView.RPC("Change", RpcTarget.All);
            }

            if (mm == 60)
            {
                hh += 1;
                mm = 0;
            }

            yield return oneSecond;
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

        GameManager.Instance.textClock.text = $"{hh:D2} : {mm:D2}";
    }

    [PunRPC]
    private void Change()
    {
        GameManager.Instance.ChangeRound();
    }
}
