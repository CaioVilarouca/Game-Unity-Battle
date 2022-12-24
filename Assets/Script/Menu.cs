using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu: MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void PlayerBattle(string Battle) { 
		SceneManager.LoadScene("Battle");
	}
	
	public void BackMenu(string Menu){
		SceneManager.LoadScene("Menu");
	}

	public void ExitGame() {
		Application.Quit();
	}
}
