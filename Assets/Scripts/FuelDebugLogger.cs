using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

public class FuelDebugLogger : MonoBehaviour {

	static FuelDebugLogger s_instance;
	public static FuelDebugLogger Instance
	{
		get
		{
			Assert.IsNotNull(s_instance, "no singleton FuelDebugLogger in scene");
			return s_instance;
		}
	}

	// Use this for initialization
	void Awake () {
		
		s_instance = this;
	}


	public void OutputAll ( Dictionary<string, IgniteEvent> eventDict )
	{

		foreach(String cKey in eventDict.Keys)
		{
			Debug.Log("eventDict : cKey = " + cKey);

			IgniteEvent _e = eventDict [cKey];

			IgniteLeaderBoard ILB = (IgniteLeaderBoard)_e.activity;

			string CurrentUserId = ILB.CurrentUserId;

			string debugLogEntry = 
				"Ignite Event" + "\n" +
				"cKey = " + cKey + "\n" +
				"Id = " + _e.Id + "\n" +
				"EventId = " + _e.EventId + "\n" +
				"StartTime = " + _e.StartTime.ToLongDateString () + "\n" +
				"CurrentUserId = " + CurrentUserId + "\n";

			debugLogEntry += "\tLeader Board Entries" + "\n";
			debugLogEntry += "\tId" + "              " + "Rank" +  "  " + "Score" + "   " + "Name" + "    " + "User" + "\n";

			List<LeaderData> LeaderList = ILB.Leaders;

			foreach (LeaderData eventObject in LeaderList) 
			{
				string cUser = eventObject.IsCurrentUser.ToString ();

				debugLogEntry += "\t" + eventObject.Id  + " " + eventObject.Rank + " " + eventObject.Score + " " + eventObject.Name + " " + cUser + "\n";
			}

			Debug.Log (debugLogEntry);

		}
	}
		
}
