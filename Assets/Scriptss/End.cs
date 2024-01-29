using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private EventSystem _eventSystem;
    public TextMeshProUGUI scoreText;

    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        _eventSystem = EventSystem.current;
    }
    void OnEnable()
    {
        _eventSystem.SetSelectedGameObject(button);
    }
    
    public void Open(int score)
    {
        scoreText.text = score.ToString();
        Time.timeScale = 0;
    }

    public void toTitleScreen()
    {
        SceneManager.LoadScene("Intro");
    }
}
