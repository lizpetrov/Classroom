using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	private string key;
	public string addEntryURL = "";
	public string getEntryURL = "";

	// Use this for initialization
	void Start () {
		StartCoroutine (AddEntry("Xavier", "Lu", 11, 1, "Horn", "hi@gmail.com"));
	}

	IEnumerator AddEntry(string firstName, string lastName, int grade, int periodNumber, string teacherName, string email){
		string hash = MD5.Md5Sum (firstName + lastName + grade + periodNumber + teacherName + email + key);
		string postURL = addEntryURL + "firstName=" + WWW.EscapeURL(firstName) + "&lastName=" + WWW.EscapeURL(lastName) + "&grade=" + grade + 
							"&periodNumber=" + periodNumber + "&teacherName=" + WWW.EscapeURL(teacherName) + 
							"&email=" + WWW.EscapeURL(email) + "&hash=" + hash;

		WWW post = new WWW (postURL);
		yield return post;

		if(post.error != null){
			Debug.LogError ("Error: "+post.error);
		}
	}

	IEnumerator GetStudentsInClass(int periodNumber, string teacherName){
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
