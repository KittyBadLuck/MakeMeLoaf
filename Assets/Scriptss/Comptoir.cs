using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Comptoir : MonoBehaviour
{
    public List<GameObject> waitingCustomers;
    public List<GameObject> giveButtons;
    public GameObject OrderingCustomer;
    public HUDCount hub;

    public GameObject selectedDefault;
    public GameObject canvas;

    private EventSystem _eventSystem;
    public bool isOpen;

    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    public void Open()
    {
        if (isOpen == false)
        {
            print("open");
            isOpen = true;
            canvas.SetActive(true);
        }
    }

    public void Close()
    {
        print("close");
        isOpen = false;
        canvas.SetActive(false);

    }

    public void TakeOrder()
    {
        GameObject customerOrder = null;
        foreach (var customer in waitingCustomers)
        {
            if (!customer.activeSelf)
            {
                customerOrder = customer;
            }
        }
        if (customerOrder != null)
        {
            customerOrder.GetComponent<Image>().sprite = OrderingCustomer.GetComponent<Image>().sprite;
            customerOrder.SetActive(true);
            OrderingCustomer.SetActive(false);
            hub.AddOrder();
        }

    }

    public void GiveOrder(GameObject customer)
    {
        _eventSystem.SetSelectedGameObject(selectedDefault);
        hub.RemoveOrder();
        customer.SetActive(false);
        
        
    }
    
}
