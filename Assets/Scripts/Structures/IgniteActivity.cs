using System;
using System.Collections;

public interface IgniteActivityInterface {
	
	void Create ( System.Collections.Generic.Dictionary<string,object> dataDict );
}

public class IgniteActivity : IgniteActivityInterface {

	public string Id { get; set; }
	public float Progress { get; set; }

	public virtual void Create ( System.Collections.Generic.Dictionary<string,object> dataDict ) {
		if( dataDict.ContainsKey( "id" ) ) {
			this.Id = Convert.ToString( dataDict["id"] );
		}
		if( dataDict.ContainsKey( "progress" ) ) {
			this.Progress = (float)Math.Round( Convert.ToDouble(dataDict["progress"]), 2);
		}
	}
}
