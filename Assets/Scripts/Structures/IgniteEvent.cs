using System;
using System.Collections;

public enum IgniteEventType
{
	none         	= -1,
	leaderBoard 	= 0,
	mission      	= 1,
	quest        	= 2
}

public class IgniteEvent{

	public string Id { get; set; }
	public DateTime StartTime { get; set; }
	public bool Authorized { get; set; }
	public string EventId { get; set; }
	public string State { get; set; }
	public float Score { get; set; }
	public IgniteEventType Type { get; set; }
	public DateTime EndTime { get; set; }
	public IgniteActivityInterface activity;
	public EventMetadata Metadata { get; set; }

	public bool Active {
		get{
			if( State != "active" ) {
				return false;
			}
			if( StartTime.CompareTo(DateTime.UtcNow) <= 0 && EndTime.CompareTo (DateTime.UtcNow) >= 0 ) {
				return true;
			}
			return false;
		}
	}
	
	public bool CommingSoon {
		get{
			if(StartTime.CompareTo (DateTime.UtcNow) >= 0 ) {
				return true;
			}
			return false;
		}
	}
	
	public bool Ended {
		get{
			if( EndTime.CompareTo (DateTime.UtcNow) <= 0 && Score < 1.0f ) {
				return true;
			}
			return false;
		}
	}
	
	public bool Completed {
		get{
			if( EndTime.CompareTo (DateTime.UtcNow) <= 0 && Score >= 1.0f ) {
				return true;
			}
			return false;
		}
	}

	public void Create ( System.Collections.Generic.Dictionary<string,object> eventDict ) {
		if( eventDict.ContainsKey( "id" ) ) {
			this.Id = Convert.ToString( eventDict["id"] );
		}
		if( eventDict.ContainsKey( "startTime" ) ) {
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			long t = Convert.ToInt64 (eventDict["startTime"]);
			this.StartTime = epoch.AddSeconds(t);
		}
		if( eventDict.ContainsKey( "authorized" ) ) {
			this.Authorized = Convert.ToBoolean( eventDict["authorized"] );
		}
		if( eventDict.ContainsKey( "eventId" ) ) {
			this.EventId = Convert.ToString( eventDict["eventId"] );
		}
		if( eventDict.ContainsKey( "state" ) ) {
			this.State = Convert.ToString( eventDict["state"] );
		}
		if( eventDict.ContainsKey( "score" ) ) {
			this.Score = (float)Convert.ToDouble( eventDict["score"] );
		}
		if( eventDict.ContainsKey( "type" ) ) {
			this.Type = (IgniteEventType) Enum.Parse( typeof(IgniteEventType) , Convert.ToString( eventDict["type"] ) );
		}
		if( eventDict.ContainsKey( "endTime" ) ) {
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			long t = Convert.ToInt64 (eventDict["endTime"]);
			this.EndTime = epoch.AddSeconds(t);
		}
		if( eventDict.ContainsKey( "metadata" ) ) {
			System.Collections.Generic.Dictionary<string,object> eventMetadataDict = eventDict["metadata"] as System.Collections.Generic.Dictionary<string,object>;
			EventMetadata eventMetadata = new EventMetadata();
			if( eventMetadataDict.ContainsKey( "imageUrl" ) ) {
				eventMetadata.imageUrl = Convert.ToString( eventMetadataDict["imageUrl"] );
			}
			if( eventMetadataDict.ContainsKey( "name" ) ) {
				eventMetadata.Name = Convert.ToString( eventMetadataDict["name"] );
			}
			if( eventMetadataDict.ContainsKey( "gamedata" ) ) {
				eventMetadata.GameData = Convert.ToString( eventMetadataDict["gamedata"] );
			}
			this.Metadata = eventMetadata;
		}

		switch( this.Type ) {
			case IgniteEventType.leaderBoard:
			FuelSDKGroovePlanetIntegration.Instance.GetLeaderBoard( this.Id );
				activity = new IgniteLeaderBoard();
				break;
			case IgniteEventType.mission:
			FuelSDKGroovePlanetIntegration.Instance.GetMission( this.Id );
				activity = new IgniteMission();
				break;
			case IgniteEventType.quest:
			FuelSDKGroovePlanetIntegration.Instance.GetQuest( this.Id );
				activity = new IgniteQuest();
				break;
			default:
				break;
		}
	}

	public void LoadActivityData( System.Collections.Generic.Dictionary<string,object> dataDict ) {
		if( this.activity != null ) {
			this.activity.Create( dataDict );
		}
	}
}

public struct EventMetadata {
	public string Name { get; set; }
	public string imageUrl { get; set; }
	public string GameData { get; set; }
}
