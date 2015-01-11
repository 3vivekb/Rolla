using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public void Restart(string id){
		print ("This gui texture covers pixel 360, 450");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (GUI.Button (new Rect (400, 25, 200, 100), "Restart Button")) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
