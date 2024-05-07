using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class ScoreboarItem : MonoBehaviour
{
    public TMP_Text usernameText ;
    public TMP_Text killsText;
    public TMP_Text deathsText;

    // Start is called before the first frame update
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        
    }
}
