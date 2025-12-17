using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class ModelSpawnManager : MonoBehaviour
{

   private static ModelSpawnManager instance;

   [SerializeField]
   GameObject modelPreviewPrefab;

    public static ModelSpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<ModelSpawnManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("ModelSpawnManager");
                    instance = obj.AddComponent<ModelSpawnManager>();
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

    public async void Spawn(string url, GameObject container) {
        Debug.LogWarning($"Requesting model at {url}");
        long startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Model model = await Client.Get<Model>(url);
        long requestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - startTime;
        Debug.LogWarning($"Model found. Request time: {requestTime} seconds");
        if(model == null) {
            return;
        }
        Debug.Log($"Model retrieved from API : {model}");
        container.transform.position = container.transform.position + new Vector3(model.xOffset, model.yOffset, model.zOffset);
        StartCoroutine(RenderModel(model, container));
        
    }

    public IEnumerator RenderModel(Model model, GameObject container){
        Debug.LogWarning("Starting to render model");
        byte[] content = Convert.FromBase64String(model.base64Content);
        long renderStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        GameObject preview = Instantiate(modelPreviewPrefab, container.transform);
        Task<bool> renderModelTask = GLTFHandler.Instantiate(content, container);
        yield return new WaitUntil(()=>renderModelTask.IsCompleted);
        Destroy(preview);
        long renderTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - renderStartTime;
        Debug.LogWarning($"Model rendered. Time required: {renderTime} seconds");
        ModelInteractionManager.Instance.Spawn(container, model.interaction);
    }

}
