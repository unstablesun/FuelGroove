using System.Collections.Generic;

public class FuelListener : FuelSDKListener 
{
	private FuelHandler m_fuelHandler;

	public FuelListener(FuelHandler fuelHandler) {
		m_fuelHandler = fuelHandler;
	}

	public override void OnIgniteEvents (List<object> events)
	{
		m_fuelHandler.OnIgniteEvents (events);
	}

	public override void OnIgniteLeaderBoard (Dictionary<string, object> leaderBoard)
	{
		m_fuelHandler.OnIgniteLeaderBoard (leaderBoard);
	}

	public override void OnIgniteMission (Dictionary<string, object> mission)
	{
		m_fuelHandler.OnIgniteMission (mission);
	}

	public override void OnIgniteQuest (Dictionary<string, object> quest)
	{
		m_fuelHandler.OnIgniteQuest (quest);
	}

	public override void OnIgniteJoinEvent (string eventID, bool joinStatus)
	{
		m_fuelHandler.OnIgniteJoinEvent (eventID, joinStatus);
	}




	public override void OnCompeteUICompletedWithMatch (Dictionary<string, object> matchInfo)
	{
		m_fuelHandler.OnCompeteUICompletedWithMatch (matchInfo);
	}

}