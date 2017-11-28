using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

    const string MEGACOOL_MENU_SCENE = "MegacoolMenu";

    [SerializeField]
    private Button Back;

	void Awake() {
        Back.onClick.AddListener( () => TappedBack() );
	}

    public void TappedBack() {
        SceneManager.LoadScene(MEGACOOL_MENU_SCENE);
    }
}
