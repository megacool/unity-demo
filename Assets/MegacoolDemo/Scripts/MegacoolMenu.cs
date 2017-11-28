using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;

public class MegacoolMenu : MonoBehaviour {

    private List<string> sceneNames = new List<string>
    {
        "0_QuickStart_GifSharing",
        "1_QuickStart_Referral",
        "2_CustomizeRecording",
    };


    void Start () {
        sceneNames.ForEach( (obj) => SetCallbackForButton(obj) );
        Debug.Log("Initializing Megacool SDK");
        Megacool.Instance.Start();
    }

    void SetCallbackForButton(string _sceneName) {
        var GO = GameObject.Find(_sceneName);
        if (GO == null) {
            return;
        }
            
        var m_button = GO.GetComponent<Button> ();
        if (m_button == null) {
            return;
        }

        m_button.onClick.RemoveAllListeners();
        m_button.onClick.AddListener( () => LoadDemoScene(_sceneName));
    }
        
    public void LoadDemoScene(string _sceneName) {
        SceneManager.LoadScene(_sceneName);
    }
}
