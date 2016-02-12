using UnityEngine;
using System.Collections;

public class AdvanceToMain : MonoBehaviour {

	public float advanceTimerValue = 0.0f;

	// Use this for initialization
	void Start () {
		advanceTimerValue = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

		advanceTimerValue += Time.deltaTime;
		if (advanceTimerValue > 0.5f) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("main");
		}
	}
}
