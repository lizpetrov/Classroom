using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	private static string key = "mySecretKey";
	public static string addEntryURL = "http://www.classdiscover.com/register.php?";
	public static string loginURL = "http://www.classdiscover.com/login.php?";
	public static string getEntryURL = "";

	// Use this for initialization
	void Start () {
		//StartCoroutine ();
	}

	public static IEnumerator Register(string firstName, string lastName, string email, string password, int grade, string gender, 
						string period1TeacherName, string period2TeacherName, string period3TeacherName, 
						string period4TeacherName, string period5TeacherName, string period6TeacherName){

		string hash = MD5.Md5Sum (firstName + lastName + email + password + key);
		string postURL = addEntryURL + "firstName=" + WWW.EscapeURL(firstName) + "&lastName=" + WWW.EscapeURL(lastName) + 
										"&email=" + WWW.EscapeURL(email) + "&password=" + WWW.EscapeURL(password) + 
										"&grade=" + grade + "&gender=" + gender + 
										"&period1TeacherName=" + period1TeacherName + "&period2TeacherName=" + period2TeacherName + 
										"&period3TeacherName=" + period3TeacherName + "&period4TeacherName=" + period4TeacherName + 
										"&period5TeacherName=" + period5TeacherName + "&period6TeacherName=" + period6TeacherName + 
							 			"&hash=" + hash;

		WWW post = new WWW (postURL);
		yield return post;

		if(post.error != null){
			Debug.LogError ("Error: "+post.error);
		}
	}

	IEnumerator Login(string email, string password){
		string url = loginURL + "email=" + WWW.EscapeURL (email);
		WWW loginUser = new WWW (url);
		yield return loginUser;

		if (loginUser.error != null) {
			Debug.LogError ("Error: " + loginUser.error);
		} 
		else {
			if (password.Equals (loginUser.text)) {
				//success
			} 
			else {
				Debug.LogError ("Email/Password incorrect!");
			}
		}
	}


	public static IEnumerator GetStudentsInClass(int periodNumber, string teacherName){
		string hash = MD5.Md5Sum (periodNumber + teacherName + key);
		string getURL = getEntryURL + "periodNumber=" + periodNumber + "&teacherName=" + WWW.EscapeURL(teacherName) + "&hash=" + hash;

		WWW getStudents = new WWW (getURL);
		yield return getStudents;

		if (getStudents.error != null) {
			Debug.LogError ("Error: " + getStudents.error);
		} 
		else {
			string students = getStudents.text;
		}
	}
}
