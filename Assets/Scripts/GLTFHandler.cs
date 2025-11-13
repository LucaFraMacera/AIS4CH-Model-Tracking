using GLTFast;
using System;
using UnityEngine;

public class GLTFHandler{

    public async static void Instantiate(byte[] modelData, GameObject container=null){

        var gltf = new GltfImport();
        bool success = await gltf.Load(modelData, new Uri("https://example.com/virtualBase/"));
        if (success)
        {
            if(!container) {
                container = new GameObject(Guid.NewGuid().ToString());
            }
            await gltf.InstantiateMainSceneAsync(container.transform);
            Debug.Log("Model loaded successfully");
        }
        else
        {
            Debug.LogError("❌ Failed to parse GLB data");
        }
    }

}