using System;
using System.Collections;

public class IgniteMission : IgniteActivity {

	public System.Collections.Generic.Dictionary<string,MissionRuleData> Rules { get; set; }
	public MissionMetadata Metadata { get; set; }

	public bool AllRulesCompleted {
		get {
			return RulesCompleted == Rules.Count;
		}
	}
	
	public int RulesCompleted {
		get {
			int rulesCompleted = 0;
			if( Rules != null ) {
				foreach( MissionRuleData missionRule in Rules.Values ) {
					if( missionRule.Progress >= 1 ) {
						rulesCompleted++;
					}
				}
			}
			return rulesCompleted;
		}
	}
	
	public bool Completed {
		get {
			return base.Progress >= 1;
		}
	}

	public override void Create( System.Collections.Generic.Dictionary<string,object> dataDict ) {
		base.Create( dataDict );

		if( dataDict.ContainsKey( "metadata" ) ) {
			System.Collections.Generic.Dictionary<string,object> missionMetadataDict = dataDict["metadata"] as System.Collections.Generic.Dictionary<string,object>;
			this.Metadata = MissionMetadata.ParseFromDictionary(missionMetadataDict);
		}
		if( dataDict.ContainsKey("rules") ) {
			this.Rules = new System.Collections.Generic.Dictionary<string, MissionRuleData>();
			System.Collections.Generic.List<object> rulesList = dataDict["rules"] as System.Collections.Generic.List<object>;
			foreach(object rule in rulesList ) {
				System.Collections.Generic.Dictionary<string,object> ruleDict = rule as System.Collections.Generic.Dictionary<string, object>;
				MissionRuleData ruleData = MissionRuleData.ParseFromDictionary( ruleDict );
				this.Rules.Add( ruleData.Id, ruleData);
			}
		}
	}

}

#region ===================================== Mission Meta Data ====================================

public struct MissionMetadata {
	public string Name { get; set; }
	public string GameData { get; set; }
	//public VirtualGoodData VirtualGood { get; set; }
	
	public static MissionMetadata ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> missionMetadataDict ) {
		MissionMetadata missionMetadata = new MissionMetadata();
		if( missionMetadataDict.ContainsKey( "name" ) ) {
			missionMetadata.Name = Convert.ToString( missionMetadataDict["name"] );
		}
		if( missionMetadataDict.ContainsKey( "virtualGood" ) ) {
			/*
			VirtualGoodData virtualGood = new VirtualGoodData();
			System.Collections.Generic.Dictionary<string,object> virtualGoodDict = missionMetadataDict["virtualGood"] as System.Collections.Generic.Dictionary<string,object>;
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
			missionMetadata.VirtualGood = virtualGood;
			*/
		}
		if( missionMetadataDict.ContainsKey( "gamedata" ) ) {
			missionMetadata.GameData = Convert.ToString( missionMetadataDict["gamedata"] );
		}
		return missionMetadata;
	}
}

#endregion

#region ===================================== Mission Rule ====================================

public enum MissionRuleType {
	incremental = 0,
	spot       	= 1,
}

public struct MissionRuleData {
	public string Id { get; set; }
	public int Score { get; set; }
	public int Target { get; set; }
	public bool Achieved { get; set; }
	public string Variable { get; set; }
	public MissionRuleType Kind { get; set; }
	public MissionRuleMetadata Metadata { get; set; }
	
	public float Progress {
		get {
			double progressValue = (Target > 0)?Math.Round( (double)Score/(double)Target , 3 ):0;
			if( progressValue > 1 ) {
				progressValue = 1;
			}
			return (float)progressValue;
		}
	}
	
	public bool Complete {
		get {
			return Progress == 1.0f;
		}
	}
	
	public static MissionRuleData ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> ruleDict ) {
		MissionRuleData ruleData = new MissionRuleData();
		if( ruleDict.ContainsKey("id") ) {
			ruleData.Id = Convert.ToString( ruleDict["id"] );
		}
		if( ruleDict.ContainsKey("score") ) {
			ruleData.Score = Convert.ToInt32( ruleDict["score"] );
		}
		if( ruleDict.ContainsKey("target") ) {
			ruleData.Target = Convert.ToInt32( ruleDict["target"] );
		}
		if( ruleDict.ContainsKey("achieved") ) {
			ruleData.Achieved = Convert.ToBoolean( ruleDict["achieved"] );
		}
		if( ruleDict.ContainsKey("variable") ) {
			ruleData.Variable = Convert.ToString( ruleDict["variable"] );
		}
		if( ruleDict.ContainsKey("kind") ) {
			ruleData.Kind = (MissionRuleType) Enum.Parse( typeof(MissionRuleType) , Convert.ToString( ruleDict["kind"] ) );
		}
		if( ruleDict.ContainsKey( "metadata" ) ) {
			System.Collections.Generic.Dictionary<string,object> ruleMetadataDict = ruleDict["metadata"] as System.Collections.Generic.Dictionary<string,object>;
			ruleData.Metadata = MissionRuleMetadata.ParseFromDictionary(ruleMetadataDict);
		}
		return ruleData;
	}
}

public struct MissionRuleMetadata {
	public string Name { get; set; }
	public string GameData { get; set; } 
	
	public static MissionRuleMetadata ParseFromDictionary ( System.Collections.Generic.Dictionary<string,object> metadataDict ) {
		MissionRuleMetadata ruleDataMetadata = new MissionRuleMetadata();
		if( metadataDict != null ) {
			if( metadataDict.ContainsKey( "name" ) ) {
				ruleDataMetadata.Name = Convert.ToString( metadataDict["name"] );
			}
			if( metadataDict.ContainsKey( "gamedata" ) ) {
				ruleDataMetadata.GameData = Convert.ToString( metadataDict["gamedata"] );
			}
		}
		return ruleDataMetadata;
	}
}

#endregion
