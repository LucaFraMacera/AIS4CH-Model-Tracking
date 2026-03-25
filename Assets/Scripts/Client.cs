using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class Client
{

    public async static Task<T> Get<T>(string url) {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url)) {
            var operation = uwr.SendWebRequest();

            // Wait for the request to finish without blocking the main thread
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogWarning($"No Model Found! {uwr.error}");
                return default(T);
            }

            string json = uwr.downloadHandler.text;
            T model = JsonUtility.FromJson<T>(json);
            return model;
        }
    }

}
