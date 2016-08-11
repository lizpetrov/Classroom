using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Canvas mainCanvas;
	public Canvas registerCanvas;

	//register
	public InputField firstname;
	public InputField lastname;
	public InputField email;
	public InputField password;
	public InputField grade;
	public Toggle male;
	public Toggle female;
	public Toggle hotpotato;
	public InputField[] periodTeachers;

	// Use this for initialization
	void Start () {
		
	}

	void Update(){
		if (registerCanvas.enabled) {
			if (male.isOn && female.isOn)
				male.isOn = false;
			else if (male.isOn && hotpotato.isOn)
				hotpotato.isOn = false;
			else if (female.isOn && hotpotato.isOn)
				hotpotato.isOn = false;
		}
	}

	public void OpenRegisterPage(){
		mainCanvas.enabled = false;
		registerCanvas.enabled = true;
	}

	public void SubmitRegister(){
		string gender;
		if(male.isOn)
			gender = "M";
		else if(female.isOn)
			gender = "F";
		else if(hotpotato.isOn)
			gender = "HP";
		else
			gender = "F";

		Manager.Register (firstname.text, lastname.text, email.text, password.text, int.Parse(grade.text.Trim()), gender,
			periodTeachers[0].text, periodTeachers[1].text, periodTeachers[2].text, periodTeachers[3].text, 
			periodTeachers[4].text, periodTeachers[5].text);

		mainCanvas.enabled = true;
		registerCanvas.enabled = false;
	}
}
