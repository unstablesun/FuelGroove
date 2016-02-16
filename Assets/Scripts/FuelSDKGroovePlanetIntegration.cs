using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

public class FuelSDKGroovePlanetIntegration : MonoBehaviour 
{

	private int variable_inc = 1;



	#region ------------------------------------- Integration Handler Init -------------------------------------

	static FuelSDKGroovePlanetIntegration s_instance;
	public static FuelSDKGroovePlanetIntegration Instance
	{
		get
		{
			Assert.IsNotNull(s_instance, "no singleton FuelSDKGroovePlanetIntegration in scene");
			return s_instance;
		}
	}

	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
		Assert.IsNull(s_instance, "already have FuelSDKGroovePlanetIntegration instance in scene");

		s_instance = this;


		events = new Dictionary<string, IgniteEvent>();
	}

	void Start()
	{
		FuelSDK.setListener (new GroovePlanetFuelSDKListener());

		GetEvents();
	}

	void OnDestroy()
	{
		Assert.IsTrue(s_instance == this, "we should be the only FuelSDKGroovePlanetIntegration instance in scene");
		s_instance = null;

	}
	#endregion




	#region ===================================== Ignite Send Progress ====================================

	public void SendProgress () {

		Dictionary<string,int> scoreDict = new Dictionary<string, int>();
		scoreDict.Add("value", variable_inc);

		Dictionary<string,object> progressDict = new Dictionary<string, object>();
		progressDict.Add("bronze", scoreDict);//these keys should match the variable names

		List<object> tags = new List<object>();
		tags.Add("BronzeFilter");
		tags.Add("bronzeSong1");

		//List<object> methodParams = new List<object>();
		//methodParams.Add( progressDict );
		//methodParams.Add( tags );
		//bool success = FuelSDK.ExecMethod("SendProgress", methodParams);
		//if(success == true) {
			//Your progress has been successfully updated
		//}

		FuelSDK.SendProgress( progressDict , tags );
	}

	#endregion


	#region ------------------------------------- Ignite Events -------------------------------------

	private Dictionary<string,IgniteEvent> events;

	public void GetEvents() {
		Debug.Log( "FuelSDKGrooveIntegration - GetEvents called." );
		List<object> tags = GetEventTags();
		FuelSDK.GetEvents(tags);
	}

	private List<object> GetEventTags () {
		List<object> tags = new List<object>();

		tags.Add("BronzeFilter");
		tags.Add("bronzeSong1");
		tags.Add("bronzeSong2");

		return tags;
	}

	public void OnIgniteEvents( List<object> eventsList ) {
		Debug.Log( "FuelSDKGrooveIntegration - OnIgniteEventsReceive. event count: "+eventsList.Count );
		foreach(object eventObject in eventsList ) {
			Dictionary<string,object> eventDict = eventObject as Dictionary<string,object>;
			string eventId = Convert.ToString( eventDict["id"] );
			if( !events.ContainsKey( eventId ) ) {
				events.Add( eventId , new IgniteEvent() );
			}
			events[eventId].Create( eventDict );
		}
		/*
		List<EventData> events = new List<EventData>();
		foreach(object eventObject in eventsList ) {
			Dictionary<string,object> eventDict = eventObject as Dictionary<string,object>;
			if( eventDict.Count > 0 ) {
				EventData eventData = EventData.ParseFromDictionary( eventDict );
				events.Add( eventData );
			}
		}
		IgniteUIManager.PopulateIgniteEvents( events );
		*/
	}

	#endregion


	#region ------------------------------------- Ignite LeaderBoard -------------------------------------
			
	public void GetLeaderBoard(string boardID) {
		Debug.Log( "FuelSDKGrooveIntegration - GetLeaderBoard. boardID: " + boardID );
		FuelSDK.GetLeaderBoard( boardID );
	}

	public void OnIgniteLeaderBoard( Dictionary<string,object> leaderBoardDict ) {
		Debug.Log( "FuelSDKGrooveIntegration - OnIgniteLeaderBoard called." );
		if( leaderBoardDict.ContainsKey( "id" ) ) {
			string id = Convert.ToString( leaderBoardDict["id"] );
			events[id].LoadActivityData( leaderBoardDict );
		}
	}

	#endregion

	#region ------------------------------------- Ignite Mission -------------------------------------

	public void GetMission(string missionID) {
		Debug.Log( "FuelSDKGrooveIntegration - GetMission. missionID: "+missionID );
		FuelSDK.GetMission( missionID );
	}

	public void OnIgniteMission(  Dictionary<string,object> missionDict  ) {
		Debug.Log( "FuelSDKGrooveIntegration - OnIgniteMission called." );
		if( missionDict.ContainsKey( "id" ) ) {
			string id = Convert.ToString( missionDict["id"] );
			events[id].LoadActivityData( missionDict );
		}
	}

	#endregion

	#region ------------------------------------- Ignite Quest -------------------------------------

	public void GetQuest(string questID) {
		Debug.Log( "FuelSDKTripleTownIntegration - GetQuest. questID: "+questID );
		FuelSDK.GetQuest( null );
	}

	public void OnIgniteQuest( Dictionary<string, object> quest ) {
		Debug.Log( "FuelSDKTripleTownIntegration - OnIgniteQuest called." );
	}

	#endregion




	#region ------------------------------------- Ignite Join Event --------------------------------
	public void OnIgniteJoinEvent (string eventID, bool joinStatus)
	{

	}
	#endregion







	/*--------------------------------------------------------------------------------------------*/
	#region ------------------------------------- NON Ignite Stuff --------------------------------


	public void ChangeUser()
	{
		FuelSDK.Launch ();
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
	#endregion


}