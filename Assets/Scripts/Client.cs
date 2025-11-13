using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class Client
{

    public async static Task<Model> Get(string url) {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url)) {
            await uwr.SendWebRequest();
            if(uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogWarning("No Model Found!");
                return null;
            }
            string json = uwr.downloadHandler.text;
            Model model = JsonUtility.FromJson<Model>(json);
            return model;
        }
    }

}
