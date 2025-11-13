using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModelSpawnManager : MonoBehaviour
{

    public List<GameObject> models;

    public List<string> modelKeys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn(string modelKey, Vector3 position) {
       int keyPos = -1;
       for(int i = 0; i < modelKeys.Count; i++) {
        if(modelKeys[i] == modelKey) {
            keyPos = i;
        }
       }
       if(keyPos < 0) {
        return null;
       }
       return Instantiate(models[keyPos], position, Quaternion.identity);
    }

    public async void Spawn(string url, GameObject container) {
        Model model = await Client.Get(url);
        if(model == null) {
            return;
        }
        Debug.Log($"Model retrieved from API : {model}");
        container.transform.position = container.transform.position + new Vector3(model.xoffset, model.yoffset, model.zoffset);
        GLTFHandler.Instantiate(System.Convert.FromBase64String(model.base64Content), container);
    }

}
