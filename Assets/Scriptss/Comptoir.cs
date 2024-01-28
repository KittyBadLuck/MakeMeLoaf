using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comptoir : MonoBehaviour
{
    public List<GameObject> waitingCustomers;
    public GameObject OrderingCustomer;
    public Canvas hub;
    

    public void TakeOrder()
    {
        print("takeOrder");
        GameObject customerOrder = null;
        foreach (var customer in waitingCustomers)
        {
            if (!customer.activeSelf)
            {
                customerOrder = customer;
            }
        }
        if (customerOrder == null)
        {
            Debug.Log("No Space Left");
        }
        else
        {
            customerOrder.GetComponent<Image>().sprite = OrderingCustomer.GetComponent<Image>().sprite;
            customerOrder.SetActive(true);
            OrderingCustomer.SetActive(false);
            //Add order to hub
        }

    }

    public void GiveOrder()
    {
        
    }
}
