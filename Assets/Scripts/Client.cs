using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class Client
{

    public async static Task<T> Get<T>(string url) {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url)) {
            await uwr.SendWebRequest();
            if(uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogWarning($"No Model Found! {uwr.error}");
                return default(T);
            }
            string json = uwr.downloadHandler.text;
            T model = await Task.Run(() => JsonUtility.FromJson<T>(json));
            return model;
        }
    }

}
