using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
using System.Text.RegularExpressions;

public class MagTile : MonoBehaviour {
	public GameObject magCamera;
	public GameObject mag1;
	public GameObject mag2;
	public GameObject mag3;
	string stringToEdit;
	int rand2;
	int j=0;
	Vector3 cameraPos;
	Vector3 mag1Pos;
	Vector3 mag2Pos;
	Vector3 mag3Pos;
	public Vector2 magBackgroundPos;
	public Vector2 magBackgroundSize;
	
	public Vector2 magExPos;
	public Vector2 magExSize;
	
	public Vector2 timePos;
	public Vector2 timeSize;
	string magEx = "자석을 자유롭게 이동시켜 자기장의 변화를 관찰해 보세요.";
	
	float beginOnMagTime;
	
	bool playerOnAMagTile = false;
	public static bool checkMagTile = false;
	bool mQuiz;
	public GUISkin S1;
	int rand;
	//문제 바탕창
	public Vector2 questionBoxPosition;
	public Vector2 questionBoxSize;
	
	//문제 번호창 
	public Vector2 questionNumberPosition;
	public Vector2 questionNumberSize;
	
	//문제 난이도 표시창
	public Vector2 questionLODPosition;
	public Vector2 questionLODSize;
	
	//문제 표시 창	
	public Vector2 questionPosition;
	public Vector2 questionSize;
	
	//답 입력창
	public Vector2 answeringPosition;
	public Vector2 answeringSize;
	
	//문제 설명 그림
	public Vector2 explainPicturePosition;
	public Vector2 explainPictureSize;
	
	//문제 타입
	public Vector2 questionTypePosition;
	public Vector2 questionTypeSize;
	
	public Vector2 confirmButtonPosition;
	public Vector2 confirmButtonSize;

	//문제설명그림 바탕
	public Vector2 explainPictureBackgroundPosition;
	public Vector2 explainPictureBackgroundSize;
	
	const int QCount=200;
	string[] questions = new string[QCount];
	string[] qNumber = new string[QCount];
	string[] qType = new string[QCount];
	string[] qAnswer = new string[QCount];
	string[] qLOD = new string[QCount];
	public Texture2D[] qImage = new Texture2D[QCount];
	public Texture2D tDynamicTx;
	public WWW tLoad;
	public string images;
	// Use this for initialization
	
	void Awake()
	{
		checkMagTile = false;
	}
	void Start () {
		checkMagTile = false;
		cameraPos = magCamera.transform.position;
		mag1Pos = mag1.transform.position;
		mag2Pos = mag2.transform.position;
		mag3Pos = mag3.transform.position;
		beginOnMagTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerOnAMagTile)
		{
			if(TimeOver())
			{
				checkMagTile = false;
				magCamera.camera.enabled = false;
				playerOnAMagTile = false;	
				mQuiz = true;
			}
		}
		if(mQuiz)
		{			
			QuestionTile.magTile = true;
			mQuiz=false;
		}
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			QuestionTile._checkQ = true;
			magCamera.transform.position = cameraPos;
			mag1.transform.position = mag1Pos;
			mag2.transform.position = mag2Pos;
			mag3.transform.position = mag3Pos;
			checkMagTile = true;
			magCamera.camera.enabled = true;
			playerOnAMagTile = true;
			beginOnMagTime = Time.realtimeSinceStartup;
		}
	}
	public bool TimeOver(){
		return (Time.realtimeSinceStartup-beginOnMagTime)>10;
	}
	
	void OnGUI()	
	{
		if(playerOnAMagTile)
		{
			GUI.skin = S1;
			GUI.Box(new Rect(magBackgroundPos.x,magBackgroundPos.y,magBackgroundSize.x,magBackgroundSize.y),"");
			GUI.Box(new Rect(magExPos.x,magExPos.y,magExSize.x,magExSize.y),magEx);
			GUI.Label(new Rect(timePos.x,timePos.y+70,timeSize.x,timeSize.y),((int)Time.realtimeSinceStartup-(int)beginOnMagTime)+"초 지남\n제한시간 10초");
		}
		
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
