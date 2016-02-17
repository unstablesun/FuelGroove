using UnityEngine;
using System;
using System.Collections;

public class IgniteLeaderBoard : IgniteActivity {
	public string CurrentUserId { get; set; }
	public System.Collections.Generic.List<LeaderData> Leaders { get; set; }
	public LeaderBoardRuleData Rule { get; set; }
	public LeaderBoardMetadata Metadata { get; set; }

	public override void Create( System.Collections.Generic.Dictionary<string,object> dataDict ) {

		Debug.Log( "IgniteLeaderBoard:Create");

		base.Create( dataDict );
		if( dataDict.ContainsKey( "currentUserId" ) ) {
			this.CurrentUserId = Convert.ToString( dataDict["currentUserId"] );
		}
		
		if( dataDict.ContainsKey( "rule" ) ) {
			System.Collections.Generic.Dictionary<string,object> leaderBoardRuleDict = dataDict["rule"] as System.Collections.Generic.Dictionary<string,object>;
			this.Rule = LeaderBoardRuleData.ParseFromDictionary( leaderBoardRuleDict );
		}

		this.Leaders = new System.Collections.Generic.List<LeaderData>();
		if( dataDict.ContainsKey( "leaderList" ) ) {
			System.Collections.Generic.Dictionary<string,object> leaderListDict = dataDict["leaderList"] as System.Collections.Generic.Dictionary<string,object>;
			if( leaderListDict.ContainsKey( "leaders" ) ) {
				System.Collections.Generic.List<object> leaderList = leaderListDict["leaders"] as System.Collections.Generic.List<object>;
				int position = 1;
				foreach( object leaderObject in leaderList ) {
					System.Collections.Generic.Dictionary<string,object> leaderDict = leaderObject as System.Collections.Generic.Dictionary<string,object>;
					LeaderData leaderData = LeaderData.ParseFromDictionary( leaderDict );
					if( this.CurrentUserId == leaderData.Id ) {
						leaderData.IsCurrentUser = true;
					}
					leaderData.Rank = position;
					this.Leaders.Add( leaderData );
					position++;
				}
			}
		}

		if( dataDict.ContainsKey( "metadata" ) ) {
			System.Collections.Generic.Dictionary<string,object> metadataDict = dataDict["metadata"] as System.Collections.Generic.Dictionary<string,object>;
			this.Metadata = LeaderBoardMetadata.ParseFromDictionary( metadataDict );
		}
	}
}

public struct LeaderBoardRuleData {
	public string Id { get; set; }
	public string Variable { get; set; }
	public int Kind { get; set; }
	public int Score { get; set; }
	
	public static LeaderBoardRuleData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> leaderBoardRuleDict ) {
		LeaderBoardRuleData leaderBoardRuleData = new LeaderBoardRuleData();
		if( leaderBoardRuleDict.ContainsKey("id") ){
			leaderBoardRuleData.Id = Convert.ToString( leaderBoardRuleDict["id"] );
		}
		if( leaderBoardRuleDict.ContainsKey("variable") ){
			leaderBoardRuleData.Variable = Convert.ToString( leaderBoardRuleDict["variable"] );
		}
		if( leaderBoardRuleDict.ContainsKey("kind") ){
			leaderBoardRuleData.Kind = Convert.ToInt32( leaderBoardRuleDict["kind"] );
		}
		if( leaderBoardRuleDict.ContainsKey("score") ){
			leaderBoardRuleData.Score = Convert.ToInt32( leaderBoardRuleDict["score"] );
		}
		return leaderBoardRuleData;
	}
}

public struct LeaderData {
	public string Id { get; set; }
	public bool IsCurrentUser { get; set; }
	public string Name { get; set; }
	public int Score { get; set; }
	public int Rank { get; set; }
	
	public static LeaderData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> leaderDict ) {
		LeaderData leaderData = new LeaderData();
		if( leaderDict.ContainsKey("id") ){
			leaderData.Id = Convert.ToString( leaderDict["id"] );
		}
		if( leaderDict.ContainsKey("name") ){
			leaderData.Name = Convert.ToString( leaderDict["name"] );
		}
		if( leaderDict.ContainsKey("score") ){
			leaderData.Score = Convert.ToInt32( leaderDict["score"] );
		}
		if( leaderDict.ContainsKey("rank") ){
			leaderData.Rank = Convert.ToInt32( leaderDict["rank"] );
		}
		return leaderData;
	}
}

public struct LeaderBoardMetadata {
	public string Name { get; set; }
	public string GameData { get; set; }
	//public System.Collections.Generic.List<VirtualGoodLeaderBoardData> VirtualGoodList;
	//public VirtualGoodData participationVirtualGood { get; set; }
	
	public static LeaderBoardMetadata ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> metadataDict ) {
		LeaderBoardMetadata leaderBoardMetadata = new LeaderBoardMetadata();
		if( metadataDict.ContainsKey( "name" ) ) {
			leaderBoardMetadata.Name = Convert.ToString( metadataDict["name"] );
		}
		if( metadataDict.ContainsKey( "gamedata" ) ) {
			leaderBoardMetadata.GameData = Convert.ToString( metadataDict["gamedata"] );
		}

		/*
		if( metadataDict.ContainsKey( "virtualGoods" ) ) {
			System.Collections.Generic.List<VirtualGoodLeaderBoardData> virtualGoodList = new System.Collections.Generic.List<VirtualGoodLeaderBoardData>();
			System.Collections.Generic.List<object> virtualGoodListObjects = metadataDict["virtualGoods"] as System.Collections.Generic.List<object>;
			foreach( object virtualGoodObject in virtualGoodListObjects ) {
				System.Collections.Generic.Dictionary<string,object> virtualGoodObjectDict = virtualGoodObject as System.Collections.Generic.Dictionary<string,object>;
				VirtualGoodLeaderBoardData virtualGoodLeaderBoardData = VirtualGoodLeaderBoardData.ParseFromDictionary( virtualGoodObjectDict );
				virtualGoodList.Add( virtualGoodLeaderBoardData );
			}
			leaderBoardMetadata.VirtualGoodList = virtualGoodList;
		}
		*/

		/*
		if( metadataDict.ContainsKey( "participationVirtualGood" ) && !String.IsNullOrEmpty(metadataDict["participationVirtualGood"].ToString())) {
			VirtualGoodData virtualGood = new VirtualGoodData();
			System.Collections.Generic.Dictionary<string,object> virtualGoodDict = metadataDict["participationVirtualGood"] as System.Collections.Generic.Dictionary<string,object>;
			if( virtualGoodDict.ContainsKey( "iconUrl" ) ) {
				virtualGood.IconUrl = Convert.ToString( virtualGoodDict["iconUrl"]);
			}
			if( virtualGoodDict.ContainsKey( "description" ) ) {
				virtualGood.Description = Convert.ToString( virtualGoodDict["description"] );
			}
			if( virtualGoodDict.ContainsKey( "id" ) ) {
				virtualGood.Id = Convert.ToString( virtualGoodDict["id"] );
			}
			if( virtualGoodDict.ContainsKey( "goodId" ) ) {
				virtualGood.GoodId = Convert.ToString( virtualGoodDict["goodId"] );
			}
			virtualGood.Init();
			
			leaderBoardMetadata.participationVirtualGood = virtualGood;
		}
		*/

		return leaderBoardMetadata;
	}
}
