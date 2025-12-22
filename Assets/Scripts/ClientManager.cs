using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ClientManager : MonoBehaviour
{

    private static ClientManager instance;

    #if UNITY_EDITOR
        public static string IP = "127.0.0.1";
    #else
        public static string IP = null;
    #endif

    public static ClientManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<ClientManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("ClientManager");
                    instance = obj.AddComponent<ClientManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SetIP(string ip) {
        IP = ip;
    }

    public Task<Model> Get(string modelKey) {
        if(IP == null || IP.Length == 0) {
            return Task.FromResult<Model>(default);
        }
        string url = $"http://{IP}:8080/api/models/{modelKey}";
        Debug.LogWarning($"Requesting model {url}");
        return Client.Get<Model>(url);
    }
    
}
