using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class About : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string text = "";
		StreamReader sr1 = new StreamReader("about.txt");
		while (!sr1.EndOfStream) {
			foreach (char a in sr1.ReadLine().ToCharArray()) {
				if (a == 'n')
					text += "\n";
				else
					text += a;
			}
		}
		GetComponent<Text> ().text = text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
