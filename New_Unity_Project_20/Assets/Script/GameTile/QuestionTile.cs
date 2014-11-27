using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.IO;  




public class QuestionTile : MonoBehaviour {
	
	public class SubjectiveQuestion { //주관식 문제 클래스
		string question=null;//문제
		string answer=null;//답
		string lOD=null;//난이도 Level of difficulty
		string number=null;//문제 번호
		string qType=null;//문제 유형
		public Texture2D qImage=null;//문제 이미지
		
		public string Question{
			get;
			set;
		}
		public string Answer{
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
		public string QType{
			get;
			set;
		}
	
	}
	bool correct = false;
	bool wrong = false;
	public Texture2D correctImage;
	public Texture2D wrongImage;
	public Vector2 imagePos;
	public Vector2 imageSize;
	public static bool _checkQ = false;
	bool magTile2;
	public bool highQuestion;
	public bool midQuestion;
	public bool lowQuestion;
	public string tileQLOD;
	public int[] highQ = new int[100];
	public int[] midQ = new int[100];
	public int[] lowQ = new int[100];
	
	public static string[] haveNum = new string[100];
	public static string[] haveLOD = new string[100];
	public static string[] haveScore = new string[100];
	public static string[] haveQuestion = new string[100];
	public static int[] haveUpgrade = new int[100];
	public static Texture2D[] haveImage = new Texture2D[100];
	public static bool magTile = false;
	
	void Awake()
	{
		_checkQ = false;
		magTile = false;
		countQuestion=0;
		for(int i = 0 ; i < 100 ; i++)
		{
			haveNum[i]=null;
			haveLOD[i]=null;
			haveScore[i]=null;
			haveQuestion[i]=null;
			haveUpgrade[i]=0;
			haveImage[i]=null;			
		}
	}
	
	public Texture2D[] questionImage = new Texture2D[8];
	public bool questionTile;
	public GUISkin QuestionSkin;
	private FileInfo theQuestionFile = null;
	private StreamReader rD = null;
	private string text = "";
	int rand2;
	//문제 배열 
	public int qLength;
	private string answer;
	
	public string stringToEdit;
	bool confirm;
	bool playerOnTheQuestionTile;
	//GUI
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
	
	public static int countQuestion=0;
	
	private int rand=5;
	int j=0;
	const int QCount = 154;
	
	
	SubjectiveQuestion[] SQuestion = new SubjectiveQuestion[QCount];
	string[] questions = new string[QCount];
	string[] qNumber = new string[QCount];
	string[] qType = new string[QCount];
	string[] qAnswer = new string[QCount];
	string[] qLOD = new string[QCount];
	public Texture2D[] qImage = new Texture2D[QCount];
	
	/*
	private bool LoadFile(string fileName)
	{
    // Handle any problems that might arise when reading the text
    try
    {
        string line;
        // Create a new StreamReader, tell it which file to read and what encoding the file
        // was saved as
        StreamReader theReader = new StreamReader(fileName, Encoding.Default);
 
        // Immediately clean up the reader after this block of code is done.
        // You generally use the "using" statement for potentially memory-intensive objects
        // instead of relying on garbage collection.
        // (Do not confuse this with the using directive for namespace at the 
        // beginning of a class!)
        using (theReader)
        {
            // While there's lines left in the text file, do this:
            do
            {
                line = theReader.ReadLine();
 
                if (line != null)
                {
                    
                    string[] entries = line.Split('\n');
					string[] files = new string[entries.Length*5];
					
					for(int i=0;i<entries.Length;i++)
					{
						files[i] = entries[i].Split(',');	
					}
						
					if(files.Length >0)
					{
						for(int i=0;i<files.Length;i++)
						{
							SQuestion[i].Number = files[j];
							SQuestion[i].Question = files[j+1];
							SQuestion[i].Answer = files[j+2];
							SQuestion[i].LOD = files[j+3];
							SQuestion[i].QType = files[j+4];
							j=j+5;
						}
							return true;							
					}					
                }
            }
            
            while (line != null);
 
            // Done reading, close the reader and return true to broadcast success    
            theReader.Close();
            return true;
            }
        }
 
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
            return false;
        }
    }
	*/
	
	
	
	
	private bool LoadFile(string fileName, int qCount)
	{
		//images = "file://"+ Application.dataPath +"/QuestionImage/No.00"+num+".png";
		//string line = System.IO.File.ReadAllText(@"C:\"+fileName);
		string line = System.IO.File.ReadAllText("Assets/TxtFile/"+fileName);
		string[] question = line.Split(',');
		qCount--;
		qCount = qCount *5;
		j = qCount;
		qNumber[0] = question[j];
		questions[0] = question[j+1].Trim();
		qAnswer[0] =  question[j+2].Trim();
		qLOD[0] = question[j+3].Trim();
		qType[0] = question[j+4].Trim();
		//0:문제숫자 1:문제 2:정답 3:난이도4:문제타입
		//파일에 문제 번호를 저장한 qNumber[0]에 숫자만 추출하여 rand2에 저장
		//rand2를 이미지를 불러오는 인덱스로 사용
		/*
		qAnswer[0] = qAnswer[0].Trim();
		qLOD[0] = qLOD[0].Trim();
		*/
		qNumber[0] = Regex.Replace(qNumber[0],@"\D","");
		rand2 = Int32.Parse(qNumber[0]);
		
		
		return true;
	}
	
	
	
	private bool Load(string fileName)
	{
		
		
    // Handle any problems that might arise when reading the text
    try
    {
        string line;
        // Create a new StreamReader, tell it which file to read and what encoding the file
        // was saved as
        StreamReader theReader = new StreamReader(fileName, Encoding.Default);
 
        // Immediately clean up the reader after this block of code is done.
        // You generally use the "using" statement for potentially memory-intensive objects
        // instead of relying on garbage collection.
        // (Do not confuse this with the using directive for namespace at the 
        // beginning of a class!)
        using (theReader)
        {
            // While there's lines left in the text file, do this:
            do
            {
                line = theReader.ReadLine();
                if (line != null)
                {
                    // Do whatever you need to do with the text line, it's a string now
                    // In this example, I split it into arguments based on comma
                    // deliniators, then send that array to DoStuff()
                    string[] entries = line.Split(',');
                    if (entries.Length > 0)
					{	int i=0;
						for(;i<entries.Length;i++)
						{	
							SQuestion[i].Number = entries[j];
							SQuestion[i].Question = entries[j+1];
							SQuestion[i].Answer = entries[j+2];
							SQuestion[i].LOD = entries[j+3];
							SQuestion[i].QType = entries[j+4];
							j=j+5;
							
							
						}
						return true;
					}
                        
					
					
                }
            }
            while (line != null);
 
            // Done reading, close the reader and return true to broadcast success    
            theReader.Close();
            return true;
            }
        }
 
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
            return false;
        }
    }
	
	
	
	
	
	
	public Texture2D tDynamicTx;
	public WWW tLoad;
	public string images;
	
	// Use this for initialization
	void Start () {
		stringToEdit="";		
	}
	
	void LoadImage(int num)
	{
		num++;
		if(num<10)
		{			
			images = "file://"+ Application.dataPath +"/QImageFiles/No.00"+num+".png";
			tLoad= new WWW(images);
				
			qImage[0] = new Texture2D(100,200);
			tLoad.LoadImageIntoTexture(qImage[0]);
		}
		else if(num<100)
		{
			images = "file://"+ Application.dataPath +"/QImageFiles/No.0"+num+".png";
			tLoad= new WWW(images);
				
			qImage[0] = new Texture2D(100,200);
			tLoad.LoadImageIntoTexture(qImage[0]);
		}
		else if(num>=100)
		{
			images = "file://"+ Application.dataPath +"/QImageFiles/No."+num+".png";
			tLoad= new WWW(images);
				
			qImage[0] = new Texture2D(100,200);
			tLoad.LoadImageIntoTexture(qImage[0]);
		}
	}
	void LoadImages1()
	{			
		for(int i=1;i<10;i++)
		{
			if(i==2||i==4)
				continue;
			
			images = "file://"+ Application.dataPath +"/QuestionImage/No.00"+i+".png";
			tLoad= new WWW(images);
			qImage[i] = new Texture2D(50,50);
			tLoad.LoadImageIntoTexture(qImage[i]);	
		}		
	}
	void LoadImages2()
	{
		for(int i=10;i<100;i++)
		{			
			images = "file://"+ Application.dataPath +"/QuestionImage/No.0"+i+".png";
			tLoad= new WWW(images);
			qImage[i] = new Texture2D(50,50);
			tLoad.LoadImageIntoTexture(qImage[i]);	
		}
	}
	void LoadImages3()
	{
		for(int i=100;i<qImage.Length;i++)
		{
			images = "file://"+ Application.dataPath +"/QuestionImage/No."+i+".png";
			tLoad= new WWW(images);
			
			qImage[i] = new Texture2D(50,50);
			tLoad.LoadImageIntoTexture(qImage[i]);	
			if(i>=154)
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(magTile)
		{
			rand = UnityEngine.Random.Range(1,30);
			LoadFile("M_quiz.txt",rand);				
			
			LoadImage(rand2-1);
			magTile = false;
			magTile2 = true;
		}
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			_checkQ = true;
			stringToEdit="";	
			//stringToEdit = null;
			//rand = UnityEngine.Random.Range(1,140);
			playerOnTheQuestionTile=true;
			//LoadFile("Question.txt",rand);
			//상 중 하 에 따른 읽어오는 파일이 다름]
			/*
			rand = 1;
			LoadFile("Q_H.txt",rand);
			*/
			
			if(highQuestion)
			{
				rand = UnityEngine.Random.Range(1,27);
				LoadFile("Q_H.txt",rand);
			}
			if(midQuestion)
			{
				rand = UnityEngine.Random.Range(1,139);
				LoadFile("Q_M.txt",rand);
			}
			if(lowQuestion)
			{
				rand = UnityEngine.Random.Range(1,86);
				LoadFile("Q_L.txt",rand);				
			}
			
			LoadImage(rand2-1);
		}
	}
	
	void OnGUI(){
		GUI.depth=1;
		GUI.skin = QuestionSkin;
		if(playerOnTheQuestionTile&&questionTile||magTile2)
		{			
			GUI.Box(new Rect(questionBoxPosition.x,questionBoxPosition.y,questionBoxSize.x,questionBoxSize.y),"");
			GUI.Label(new Rect(questionNumberPosition.x,questionNumberPosition.y,questionNumberSize.x,questionNumberSize.y),"No."+qNumber[0]);
			GUI.Label(new Rect(questionNumberPosition.x+300,questionNumberPosition.y,questionNumberSize.x,questionNumberSize.y),"난이도 : "+qLOD[0]);
			GUI.Label(new Rect(questionPosition.x,questionPosition.y,questionSize.x,questionSize.y),questions[0]);	
			GUI.Box(new Rect(explainPictureBackgroundPosition.x,explainPictureBackgroundPosition.y,explainPictureBackgroundSize.x,explainPictureBackgroundSize.y),"");
			GUI.DrawTexture(new Rect(explainPicturePosition.x,explainPicturePosition.y,explainPictureSize.x,explainPictureSize.y),qImage[0]);
			stringToEdit = GUI.TextField(new Rect(answeringPosition.x,answeringPosition.y,answeringSize.x,answeringSize.y), stringToEdit, 25);
			if(GUI.Button(new Rect(confirmButtonPosition.x,confirmButtonPosition.y,confirmButtonSize.x,confirmButtonSize.y),"확인"))
			{
				playerOnTheQuestionTile = false;
				magTile2 = false;
				CheckAnswer(stringToEdit);
			}				
		}	
		if(correct)
		{
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y,imageSize.x,imageSize.y),correctImage);
			if(GUI.Button(new Rect(500,500,100,50),"확인"))
			{
				_checkQ = false;
				correct = false;					
			}
		}
		
		if(wrong)
		{
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y,imageSize.x,imageSize.y),wrongImage);
			if(GUI.Button(new Rect(500,500,100,50),"확인"))
			{
				_checkQ = false;
				wrong = false;
			}
		}
		
	}
	bool CheckUpgrade()
	{
		for(int i=0;i<=countQuestion;i++)//같은 문제면
		{	
			if(haveNum[i]==qNumber[0])
			{
				if(haveUpgrade[i]>=4)
				{
					return false;
				}			
				haveUpgrade[i]++;
				return true;
			}
		}
		//처음 나온문제면
		haveUpgrade[countQuestion]++;//업그레이드 증가 0 ->1
		return false;
	}
	bool CheckAnswer(string aw)
	{
		if(aw==qAnswer[0])		
		{
			correct = true;
			if(!CheckUpgrade()){
				//haveNum[countQuestion]=""+(rand+1);
				haveNum[countQuestion]=qNumber[0];
				haveLOD[countQuestion]=qLOD[0];
				haveQuestion[countQuestion]=questions[0];
				haveImage[countQuestion] = qImage[0];
				countQuestion++;
			}
			if(qLOD[0]=="상")
				MainGUI.addHighScore=true;
			if(qLOD[0]=="중")
				MainGUI.addMidScore=true;
			if(qLOD[0]=="하")
				MainGUI.addLowScore=true;			
			return true;
		}
		else
		{
			wrong = true;
			MainGUI.subScore=true;
			return false;
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
