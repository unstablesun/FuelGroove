using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FuelHandler : MonoBehaviour 
{
	private enum EventType
	{
		leaderboard = 0,
		mission = 1,
		quest = 2
	};



	private bool m_initialized;
	private FuelListener m_listener;
	private int variable_inc = 1;

	private void Awake ()
	{
		if (!m_initialized) {
			GameObject.DontDestroyOnLoad (gameObject);

			if (!Application.isEditor) {
				// Initialize the Fuel SDK listener
				// reference for later use by the launch
				// methods.
				m_listener = new FuelListener (this);
				FuelSDK.setListener (m_listener);
			}
			m_initialized = true;
		} else {
			GameObject.Destroy (gameObject);
		}
	}

	public void ChangeUser()
	{
		FuelSDK.Launch ();
	}

	public void GetEvents() 
	{
		List<object> tags = new List<object>();
		tags.Add("BronzeFilter");
		tags.Add("bronzeSong1");
		tags.Add("bronzeSong2");
		bool success = FuelSDK.GetEvents(tags);
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}


	public void SendProgress () 
	{
		Dictionary<string,int> scoreDict = new Dictionary<string, int>();
		scoreDict.Add("value",variable_inc);

		Dictionary<string,object> progressDict = new Dictionary<string, object>();
		progressDict.Add("bronze", scoreDict);//these keys should match the variable names

		List<object> tags = null;//new List<object>();
		tags.Add("BronzeFilter");
		tags.Add("bronzeSong1");

		List<object> methodParams = new List<object>();
		methodParams.Add( progressDict );
		methodParams.Add( tags );
		bool success = FuelSDK.ExecMethod("SendProgress", methodParams);
		if(success == true) {
			//Your progress has been successfully updated
		}
	}


	public void GetLeaderBoard(string Id) {
		bool success = FuelSDK.GetLeaderBoard( Id );
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}

	public void GetMission(string Id) {
		bool success = FuelSDK.GetMission( Id );
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}

	public void GetQuest(string Id) {
		bool success = FuelSDK.GetQuest( Id );
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}


	public void OnIgniteEvents (List<object> events)
	{
		if (events == null) {
			Debug.Log ("OnIgniteEvents - undefined list of events");
			return;
		}

		if (events.Count == 0) {
			Debug.Log ("OnIgniteEvents - empty list of events");
			return;
		}

		foreach (object eventObject in events) {
			Dictionary<string, object> eventInfo = eventObject as Dictionary<string, object>;

			if (eventInfo == null) {
				Debug.Log ("OnIgniteEvents - invalid event data type: " + eventObject.GetType ().Name);
				continue;
			}

			object eventIdObject = eventInfo["id"];

			if (eventIdObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event ID");
				continue;
			}

			if (!(eventIdObject is string)) {
				Debug.Log ("OnIgniteEvents - invalid event ID data type: " + eventIdObject.GetType ().Name);
				continue;
			}

			string eventId = (string)eventIdObject;

			object eventTypeObject = eventInfo["type"];

			if (eventTypeObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event type");
				continue;
			}

			if (!(eventTypeObject is long)) {
				Debug.Log ("OnIgniteEvents - invalid event type data type: " + eventTypeObject.GetType ().Name);
				continue;
			}

			long eventTypeLong = (long)eventTypeObject;

			int eventTypeValue = (int)eventTypeLong;

			if (!Enum.IsDefined (typeof (EventType), eventTypeValue)) {
				Debug.Log ("OnIgniteEvents - unsupported event type value: " + eventTypeValue.ToString ());
				continue;
			}

			EventType eventType = (EventType)eventTypeValue;

			object eventJoinedObject = eventInfo["joined"];

			if (eventJoinedObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event joined status");
				continue;
			}

			if (!(eventJoinedObject is bool)) {
				Debug.Log ("OnIgniteEvents - invalid event joined data type: " + eventJoinedObject.GetType ().Name);
				continue;
			}

			bool eventJoined = (bool)eventJoinedObject;

			string eventTypeString = eventType.ToString ();

			if (eventJoined) {
				Debug.Log ("OnIgniteEvents - player is joined in event of type '" + eventTypeString + "' with event ID: " + eventId);

				switch (eventType) {
				case EventType.leaderboard:
					GetLeaderBoard (eventId);
					break;
				case EventType.mission:
					GetMission (eventId);
					break;
				case EventType.quest:
					break;
				default:
					Debug.Log ("OnIgniteEvents - unsupported event type: " + eventTypeString);
					continue;
				}
			} else {
				Debug.Log ("OnIgniteEvents - player can opt-in to join event of type '" + eventTypeString + "' with event ID: " + eventId);
			}
		}

	}

	public void OnIgniteLeaderBoard (Dictionary<string, object> leaderBoard)
	{
		if (leaderBoard == null) {
			Debug.Log ("OnIgniteLeaderBoard - undefined leaderboard");
			return;
		}

		if (leaderBoard.Count == 0) {
			Debug.Log ("OnIgniteLeaderBoard - empty leaderboard");
			return;
		}

		string leaderBoardString = FuelSDKCommon.Serialize (leaderBoard);

		if (leaderBoardString == null) {
			Debug.Log ("OnIgniteLeaderBoard - unable to serialize the leaderboard");
			return;
		}

		Debug.Log ("OnIgniteLeaderBoard - leaderboard: " + leaderBoardString);

		// process the leaderboard information
	}

	public void OnIgniteMission (Dictionary<string, object> mission)
	{
		if (mission == null) {
			Debug.Log ("OnIgniteMission - undefined mission");
			return;
		}

		if (mission.Count == 0) {
			Debug.Log ("OnIgniteMission - empty mission");
			return;
		}

		string missionString = FuelSDKCommon.Serialize (mission);

		if (missionString == null) {
			Debug.Log ("OnIgniteMission - unable to serialize the mission");
			return;
		}

		Debug.Log ("OnIgniteMission - mission: " + missionString);

		// process the mission information
			
	}

	public void OnIgniteQuest (Dictionary<string, object> quest)
	{
		
	}

	public void OnIgniteJoinEvent (string eventID, bool joinStatus)
	{
		
	}









	private string m_tournamentID;
	private string m_matchID;

	public void OnCompeteUICompletedWithMatch (Dictionary<string, object> matchInfo)
	{
		if (matchInfo == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - undefined match info");
			return;
		}

		if (matchInfo.Count == 0) {
			Debug.Log ("OnCompeteUICompletedWithMatch - empty match info");
			return;
		}

		string matchInfoString = FuelSDKCommon.Serialize (matchInfo);

		if (matchInfoString == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - unable to serialize match info");
			return;
		}

		Debug.Log ("OnCompeteUICompletedWithMatch - match info: " + matchInfoString);

		object tournamentIDObject = matchInfo["tournamentID"];

		if (tournamentIDObject == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - missing expected tournament ID");
			return;
		}

		if (!(tournamentIDObject is string)) {
			Debug.Log ("OnCompeteUICompletedWithMatch - invalid tournament ID data type: " + tournamentIDObject.GetType ().Name);
			return;
		}

		string tournamentID = (string)tournamentIDObject;

		object matchIDObject = matchInfo["matchID"];

		if (matchIDObject == null) {
			Debug.Log ("OnCompeteUICompletedWithMatch - missing expected match ID");
			return;
		}

		if (!(matchIDObject is string)) {
			Debug.Log ("OnCompeteUICompletedWithMatch - invalid match ID data type: " + matchIDObject.GetType ().Name);
			return;
		}

		string matchID = (string)matchIDObject;

		// Caching match information for later
		m_tournamentID = tournamentID;
		m_matchID = matchID;

		// fake playing a match
		StartCoroutine (PerformMatch (matchInfo));
	}


	private IEnumerator PerformMatch (Dictionary<string, object> matchInfo)
	{
		// fake obtaining match results
		yield return new WaitForSeconds (2.0f);

		Dictionary<string, object> matchResult = new Dictionary<string, object> ();
		matchResult.Add ("matchID", m_matchID);
		matchResult.Add ("tournamentID", m_tournamentID);
		matchResult.Add ("score", "55");

		FuelSDK.SubmitMatchResult (matchResult);

		// return back to multiplayer
		FuelSDK.Launch ();
	}



}
