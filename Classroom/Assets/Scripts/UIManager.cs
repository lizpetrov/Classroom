using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Canvas mainCanvas;
	public Canvas registerCanvas;
	public Text creditText;
	public Toggle[] genderToggles;

	// Use this for initialization
	void Start () {
	
	}

	public void OpenRegisterPage(){
		mainCanvas.enabled = false;
		registerCanvas.enabled = true;
	}

	public void SubmitRegister(){
		mainCanvas.enabled = true;
		registerCanvas.enabled = false;
	}

	public void ToggleCreditText(){
		creditText.enabled = !creditText.enabled;
	}
}
