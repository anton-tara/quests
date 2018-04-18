using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class About : MonoBehaviour {

	void Start () {
		string text = "";
		string line; 
		StreamReader reader; 
		TextAsset reader_file = Resources.Load<TextAsset>("about"); 
		if (reader_file != null) 
		{ 
			using (reader = new StreamReader(new MemoryStream(reader_file.bytes))) 
			{ 
				while ((line = reader.ReadLine()) != null) 
				{ 
					foreach (char a in line) {
						if (a == 'n')
							text += "\n";
						else
							text += a;
					}
				} 
			} 
		} 

		GetComponent<Text> ().text = text;
	}
}
