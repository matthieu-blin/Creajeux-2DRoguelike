using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI   ;
using UnityEngine.SceneManagement;

public class SimpleLobby : MonoBehaviour
{
    public int m_sceneOnGo;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
     public void GoPressed()
    {
        SceneManager.LoadScene(m_sceneOnGo, LoadSceneMode.Single);
    }

}
