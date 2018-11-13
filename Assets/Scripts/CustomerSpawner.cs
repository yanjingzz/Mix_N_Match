using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : Singleton<CustomerSpawner>
{
    protected CustomerSpawner() {}
    public GameObject CustomerPrefab;



    readonly string characterSpritePrefix = "Images/Customers/Customers_0";
    bool spawning;
    List<CustomerManager> managerQueue = new List<CustomerManager>(4);
    List<CustomerManager> managerPool = new List<CustomerManager>(5);



	// Use this for initialization
	void Start () {
        EventManager.Instance.OnUpdatedScore += (int score) =>
        {
            if (score == 10)
            {
                spawning = true;
            }
        };
        EventManager.Instance.OnPlaced += SpawnCustomer;
        for (int i = 1; i <= 5; i++) {
            var customer = Instantiate(CustomerPrefab, this.transform);
            var manager = customer.GetComponent<CustomerManager>();
            managerPool.Add(manager);
            manager.ResetPosition();
            manager.characterRenderer.sprite = Resources.Load<Sprite>(characterSpritePrefix + i);
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void SpawnCustomer() {
        if (!spawning) return;
        if (managerQueue.Count >= 4) return;
        foreach(CustomerManager manager in managerPool) 
        {
            if(!manager.OnScreen) 
            {
                //Debug.Log("Customer spawner: found not on screen " + manager);
                managerQueue.Add(manager);
                break;
            }
        }
        updateIndices();

    }

    public void OrderFulfilled(int index)
    {
        managerQueue[index].Index = -1;
        managerQueue.RemoveAt(index);
        updateIndices();

    }

    void updateIndices() {
        int count = 0;
        foreach (CustomerManager manager in managerQueue)
        {
            manager.Index = count;
            count++;
        }
    }
}
