using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.IO;  
public class SolveTile : MonoBehaviour {
	
	public bool highAnswer;
	public bool midAnswer;
	public bool lowAnswer;
	
	
	
	public Vector2 solveBackGroundPos;
	public Vector2 solveBackGroundSize;
	
	public Vector2 solveNumPos;
	public Vector2 solveNumSize;
	
	public Vector2 lODSize;
	public Vector2 lODPos;
	
	public Vector2 exPos;
	public Vector2 exSize;
	
	
	public int sLength=9;
	
	public GUISkin S1;
	
	bool playerOnATile;
	public static bool checkOnASolveTIle = false;
	private int rand=5;
	int j=0;
	
	public static int countSolve=0;
	public static string[] haveNum = new string[100];
	public static string[] haveLOD = new string[100];
	public static string[] haveExplain = new string[100];
	void Awake(){
		checkOnASolveTIle = false;
		countSolve = 0;
		for(int i = 0 ; i < sLength ; i++)
		{
			SSolve[i] = new SubjectiveSolve();
		}
		for(int i=0;i<100;i++)
		{
			haveNum[i] = null;
			haveLOD[i] = null;
			haveExplain[i] = null;
		}
		
	}
	public Vector2 confirmButtonPosition;
	public Vector2 confirmButtonSize;
	
	public string num;
	public string explain;
	public string lOD;
	
	public class SubjectiveSolve { //주관식 풀이 클래스
		string explain=null;//문제
		string lOD;//난이도 Level of difficulty
		string number;//문제 번호
		public string Explain{
			get;
			set;
		}
		
		public string LOD{
			get;
			set;
		}
		public string Number{
			get;
			set;
		}
	
	}
	SubjectiveSolve[] SSolve = new SubjectiveSolve[100];
	
	
	private bool LoadFile(string fileName,int aCount)
	{
		string line = System.IO.File.ReadAllText("Assets/TxtFile/"+fileName);
		//string line = System.IO.File.ReadAllText(fileName);
		//string line = System.IO.File.ReadAllText(@"C:\"+fileName);
		string[] answer = line.Split(',');
		
		aCount--;
		aCount = aCount *3;
		j = aCount;
		
		num = answer[j];
		explain = answer[j+1];
		lOD =  answer[j+2];
		
		return true;
	}
	
	private bool LoadFile(string fileName)
	{
		//string line = System.IO.File.ReadAllText(@"C:\"+fileName);
		//string line = System.IO.File.ReadAllText(fileName);
		string line = System.IO.File.ReadAllText("Assets/TxtFile/"+fileName);
		string[] answer = line.Split(',');
		for(int i=0;i<answer.Length;i=i+3)
		{
			print ("num"+answer[i]);
			print ("explain"+answer[i+1]);
			print ("lOD"+answer[i+2]);
		}
		return true;
	}
	
	// Use this for initialization
	void Start () 
	{
		//LoadFile("A_L.txt");
	}
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			//rand = UnityEngine.Random.Range(0,120);
			playerOnATile = true;
			checkOnASolveTIle = true;
			if(highAnswer)
			{
				rand = UnityEngine.Random.Range(0,25);
				//LoadFile("A_H.txt");
				LoadFile("A_H.txt",rand);
			}
			if(midAnswer)
			{
				rand = UnityEngine.Random.Range(0,138);
				//LoadFile("A_M.txt");
				LoadFile("A_M.txt",rand);
			}
			if(lowAnswer)
			{
				rand = UnityEngine.Random.Range(0,84);
				//LoadFile("A_L.txt");
				LoadFile("A_L.txt",rand);
			}
		}
	}
	

	// Update is called once per frame
	void Update () {
		/*
		if(Input.GetKeyDown(KeyCode.Mouse0)&&playerOnATile)
		{
			countSolve++;
			playerOnATile = false;				
		}
		*/
	}
	
	void OnGUI(){
		GUI.depth=1;
		GUI.skin = S1;
		if(playerOnATile)
		{
			haveNum[countSolve]=num;
			haveLOD[countSolve]=lOD;
			haveExplain[countSolve]=explain;
			GUI.Box(new Rect(solveBackGroundPos.x,solveBackGroundPos.y,solveBackGroundSize.x,solveBackGroundSize.y),"");
			GUI.Box (new Rect(solveNumPos.x,solveNumPos.y,solveNumSize.x,solveNumSize.y),num);
			GUI.Box(new Rect(lODPos.x,lODPos.y,lODSize.x,lODSize.y),lOD);
			GUI.Box(new Rect(exPos.x,exPos.y,exSize.x,exSize.y),explain);
			if(GUI.Button(new Rect(confirmButtonPosition.x,confirmButtonPosition.y,confirmButtonSize.x,confirmButtonSize.y),"확인"))
			{
				playerOnATile = false;	
				checkOnASolveTIle = false;
				countSolve++;
			}	
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
