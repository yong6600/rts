using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonMonobehaviour<ResourceManager>
{
    public Dictionary<string, Object> ResourceContainer = new Dictionary<string, Object>();
    public Dictionary<string, GameObject[]> ResourcePool = new Dictionary<string, GameObject[]>();


    public static UnityEngine.Object Load(string path)
    {
        // deveop : resource.load
        Debug.LogWarning("path:" + path);
        return Resources.Load(path);
        // production : assetbulde.. load ->
    }

    public GameObject Instantiate(string path)
    {
        Object source = null;
        // 기존에 로딩한 리소슬 라면 바로 가져오고,
        if ( ResourceContainer.ContainsKey(path) == true)
        {
            source = ResourceContainer[path];
        }
        else 
        {
            source = ResourceManager.Load(path);
            ResourceContainer.Add(path, source);
        }
        if(source != null)
        {
            GameObject newObject = GameObject.Instantiate(source) as GameObject;
            return newObject;
        }
        else
        {
            Debug.LogWarning("Please Check Path Resource Load Faild :" + path); 
            return null;
        }

    }

}
