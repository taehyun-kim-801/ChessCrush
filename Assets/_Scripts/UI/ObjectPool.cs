using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Serializable]
    struct SerializeInfo
    {
        public GameObject serializeObject;
        public int initCount;
        public int plusCount;
    }

    [SerializeField]
    private List<SerializeInfo> poolSerializeList;
    protected Dictionary<string, List<GameObject>> poolDictionary = new Dictionary<string, List<GameObject>>();
    public bool isCreated { get; private set; }

    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
        isCreated = false;
        Initialize();
    }

    //private void Start()
    //{
    //    Initialize();
    //}

    private void Initialize()
    {
        foreach(var info in poolSerializeList)
        {
            var objList = new List<GameObject>();
            for(int i=0;i<info.initCount;i++)
            {
                var go = Instantiate(info.serializeObject, gameObject.transform) as GameObject;
                go.SetActive(false);
                objList.Add(go);
            }
            poolDictionary.Add(info.serializeObject.name, objList);
        }
        isCreated = true;
    }

    public GameObject Use(string objectName)
    {
        int objIndex = poolDictionary[objectName].FindIndex(go => !go.activeSelf);
        if(objIndex==-1)
        {
            var obj = poolDictionary[objectName][0];
            var newGameObject = Instantiate(obj, transform);
            poolDictionary[objectName].Add(newGameObject);
            return newGameObject;
        }
        else
        {
            poolDictionary[objectName][objIndex].SetActive(true);
            return poolDictionary[objectName][objIndex];
        }
    }

    public void Destroy(GameObject go)
    {
        go.SetActive(false);
    }

    public void DestroyAll()
    {
        foreach(var list in poolDictionary)
        {
            list.Value.ForEach(go => go.SetActive(false));
        }
    }
}
