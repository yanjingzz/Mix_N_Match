using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPooler : Singleton<CustomerPooler>
{
    protected CustomerPooler() {}
    public GameObject CustomerPrefab;

    //int spawnTier;
    //readonly int[] TierNums = { 3, 3, 6, 3 };
    //readonly float[] spawnChance = {0.2f,0.4f,0.3f,0.1f};
    List<CustomerController> customerQueue = new List<CustomerController>(4);
    List<CustomerController> customerPool = new List<CustomerController>(5);



	// Use this for initialization
	void Start () {
        EventManager.Instance.OnMatched += Matched;
        EventManager.Instance.OnPlaced += SpawnCustomer;

        for (int i = 0; i < 5; i++) {
            var customer = Instantiate(CustomerPrefab, this.transform);
            var controller = customer.GetComponent<CustomerController>();
            customerPool.Add(controller);
            controller.Create(i);
        }


    }
	
    void SpawnCustomer() {
        Debug.Log("Customer spawner: spawning");
        if (customerQueue.Count >= 4) return;
        foreach(CustomerController customer in customerPool) 
        {
            if(customer.Spawnable) 
            {
                //Debug.Log("Customer spawner: found not on screen " + manager);
                if (customer.Art == null)
                {
                    //Debug.Log("Customer spawner: artpiece not found, generating artpiece for " + manager);
                    var piece = ArtpieceManager.Instance.RandomArtpiece();

                    if (piece != null)
                    {
                        Debug.Log("Customer spawner: Assigning " + piece + " to " + customer);

                        customer.SpawnWithArt(piece, this);
                        customerQueue.Add(customer);
                        break;
                    }
                    else
                    {
                        //Debug.Log("Customer spawner: no additional pieces found, skipping");
                    }
                }
                else
                {
                    Debug.Log("Customer spawner: spawning " + customer + " w/ " + customer.Art);
                    customerQueue.Add(customer);
                    break;
                }
            }
        }
        Invoke("UpdateIndices", 1f);

    }

    public void FulfillOrder(int index)
    {
        var customer = customerQueue[index];
        customer.StandingIndex = -1;
        customer.FulfillOrder();
        customerQueue.RemoveAt(index);
        Invoke("UpdateIndices", 1f);

    }

    private void UpdateIndices() {
        int count = 0;
        foreach (CustomerController manager in customerQueue)
        {
            manager.StandingIndex = count;
            count++;
        }
    }


    public void GiftGiven(int index) {
        var customer = customerQueue[index];
        customer.StandingIndex = -1;
        customerQueue.RemoveAt(index);
        Invoke("UpdateIndices", 1f);
    }

    public void Matched(Paint paint, int num)
    {
        //bool hasFilledOrder = false;
        for (int i = 0; i < customerQueue.Count; i++)
        {
            var customer = customerQueue[i];
            if (customer.CanFulfillOrder && customer.OnScreen && customer.CheckOrderFulfilled(paint))
            {
                Debug.Log("Customer spawner: Fulfilled order");
                FulfillOrder(i);
                //hasFilledOrder = true;
                break;
            }
        }
        //if(!hasFilledOrder) SpawnCustomer();
        UpgradeTier();

    }

    void UpgradeTier() {
        int tier = ArtpieceManager.Instance.Tier;
        if (ScoreManager.Instance.Score >= 200 && tier <= 0)
        {
            tier = 1;
        }
        else if (ScoreManager.Instance.Score >= 600 && tier <= 1)
        {
            tier = 2;
        }
        else if (ScoreManager.Instance.Score >= 1000 && tier <= 2)
        {
            tier = 3;
        }
        ArtpieceManager.Instance.Tier = tier;
    }
}
