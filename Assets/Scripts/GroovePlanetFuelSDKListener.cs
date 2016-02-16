using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

public class GroovePlanetFuelSDKListener : FuelSDKListener 
{

	public override void OnIgniteEvents (List<object> events)
	{
		FuelSDKGroovePlanetIntegration.Instance.OnIgniteEvents (events);
	}

	public override void OnIgniteLeaderBoard (Dictionary<string, object> leaderBoard)
	{
		FuelSDKGroovePlanetIntegration.Instance.OnIgniteLeaderBoard (leaderBoard);
	}

	public override void OnIgniteMission (Dictionary<string, object> mission)
	{
		FuelSDKGroovePlanetIntegration.Instance.OnIgniteMission (mission);
	}

	public override void OnIgniteQuest (Dictionary<string, object> quest)
	{
		FuelSDKGroovePlanetIntegration.Instance.OnIgniteQuest (quest);
	}

	public override void OnIgniteJoinEvent (string eventID, bool joinStatus)
	{
		FuelSDKGroovePlanetIntegration.Instance.OnIgniteJoinEvent (eventID, joinStatus);
	}




	public override void OnCompeteUICompletedWithMatch (Dictionary<string, object> matchInfo)
	{
		FuelSDKGroovePlanetIntegration.Instance.OnCompeteUICompletedWithMatch (matchInfo);
	}

}