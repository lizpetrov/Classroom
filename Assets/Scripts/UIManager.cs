using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Canvas mainCanvas;
	public Canvas registerCanvas;
	public Canvas portalCanvas;
	public Text titleText;

	public int id;
	public int schoolID;//aka school index

	//login
	public InputField loginEmail;
	public InputField loginPassword;
	public Text loginText;

	//register
	public InputField firstname;
	public InputField lastname;
	public InputField registerEmail;
	public InputField registerPassword;
	public InputField grade;
	public Toggle male;
	public Toggle female;
	public Toggle hotpotato;
	public InputField[] periodTeachers;
	public Dropdown school;

	//portal
	public GameObject selectionPage;
	public GameObject studentTable;
	public Text portalTitleText;
	public Text[] studentTexts;
	public Canvas updateCanvas;
	public InputField[] updatePeriodTeachers;

	private static string key = "mySecretKey";
	public static string addEntryURL = "http://www.classreveal.com/register.php?";
	public static string loginURL = "http://www.classreveal.com/login.php?";
	public static string getEntryURL = "http://www.classreveal.com/getStudents.php?";
	public static string updateURL = "http://www.classreveal.com/update.php?";

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
		portalCanvas.enabled = false;
		registerCanvas.enabled = true;
		updateCanvas.enabled = false;
	}

	public void BackToPortal(){
		mainCanvas.enabled = false;
		portalCanvas.enabled = true;
		registerCanvas.enabled = false;
		updateCanvas.enabled = false;
	}

	public void ShowUpdateCanvas(){
		mainCanvas.enabled = false;
		portalCanvas.enabled = false;
		registerCanvas.enabled = false;
		updateCanvas.enabled = true;
	}

	public void ExitTable(){
		selectionPage.SetActive (true);
		studentTable.SetActive (false);
		foreach(Text txt in studentTexts){
			txt.text = "";
		}
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

		try{
			StartCoroutine( Register(firstname.text, lastname.text, registerEmail.text, registerPassword.text, int.Parse(grade.text.Trim()), gender,
				periodTeachers[0].text.Trim (), periodTeachers[1].text.Trim (), periodTeachers[2].text.Trim (), periodTeachers[3].text.Trim (), 
				periodTeachers[4].text.Trim (), periodTeachers[5].text.Trim(), periodTeachers[6].text.Trim(), periodTeachers[7].text.Trim(), school.value));	
			mainCanvas.enabled = true;
			registerCanvas.enabled = false;
		}
		catch(Exception e){
			titleText.fontSize = 35;
			titleText.text = "Please fill in all fields.";
		}
	}

	public void Login(){
		StartCoroutine (LoginAccount(loginEmail.text.Trim(), loginPassword.text.Trim()));
	}

	public void GetStudents(int periodNumber){
		StartCoroutine (GetStudentsInClass (periodNumber, updatePeriodTeachers[periodNumber - 1].text.Trim(), schoolID));
	}

	public void UpdateUser(){
		StartCoroutine (Update(id, updatePeriodTeachers[0].text.Trim(), updatePeriodTeachers[1].text.Trim(),
			updatePeriodTeachers[2].text.Trim(), updatePeriodTeachers[3].text.Trim(),
			updatePeriodTeachers[4].text.Trim(), updatePeriodTeachers[5].text.Trim(),
			updatePeriodTeachers[6].text.Trim(), updatePeriodTeachers[7].text.Trim()));
	}

	IEnumerator Register(string firstName, string lastName, string email, string password, int grade, string gender, 
		string period1TeacherName, string period2TeacherName, string period3TeacherName, 
		string period4TeacherName, string period5TeacherName, string period6TeacherName, string period7TeacherName, string period8TeacherName, int schoolIndex){
		string hash = MD5.Md5Sum (firstName + lastName + email + password + key);
		string postURL = addEntryURL + "firstName=" + WWW.EscapeURL (firstName) + "&lastName=" + WWW.EscapeURL (lastName) +
		                 "&email=" + WWW.EscapeURL (email) + "&password=" + WWW.EscapeURL (password) +
		                 "&grade=" + grade + "&gender=" + WWW.EscapeURL (gender) +
		                 "&period1TeacherName=" + WWW.EscapeURL (period1TeacherName) + "&period2TeacherName=" + WWW.EscapeURL (period2TeacherName) +
		                 "&period3TeacherName=" + WWW.EscapeURL (period3TeacherName) + "&period4TeacherName=" + WWW.EscapeURL (period4TeacherName) +
		                 "&period5TeacherName=" + WWW.EscapeURL (period5TeacherName) + "&period6TeacherName=" + WWW.EscapeURL (period6TeacherName) +
		                 "&period7TeacherName=" + WWW.EscapeURL (period7TeacherName) + "&period8TeacherName=" + WWW.EscapeURL (period8TeacherName) +
		                 "&school=" + schoolIndex + "&hash=" + hash;

		WWW post = new WWW (postURL);
		yield return post;

		if (post.error != null) {
			Debug.LogError ("Error: " + post.error);
		} else {
			loginText.fontSize = 32;
			loginText.text = "Successfully Registered";
		}
	}

	IEnumerator LoginAccount(string email, string password){
		string url = loginURL + "email=" + WWW.EscapeURL (email);
		WWW loginUser = new WWW (url);
		yield return loginUser;

		if (loginUser.error != null) {
			Debug.LogError ("Error: " + loginUser.error);
		} 
		else {
			string[] tokens = loginUser.text.Split (' ');
			id = int.Parse (tokens[1].Trim());
			schoolID = int.Parse (tokens[2].Trim());
			for(int i = 3; i < tokens.Length; i++){
				updatePeriodTeachers [i - 3].text = tokens [i].Trim ();
			}
			if (password.Equals (tokens[0])) {//login success
				mainCanvas.enabled = false;
				portalCanvas.enabled = true;
			} 
			else {
				loginText.fontSize = 32;
				loginText.text = "Email/Password incorrect!";
			}
		}
	}

	IEnumerator GetStudentsInClass(int periodNumber, string teacherName, int schoolIndex){
		string hash = MD5.Md5Sum (periodNumber + teacherName + key);
		string getURL = getEntryURL + "periodNumber=" + periodNumber + "&teacherName=" + WWW.EscapeURL (teacherName) +
		                "&schoolID=" + schoolIndex + "&hash=" + hash;

		WWW getStudents = new WWW (getURL);
		yield return getStudents;

		if (getStudents.error != null) {
			Debug.LogError ("Error: " + getStudents.error);
		} 
		else {
			selectionPage.SetActive (false);
			studentTable.SetActive (true);
			string[] students = getStudents.text.Split ('=');
			for(int i = 0; i < students.Length - 1; i++){
				studentTexts [i].text = students [i];
			}
		}
	}

	IEnumerator Update(int userid, string period1TeacherName, string period2TeacherName, string period3TeacherName, 
		string period4TeacherName, string period5TeacherName, string period6TeacherName, string period7TeacherName, string period8TeacherName){
		string url = updateURL + "userid=" + userid +
			"&period1=" + WWW.EscapeURL(period1TeacherName) + "&period2=" + WWW.EscapeURL(period2TeacherName) + 
			"&period3=" + WWW.EscapeURL(period3TeacherName) + "&period4=" + WWW.EscapeURL(period4TeacherName) + 
			"&period5=" + WWW.EscapeURL(period5TeacherName) + "&period6=" + WWW.EscapeURL(period6TeacherName) + 
			"&period7=" + WWW.EscapeURL(period7TeacherName) + "&period8=" + WWW.EscapeURL(period8TeacherName);
		WWW updateUser = new WWW (url);
		yield return updateUser;

		if (updateUser.error != null) {
			Debug.LogError ("Error: " + updateUser.error);
		} 
		else {
			portalTitleText.text = "Successfully Updated";
			portalCanvas.enabled = true;
			updateCanvas.enabled = false;
		}
	}
}
