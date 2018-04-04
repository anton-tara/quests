using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class TextClass : MonoBehaviour {
	public GameObject panel;
	public Font f;
	public GameObject spite;
	private string text = "";
	private List<string> things;
	private GameObject[] bs;//массив кнопок
	private bool isBut = false;//Есть ли кнопки на странице
	private int currentPart = 0; 
	private int currentPage = 1;
	private bool win = false;
	private bool lose = false;


	void Start () {
		things = new List<string> ();
		SaveManager.currentPoints = 0;
		StreamReader sr = new StreamReader("quest"+SaveManager.currentQuest+".txt");
		while (!sr.EndOfStream) {
			foreach (char a in sr.ReadLine().ToCharArray()) {
				if (a == 'n')
					text += "\n";
				else
					text += a;
			}
		}
		string str = Page(Part (0),1);
		GameObject.Find ("text1").GetComponent<Text> ().text = SetImage(str.Split('[')[0]==null?str:str.Split('[')[0]);
	}


	void Update () {
		string now = Page (Part (currentPart), currentPage);
		if (!isBut && now.Contains ("[")) {
			SetButs (now);
			isBut = true;
		} else if (!now.Contains ("[")) {
			isBut = false;
		}
		//анимация выигрыша/проигрыша
		if (GameObject.Find ("winPanel") != null && GameObject.Find ("winPanel").GetComponent<Image>().color.a<141f/255) {
			GameObject g = GameObject.Find ("winPanel");
			g.GetComponent<Image> ().color = new Color (1, 0, 0, g.GetComponent<Image>().color.a+1f/255/*141f/255*/);
		}
		if (GameObject.Find ("losePanel") != null && GameObject.Find ("losePanel").GetComponent<Image>().color.a<141f/255) {
			GameObject g = GameObject.Find ("losePanel");
			g.GetComponent<Image> ().color = new Color (1, 0, 0, g.GetComponent<Image>().color.a+1f/255/*141f/255*/);
		}

	}

	//возвращает словарь номер тега/надпись на кнопке(выбор)
	public Dictionary<string, int> Buttons(string s){
		Dictionary<string, int> buttons = new Dictionary<string, int> ();
		string num = "";
		int n = 0;
		string button = "";
		bool isNum = false;
		for(int k = 1;k<s.Split('[').Length;k++){
			foreach (char a in s.Split('[')[k].ToCharArray()) {
				if (a == '(')
					isNum = true;
				else if (a == ')') {
					isNum = false;
					n = Convert.ToInt32(num.Replace("+",""));
					num = "";
				}
				if (!isNum && a != ']'&&a!=')')
					button += a;
				else if (isNum&&a!='(')
					num += a;
			}
			buttons.Add (button, n);
			button = "";
		}
		return buttons;
	}

	public void SetButs(string str){
		List<string> s = new List<string> ();
		Dictionary<string, int>.Enumerator enumerator = Buttons(str).GetEnumerator();
		while (enumerator.MoveNext()) {
			s.Add(enumerator.Current.Key+"/"+enumerator.Current.Value);
		}
		switch ((str.Split ('[').Length)-1) {
		case 2:
			{	
				bs = new GameObject[2];
				bs[0] = CreateButton (s[0], -90, -250);
				bs[1] = CreateButton (s[1], 90, -250);
				break;
			}
		case 3:
			{	
				bs = new GameObject[3];
				bs[0] = CreateButton (s[0], 0, -175);
				bs[1] = CreateButton (s[1], -88, -285);
				bs[2] = CreateButton (s[2], 88, -285);
				break;
			}
		default:
			break;
		}
	}

	//возвращает текст страницы page из текста с данным тегом
	public string Page(string s, int page){
		return s.Split ('}')[ page - 1] == null ?"" : s.Split('}')[page - 1].Replace("{","");
	}

	//возвращает текст по номеру тега <i>
	public string Part(int i){
		string res = "";
		string choice = "";
		bool b = false;
		bool c = true;
		foreach (char a in text.ToCharArray()) {
			if (a == '<') {
				b = true;
				c = false;
			}
			else if (b&&a!='>') {
				choice += a;
			}
			if (a == '>') {
				if(choice.Equals(i+"")){
					b = false;
					c = true;
					res = "";
				}
				else if(choice.Equals((i+1)+""))
					return res;
				choice = "";
				b = false;
			}
			if(c&&a!='>'){
				res+=a;
			}
		}
		if (res.Equals (text) && i!=0)
			return "";
		if (res.Equals (""))
			res = text.Split ('<') [0];
		return res;
	}

	//создание кнопки
	public GameObject CreateButton(string name, int x, int y){
		GameObject button = new GameObject (name, typeof(Image), typeof(Button));
		button.transform.SetParent(panel.transform);
		button.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 1.75f);
		button.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 1.0f);
		button.GetComponent<Button> ().onClick.AddListener (delegate {
			Press(button);
		});
		RectTransform rt = button.GetComponent<RectTransform> ();
		rt.anchoredPosition = new Vector3 (x,y,1);

		GameText (button.transform, name.Split (' ') [0], name.Split ('/') [0], new Vector3 (0.0175f, 0.01026f, 0.02f),
			new Vector2 (0, 0), new Color (0, 0, 0), 10, 1, 18, 0, 0);
		
		return button;
	}

	//возвращает номер, который прикреплен к кнопке
	public int GetNum(string text, Dictionary<int, string> d){
		Dictionary<int, string>.Enumerator enumerator = d.GetEnumerator();
		while (enumerator.MoveNext()) {
			if (enumerator.Current.Value.Equals (text))
				return enumerator.Current.Key;
		}
		return 0;
	}

	public string SetImage(string str){
		if (!str.Contains ("|"))
			return str;
		string image = str.Split ('|') [1];
		spite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(image);
		return str.Replace (image, "").Replace("|","");
	}

	//удобная инициализация текста
	public void GameText(Transform tr, string name, string t1, Vector3 v1, Vector2 v2, Color c, int fontSize, int min, int max, float w, float h){
		GameObject t = new GameObject (name, typeof(Text));
		t.transform.SetParent(tr);
		t.GetComponent<Text>().text =t1;
		if(w != 0)	t.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, w);
		if(h != 0)	t.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, h);
		t.GetComponent<Text> ().color = c;
		t.GetComponent<Text> ().font = f;
		t.GetComponent<Text> ().fontSize = fontSize;
		t.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		t.GetComponent<Text> ().alignByGeometry = true;
		t.GetComponent<Text> ().resizeTextForBestFit = true;
		t.GetComponent<Text> ().resizeTextMinSize = min;
		t.GetComponent<Text> ().resizeTextMaxSize = max;

		RectTransform r1 = t.GetComponent<RectTransform> ();
		r1.localScale = v1;
		r1.anchoredPosition = v2;
	}

	//есть ли в инвентаре у игрока такая то вещь
	public bool CheckIt(string thing){
		return things.Contains (thing);
	}

	//проиграл ли
	public void Losed(){
		if (!Page (Part (currentPart), currentPage ).Contains("[") &&
			Page (Part (currentPart), currentPage + 1).Equals("") && !win) {
			GameObject pan = new GameObject ("losePanel", typeof(Image));
			pan.GetComponent<Image> ().color = new Color (1, 0, 0, 0/*141f/255*/);
			pan.transform.SetParent (GameObject.Find ("Canvas").transform);
			RectTransform r = pan.GetComponent<RectTransform> ();
			r.position = new Vector3 (0, 0, 0);
			pan.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 4f);
			pan.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 2.41f);

			GameText (pan.transform, "losed", "Вы проиграли \n Попробуйте еще раз", new Vector3 (0.02f, 0.02f, 0.02f), 
				new Vector3 (0, 0f, 0), new Color (0, 0, 0), 10, 10, 20, 200f, 120f);

			lose = true;
		}

	}

	//выиграл ли
	public void Won(){
		if (Page (Part (currentPart), (currentPage + 1)).Equals ("") &&
			Part (currentPart + 1).Equals (Part(0)) && !win) {
			GameObject pan = new GameObject ("winPanel", typeof(Image));
			pan.GetComponent<Image> ().color = new Color (1, 0, 0, 0/*141f/255*/);
			pan.transform.SetParent (GameObject.Find ("Canvas").transform);
			RectTransform r = pan.GetComponent<RectTransform> ();
			r.position = new Vector3 (0, 0, 0);
			pan.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 4f);
			pan.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 2.41f);

			GameText (pan.transform, "win", "Вы победили", new Vector3 (0.02f, 0.02f, 0.02f), 
				new Vector3 (0, 0.66f, 0), new Color (0, 0, 0), 10, 10, 20, 150f, 25f);

			int points = (int)((SaveManager.minPoints * 1.0 / SaveManager.currentPoints) * 100);
			int stars = 0;
			if (points < 66)
				stars = 1;
			else if (points >= 66 && points < 99)
				stars = 2;
			else
				stars = 3;
			
			GameText (pan.transform, "status", "Звание : "+SaveManager.GetStat(stars), new Vector3 (0.02f, 0.02f, 0.02f), 
				new Vector3 (0, -0.9f, 0), new Color (0, 0, 0), 10, 10, 20, 150f, 25f);
			if ((PlayerPrefs.HasKey ("quest" + SaveManager.currentQuest + "Stars") &&
			    PlayerPrefs.GetInt ("quest" + SaveManager.currentQuest + "Stars") < stars) ||
			    !PlayerPrefs.HasKey ("quest" + SaveManager.currentQuest + "Stars")) {
				PlayerPrefs.SetInt ("quest" + SaveManager.currentQuest + "Stars", stars);
				PlayerPrefs.Save ();
			}

			for (int i = 0; i < stars; i++) {
				GameObject star = new GameObject ("star" + i, typeof(SpriteRenderer));
				star.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("star");
				Transform r2 = star.GetComponent<Transform>();
				r2.position = new Vector3(-1.2f+1.2f*i,-0.15f,0);
				r2.localScale = new Vector3 (1f, 1f, 1f);
				star.GetComponent<SpriteRenderer> ().sortingOrder = 2;
				star.transform.SetParent (pan.transform);
			}

			win = true;
		}
	}

	//нажатие на панель
	void OnMouseUpAsButton(){
		if (win || lose) {
			Application.LoadLevel ("Menu");
		}
		string currentText = Part (currentPart);
		int i = 1;
		while (true) {
			if ((Page (currentText, i).Split('|')[0]==null?
				Page (currentText, i):
				Page (currentText, i).Split('|')[0]).Equals(GameObject.Find ("text1").GetComponent<Text> ().text)
				&& !Page (currentText, i + 1).Equals ("")) {
				string str = Page (currentText, i + 1);
				GameObject.Find ("text1").GetComponent<Text> ().text = SetImage(str.Split('[')[0]==null?str:str.Split('[')[0]);
				currentPage = i + 1;
				Won ();
				Losed ();
				return;
			}
			else
				i++;
			if (Page (currentText, i).Equals ("")) {
				Won ();
				Losed ();
				return;
			}
		}

	}
		
	//обработка нажатия на кнопку
	public void Press(GameObject g){
		if (g.name.Split ('/') [0]
			.Split ('+') [0] != null && 
			g.name.Split ('/') [0]
			.Split ('+').Length>1 &&
			g.name.Split ('/') [0]
			.Split ('+') [1] != null &&
		   !CheckIt (g.name.Split ('+') [1])) {
			GameObject.Find(g.name.Split(' ')[0]).GetComponent<Text>().text = "У вас нет "+g.name.Split ('+') [1].Split('/')[0];
			return;
		}
			

		if(g.name.Split ('/') [1].Contains("+"))
			things.Add(g.name.Split('/')[0]);
		
		currentPart = int.Parse(g.name.Split ('/') [1].Replace("+",""));
		string str = Page (Part (currentPart), 1);
		GameObject.Find ("text1").GetComponent<Text> ().text = SetImage (str.Split ('[') [0] == null ? str : str.Split ('[') [0]);
		currentPage = 1;
		if (Page (Part (currentPart), 1).Contains ("["))
			isBut = false;
		SaveManager.currentPoints = SaveManager.currentPoints+1;
		foreach (GameObject go in bs) {
			Destroy (go);
		}
	}
}