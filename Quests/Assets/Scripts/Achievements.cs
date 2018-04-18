using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//GameObject.Find ("status2").GetComponent<Text> ().text = PlayerPrefs.GetString ("quest2Status")==null?"Никто":PlayerPrefs.GetString ("quest2Status");
		if(SaveManager.win==null) return;
		int stars1 = PlayerPrefs.GetInt ("quest1Stars");
		//int stars2 = PlayerPrefs.GetInt ("quest2Stars");
		for(int i = 1;i <= 3;i++){
			GameObject.Find ("star1" + i).GetComponent<SpriteRenderer> ().sortingOrder = i > stars1 ? -1 : 2;
			//GameObject.Find ("star2" + i).GetComponent<SpriteRenderer> ().sortingOrder = i > stars2 ? -1 : 2;
		}
		GameObject.Find ("status1").GetComponent<Text> ().text = stars1==0?"Никто":SaveManager.GetStat(stars1);
	}

}
