using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IpPanelScript : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField ipInput;

    [SerializeField]
    public TMP_Text ipText;

    // Start is called before the first frame update
    void Start()
    {}
    // Update is called once per frame
    void Update(){
        ipText.text = ClientManager.IP;
    }

    public void SetClientManagerIp() {
        string ip = ipInput.text;
        ClientManager.Instance.SetIP(ip);
    }

}
