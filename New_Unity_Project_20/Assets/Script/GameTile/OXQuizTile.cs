using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
using System.Text.RegularExpressions;
public class OXQuizTile : MonoBehaviour {
	bool onOXQuiz = false;
	
	public Vector2 QuestionPos;
	public Vector2 QuestionSize;
	
	public Vector2 oXQuestionPos;
	public Vector2 oXQuestionSize;
	
	public Vector2 oButtonPos;
	public Vector2 oButtonSize;
	
	public Vector2 xButtonPos;
	public Vector2 xButtonSize;
	
	public Vector2 confirmButtonPos;
	public Vector2 confirmButtonSize;
	
	public Texture2D OX;
	public Texture2D pass;
	public Texture2D fail;
	
	public Texture2D oButton;
	public Texture2D xButton;
	public Vector2 imagePos;
	public Vector2 imageSize;
	
	bool _pass = false;
	bool _fail = false;
	
	public GUISkin S1;
	public GUISkin S2;
	string q;//question
	string a;//answer
	int j;
	int rand;
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			onOXQuiz = true;
			rand = UnityEngine.Random.Range(1,10);
			LoadFile("OXQuiz.txt",rand);
		}
	}
	private bool LoadFile(string fileName, int qCount)
	{
		//images = "file://"+ Application.dataPath +"/QuestionImage/No.00"+num+".png";
		//string line = System.IO.File.ReadAllText(@"C:\"+fileName);
		string line = System.IO.File.ReadAllText("Assets/TxtFile/"+fileName);
		print (line);
		string[] question = line.Split(',');
		qCount--;
		qCount = qCount *2;
		j = qCount;
		q = question[j];
		a = question[j+1];
		q = q.Trim();
		a = a.Trim();
		print ("q:"+q);
		print ("a:"+a);
		return true;
	}
	void CheckAnswer(string aw)
	{
		print ("checkanswer_aw:"+aw);
		print ("checkAnswer_a:"+a);
		if(a==aw)
		{
			_pass = true;
			MainGUI.addHighScore = true;
		}
		else
		{
			MainGUI.subScore = true;
			_fail = true;
		}
	}
	
	void OnGUI()
	{
		GUI.skin = S1;
		if(onOXQuiz)
		{			
			GUI.Box(new Rect(oXQuestionPos.x,oXQuestionPos.y,oXQuestionSize.x,oXQuestionSize.y),"");
			GUI.Label(new Rect(QuestionPos.x,QuestionPos.y,QuestionSize.x,QuestionSize.y),q);
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y,imageSize.x,imageSize.y),OX);
			if(GUI.Button(new Rect(oButtonPos.x,oButtonPos.y,oButtonSize.x,oButtonSize.y),oButton))
			{
				CheckAnswer("O");
				onOXQuiz = false;
				
			}
			if(GUI.Button(new Rect(xButtonPos.x,xButtonPos.y,xButtonSize.x,xButtonSize.y),xButton))
			{
				CheckAnswer("X");
				onOXQuiz = false;
				
			}
		}
		if(_pass)
		{
			GUI.skin = S2;
			GUI.Box(new Rect(oXQuestionPos.x,oXQuestionPos.y,oXQuestionSize.x,oXQuestionSize.y),"");
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y+110,imageSize.x,imageSize.y),pass);
			if(GUI.Button(new Rect(confirmButtonPos.x,confirmButtonPos.y,confirmButtonSize.x,confirmButtonSize.y),"확인"))
			{
				_pass = false;
			}
		}
		if(_fail)
		{
			GUI.skin = S2;
			GUI.Box(new Rect(oXQuestionPos.x,oXQuestionPos.y,oXQuestionSize.x,oXQuestionSize.y),"");
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y+110,imageSize.x,imageSize.y),fail);
			if(GUI.Button(new Rect(confirmButtonPos.x,confirmButtonPos.y,confirmButtonSize.x,confirmButtonSize.y),"확인"))
			{
				_fail = false;
			}
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseOver()
	{	
		renderer.material.color = new Color(2f, 2f, 2f);//버튼위에 마우스가 있을시 밝아짐		 
	}

	void OnMouseExit() {
        renderer.material.color = Color.white;//마우스가 버튼을 벗어났을때 기존의 색 으로 돌아옴
    }
	

	void OnMouseUp()
	{
		if(AppDemo.checkPortal)
		{
			AppDemo.PlayerTileNumber=Int32.Parse(gameObject.name);
			AppDemo.selPos = transform.position;
			AppDemo.selectPosition = true;
		}
	}	
}
