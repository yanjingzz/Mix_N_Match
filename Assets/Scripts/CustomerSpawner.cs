using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : Singleton<CustomerSpawner>
{
    protected CustomerSpawner() {}
    public GameObject CustomerPrefab;



    readonly string characterSpritePrefix = "Images/Customers/Customers_0";

    int spawnTier;
    readonly int[] TierNums = { 3, 3, 6, 3 };
    readonly float[] spawnChance = {0.2f,0.4f,0.3f,0.1f};
    List<CustomerManager> managerQueue = new List<CustomerManager>(4);
    List<CustomerManager> managerPool = new List<CustomerManager>(5);



	// Use this for initialization
	void Start () {
        EventManager.Instance.OnMatched += Matched;

        //EventManager.Instance.OnPlaced += SpawnCustomer;

        for (int i = 1; i <= 5; i++) {
            var customer = Instantiate(CustomerPrefab, this.transform);
            var manager = customer.GetComponent<CustomerManager>();
            managerPool.Add(manager);
            manager.ResetPosition();
            manager.characterRenderer.sprite = Resources.Load<Sprite>(characterSpritePrefix + i);
        }


    }
	
    void SpawnCustomer() {

        if (managerQueue.Count >= 4) return;
        foreach(CustomerManager manager in managerPool) 
        {
            if(!manager.OnScreen) 
            {
                //Debug.Log("Customer spawner: found not on screen " + manager);
                managerQueue.Add(manager);
                manager.Order = RandomOrder();
                break;
            }
        }
        UpdateIndices();

    }

    public void FulfillOrder(int index)
    {
        managerQueue[index].Index = -1;
        managerQueue.RemoveAt(index);
        UpdateIndices();

    }

    private void UpdateIndices() {
        int count = 0;
        foreach (CustomerManager manager in managerQueue)
        {
            manager.Index = count;
            count++;
        }
    }

    Paint RandomOrder() {
        float pSum = 0f;

        for (int i = 0; i <= spawnTier; i++) {
            pSum += TierNums[i] * spawnChance[i];
        }
        float x = UnityEngine.Random.Range(0f, pSum);
        Debug.Log("Generating order: tier: " + spawnTier + ", pSum: " + pSum + ", x: " + x);
        int count = 0;
        for (int i = 0; i <= spawnTier; i++)
        {
            for (int j = 0; j < TierNums[i]; j++)
            {
                //Debug.Log("x: " + x + ",i: " + i + ",j: " + j);
                if (x > spawnChance[i])
                    x -= spawnChance[i];
                else
                    return Paint.Orderable[count];
                count++;
            }
        }
        
        return Paint.Orderable[Paint.Orderable.Length-1];
    }

    public void Matched(Paint paint, int num)
    {
        bool hasFilledOrder = false;
        for (int i = 0; i < managerQueue.Count; i++)
        {
            if (paint == managerQueue[i].Order)
            {
                FulfillOrder(i);
                hasFilledOrder = true;
                break;
            }
        }
        if(!hasFilledOrder) SpawnCustomer();
        if (paint.IsSecondary && spawnTier <= 0)
        {
            spawnTier = 1;
        }
        else if (paint.IsTertiary && spawnTier <= 1)
        {
            spawnTier += 1;
        }
        else if (paint.IsMuddy && spawnTier <= 2)
        {
            spawnTier += 1;
        }


    }
}
