using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    public List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool = null;
    public int amoutToPool = 0;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amoutToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPoolObject()
    {
        for (int i = 0; i < amoutToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {              
                return pooledObjects[i];               
            }
        }
        return null;
    }
}
