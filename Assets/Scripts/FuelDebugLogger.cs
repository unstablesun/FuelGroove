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


	public void OutputAll ( Dictionary<string,IgniteEvent> eventDict )
	{

		foreach(String cKey in eventDict.Keys)
		{
			Debug.Log("eventDict : cKey = " + cKey);

			IgniteEvent _e = eventDict [cKey];

			IgniteLeaderBoard ILB = (IgniteLeaderBoard)_e.activity;

			string CurrentUserId = ILB.CurrentUserId;

			Debug.Log 
			(
				"Ignite Event" + "\n" + 
				"cKey = " + cKey + "\n" + 
				"Id = " + _e.Id + "\n" + 
				"EventId = " + _e.EventId + "\n" + 
				"StartTime = " + _e.StartTime.ToLongDateString () + "\n" +
				"CurrentUserId = " + CurrentUserId + "\n"
			);


			List<LeaderData> LeaderList = ILB.Leaders;

			foreach (LeaderData eventObject in LeaderList) 
			{
				String cUser = eventObject.IsCurrentUser.ToString ();
				Debug.Log 
				(
					"\tLeaderData Entry" + "\n" + 
					"\tcurrent user = " + cUser + "\n" + 
					"\tId = " + eventObject.Id + "\n" + 
					"\tName = " + eventObject.Name + "\n" + 
					"\tRank = " + eventObject.Rank + "\n" + 
					"\tScore = " + eventObject.Score + "\n"
				);
			}
		}
	}
		
}
