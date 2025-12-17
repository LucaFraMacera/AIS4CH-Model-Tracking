using GLTFast;
using System;
using UnityEngine;
using System.Threading.Tasks;

public class GLTFHandler{

    public async static Task<bool> Instantiate(byte[] modelData, GameObject container=null){
        if(!container) {
            container = new GameObject(Guid.NewGuid().ToString());
        }
        IDeferAgent deferAgent = container.AddComponent<TimeBudgetPerFrameDeferAgent>();
        var gltf = new GltfImport(null, deferAgent);
        bool success = await gltf.Load(modelData);
        if (success)
        {
            await gltf.InstantiateMainSceneAsync(container.transform);
            Debug.Log("Model loaded successfully");
        }
        else
        {
            Debug.LogError("❌ Failed to parse GLB data");
        }
        return true;
    }

}