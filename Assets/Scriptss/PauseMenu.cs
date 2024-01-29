using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private EventSystem _eventSystem;
    public bool isOpen = false;

    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        _eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void OnEnable()
    {
        _eventSystem.SetSelectedGameObject(button);
    }

    public void Open()
    {
        isOpen = true;
        Time.timeScale = 0;
    }

    public void Close()
    {
        Time.timeScale = 1;
        isOpen = false;
        this.gameObject.SetActive(false);
    }
}
