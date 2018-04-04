using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

	public Sprite layer_blue, layer_red;

	void Start(){


	}

	void OnMouseDown(){
		GetComponent <SpriteRenderer> ().sprite = layer_red==null?GetComponent <SpriteRenderer> ().sprite:layer_red;
	}

	void OnMouseUp(){
		GetComponent <SpriteRenderer> ().sprite = layer_blue==null?GetComponent <SpriteRenderer> ().sprite:layer_blue;
	}

	void OnMouseUpAsButton(){
		switch (gameObject.name) {
		case "Play":
			{
				Application.LoadLevel ("Menu");
				break;
			}
		case "Home":
			{
				Application.LoadLevel ("Main");
				break;
			}
		case "Achievements":
			{
				Application.LoadLevel ("Achievements");
				break;}
		case "About":
			{
				Application.LoadLevel ("About");
				break;}
		case "escape from the castle":
			{
				SaveManager.currentQuest = 1;
				SaveManager.minPoints = 3;
				SaveManager.win = new Dictionary<int,string> ();
				SaveManager.win.Add (1, "Паж");
				SaveManager.win.Add (2, "Рыцарь");
				SaveManager.win.Add (3, "Герцог");
				Application.LoadLevel ("Quests");
				break;
			}
		/*case "Будущий квест":
			{
				SaveManager.currentQuest = 2;
				SaveManager.minPoints = 2;
				SaveManager.win = new Dictionary<int,string> ();
				SaveManager.win.Add (1, "Лох");
				SaveManager.win.Add (2, "Пипец");
				SaveManager.win.Add (3, "Крут");
				Application.LoadLevel ("Quests");
				break;
			}*/
		}

	}
}

public static class SaveManager{
	public static int currentQuest = 0;
	public static int currentPoints = 0;
	public static int minPoints = 0;
	public static Dictionary<int, string> win;

	public static string GetStat(int i){
		List<string> s = new List<string> ();
		Dictionary<int, string>.Enumerator enumerator = win.GetEnumerator();
		while (enumerator.MoveNext()) {
			if (enumerator.Current.Key == i)
				return enumerator.Current.Value;
		}
		return "";
	}
}


