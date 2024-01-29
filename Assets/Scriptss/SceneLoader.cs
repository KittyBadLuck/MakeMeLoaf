using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoader : MonoBehaviour
{
    public GameObject slide1;
    public GameObject slide2;
    public GameObject slide3;
    public GameObject menu;
    //public Scene 

    // Start is called before the first frame update
    void Update()
    {
        if(Input.anyKey)
        {
            slide2.SetActive(true);
            StartCoroutine(SceneStarter());
        }
    }

    IEnumerator SceneStarter()
    {
        yield return new WaitForSeconds(3);
        slide3.SetActive(true);
        yield return new WaitForSeconds(3);
        menu.SetActive(true);
        StopCoroutine(SceneStarter());
    }

    public void StartScene()
    {

    }
}
