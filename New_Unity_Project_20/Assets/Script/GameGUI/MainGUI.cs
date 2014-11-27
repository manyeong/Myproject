using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	public GameObject Kill_all;
	public Vector2 qCardBackgroundPos;
	public Vector2 qCardBackgroundSize;
	
	public Vector2 qCardInfoGroundPos;
	public Vector2 qCardInfoGroundSize;
	
	public Vector2 numGroundPos;
	public Vector2 numGroundSize;
	
	public Vector2 scoreGroundPos;
	public Vector2 scoreGroundSize;
	
	public Vector2 lODPos;//Level of Difficulty
	public Vector2 lODSize;
	bool checkQCard;
	
	public Vector2 scrollPosition = Vector2.zero;
	
	public Vector2 ScrollViewPos1;
	public Vector2 ScrollViewSize1;
	
	public Vector2 ScrollViewPos2;
	public Vector2 ScrollViewSize2;
	
	bool checkSCard;
	public Vector2 checkSCardPos;
	public Vector2 checkSCardSize;
	public Vector2 scrollViewPos3;
	public Vector2 scrollViewSize3;
	public Vector2 scrollViewPos4;
	public Vector2 scrollViewSize4;
	
	public Vector2 sCardPos;
	public Vector2 sCardSize;
	public Vector2 sCardNumPos;
	public Vector2 sCardNumSize;
	public Vector2 sCardLodPos;
	public Vector2 sCardLodSize;
	
	public Vector2 qExplainPos;
	public Vector2 qExplainSize;
	public bool clickQ;
	
	public Vector2 sExplainPos;
	public Vector2 sExplainSize;
	
	
	public Texture2D qStarBackground;
	public Texture2D qStar;
	
	public Vector2 qStarBack;
	public Vector2 qUpgradeStar1;
	public Vector2 qUpgradeStar2;
	public Vector2 qUpgradeStar3;
	public bool clickS;
	int _select;
	
	
	int solveTileHEIndex=0;
	int qTileHQIndex=0;
	
	public static int pScore;
	
	
	

	public class PlayersInfo
	{
		int score;
		public int Score{
			get;
			set;
		}
		
	}
	
	public class PlayersQuestionCard
	{
			string number;
			string lOD;//난이도level of difficulty
			int qScore;
			public string Number{
				get;
				set;
			}
			public string LOD{
				get;
				set;
			}
			public int QScore{
				get;
				set;
			}
	}
	public GUISkin GUISTYLE;
	public GUISkin ButtonStyle;
	public Vector2 ScoreBoxPOS;
	public Vector2 ScoreBoxSIZE;
	
	public Vector2 TimerPOS;
	public Vector2 TimerSIZE;
	
	public Vector2 QuestionCardPOS;
	public Vector2 QuestionCardSIZE;
	
	public Vector2 SolutionCardPOS;
	public Vector2 SolutionCardSIZE;
	
	public float timeLeft;
	float Minutes;
	float Seconds;
	//주사위 더블
	public static bool _diceDouble = false;
	//찬스카드 포인트획득,포인트 감소,시간 추가,시간 감소,점수더블
	public static bool getPointFromChance = false;
	public static bool losePointFromChance = false;
	public static bool getTimeFromChance = false;
	public static bool loseTimeFromChance = false;
	public static bool getDoubleScore = false;
	
	//업그레이드
	public static bool HUp;
	public static bool MUp;
	public static bool LUp;
	
	//카드 수수료 타일
	public static bool cardFeeTile = false;
	//시작 타일을 지났을 시
	public static bool passStartPoint = false;
	//포인트 업 타일
	public static bool pointUpTile = false;
	
	//문제를 맞췄을 시(점수 증가)
	public static bool addLowScore = false;
	public static bool addMidScore = false;
	public static bool addHighScore = false;
	
	//문제를 틀렸을 시(점수 감소)
	public static bool subScore = false;
	//미궁(걸리면 플레이어 시간이 줄어들음)
	public static bool onAMaze = false;
	public int subTimeOnAMaze = 60;
	
	//모래시계(걸리면 플레이어 시간이 늘어남)
	public static bool onASandglass = false;
	public int addTimeOnASandglass = 60;
	
	
	PlayersInfo Player = new PlayersInfo();
	PlayersQuestionCard[] PlayersQCard = new PlayersQuestionCard[100]; 
	
	void Awake(){
		Player.Score = 500;
		pScore = Player.Score;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pScore = Player.Score;
		Timer();		
		CheckGameOver();
		
		
		if(_diceDouble)
		{
			DoubleValues();
			_diceDouble = false;
		}
		if(cardFeeTile)
		{
			SubScore((SolveTile.countSolve)*10);//카드갯수 * 10			
			cardFeeTile = false;
		}
		
		if(getPointFromChance)
		{
			AddScore(200);
			getPointFromChance = false;
		}
		if(losePointFromChance)
		{
			SubScore(200);
			losePointFromChance = false;
		}
		if(getTimeFromChance)
		{
			AddTime(30.0f);
			getTimeFromChance = false;			
		}
		if(loseTimeFromChance)
		{
			SubTime(30.0f);
			loseTimeFromChance= false;
		}
		if(getDoubleScore)
		{
			DoubleScore();
			getDoubleScore = false;			
		}
		
		if(HUp)
		{
			AddScore (100);
			HUp = false;
		}
		if(MUp)
		{
			AddScore (60);
			MUp = false;
		}
		if(LUp)
		{
			AddScore (20);
			LUp = false;
		}
		
		if(pointUpTile)
		{
			AddScore (100);
			pointUpTile=false;
		}
		if(subScore)
		{
			SubScore(100);
			subScore=false;
		}
		if(addHighScore)
		{
			AddScore(100);
			addHighScore=false;
		}
		if(addMidScore)
		{
			AddScore(70);
			addMidScore=false;
		}
		if(addLowScore)
		{
			AddScore(50);
			addLowScore=false;
		}
		if(passStartPoint)
		{
			PassStartpoint();
			passStartPoint=false;
		}
		if(onAMaze)
		{
			OnAMaze();
			onAMaze=false;
		}
		if(onASandglass)
		{
			OnASandglass();
			onASandglass=false;
		}
		
	}
	public GUISkin solveExGUI;
	public Vector2 solveBackgroundPos;
	public Vector2 solveBackgroundSize;
	public Vector2 solveNumberPos;
	public Vector2 solveNumberSize;
	public Vector2 solveLODPos;
	public Vector2 solveLODSize;
	public Vector2 solveExPos;
	public Vector2 solveExSize;
	//문제 GUISKIN
	public GUISkin qSkin;
	//문제 바탕
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
	void OnGUI(){
		GUI.skin = GUISTYLE;
		
		
		//GUI 문제카드 창
		if(clickQ)
		{	
			checkQCard = false;
			AppDemo._offRollDice = true;
			GUI.skin = qSkin;					
			GUI.Box(new Rect(questionBoxPosition.x,questionBoxPosition.y,questionBoxSize.x,questionBoxSize.y),"");
			GUI.Label(new Rect(questionNumberPosition.x,questionNumberPosition.y,questionNumberSize.x,questionNumberSize.y),"No."+QuestionTile.haveNum[qTileHQIndex]);
			GUI.Label(new Rect(questionNumberPosition.x+300,questionNumberPosition.y,questionNumberSize.x,questionNumberSize.y),"난이도 : "+QuestionTile.haveLOD[qTileHQIndex]);
			GUI.Label(new Rect(questionPosition.x,questionPosition.y,questionSize.x,questionSize.y),QuestionTile.haveQuestion[qTileHQIndex]);	
			GUI.Box(new Rect(explainPictureBackgroundPosition.x,explainPictureBackgroundPosition.y,explainPictureBackgroundSize.x,explainPictureBackgroundSize.y),"");
			GUI.DrawTexture(new Rect(explainPicturePosition.x,explainPicturePosition.y,explainPictureSize.x,explainPictureSize.y),QuestionTile.haveImage[qTileHQIndex]);
			GUI.skin = solveExGUI;
			if (GUI.Button(new Rect(630,500,200,50), "확인"))
			{
				AppDemo._offRollDice = false;
				clickQ = false;
				checkQCard = false;	
				checkSCard = false;
				//checkQCard = t!checkQCard;
			}
			/*
			GUI.skin=null;
			GUI.Box(new Rect(200,100,600,500),"");
			GUI.Label(new Rect(250,230,500,100),QuestionTile.haveQuestion[qTileHQIndex]);
			GUI.skin = GUISTYLE;
			*/
		}
		//GUI 해결카드 창
		if(clickS)
		{
			AppDemo._offRollDice = true;
			checkSCard = false;
			GUI.skin = solveExGUI;
			GUI.Box(new Rect(solveBackgroundPos.x,solveBackgroundPos.y,solveBackgroundSize.x,solveBackgroundSize.y),"");
			GUI.Box(new Rect(solveNumberPos.x,solveNumberPos.y,solveNumberSize.x,solveNumberSize.y),SolveTile.haveNum[solveTileHEIndex]);
			GUI.Box(new Rect(solveLODPos.x,solveLODPos.y,solveLODSize.x,solveLODSize.y),SolveTile.haveLOD[solveTileHEIndex]);
			GUI.Box (new Rect(solveExPos.x,solveExPos.y,solveExSize.x,solveExSize.y),SolveTile.haveExplain[solveTileHEIndex]);
			if (GUI.Button(new Rect(400,500,200,50), "확인"))
			{
				clickS = false;
				checkSCard = false;
				checkQCard = false;
				AppDemo._offRollDice = false;
			}
			/*
			GUI.skin=null;
			GUI.Box(new Rect(260,200,500,300),"");
			GUI.Label(new Rect(260,200,500,300),SolveTile.haveExplain[solveTileHEIndex]);
			GUI.skin = GUISTYLE;
			*/
		}
		
		GUI.Box(new Rect(ScoreBoxPOS.x, ScoreBoxPOS.y, ScoreBoxSIZE.x, ScoreBoxSIZE.y),""+Player.Score);
		
		if(timeLeft>0){
			
			//guiText.text = String.Format ("{0:00}:{1:00}", (int)Minutes, (int)Seconds); 
			GUI.Box(new Rect(TimerPOS.x, TimerPOS.y, TimerSIZE.x, TimerSIZE.y), "남은 시간\n" + string.Format("{0:00}:{1:00}",(int)Minutes,(int)Seconds));
			//GUI.Box(new Rect(TimerPOS.x, TimerPOS.y, TimerSIZE.x, TimerSIZE.y), "남은 시간\n" + (int)Minutes+":"+(int)Seconds);
		}
		
		if (GUI.Button(new Rect(SolutionCardPOS.x, SolutionCardPOS.y, SolutionCardSIZE.x, SolutionCardSIZE.y), "해결카드"))
		{
			clickS=false;
			checkSCard=!checkSCard;
			if(checkSCard||checkQCard)
			{
				AppDemo._offRollDice = true;	
			}
			else
			{
				AppDemo._offRollDice = false;	
			}
			
		}
		if (GUI.Button(new Rect(QuestionCardPOS.x, QuestionCardPOS.y, QuestionCardSIZE.x, QuestionCardSIZE.y), "문제카드"))
		{
			clickQ=false;
			checkQCard=!checkQCard;
			if(checkQCard||checkSCard)
			{
				AppDemo._offRollDice = true;	
			}
			else
			{
				AppDemo._offRollDice = false;	
			}			
		}
		if(checkQCard)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(ScrollViewPos1.x, ScrollViewPos1.y, ScrollViewSize1.x, ScrollViewSize1.y), scrollPosition, new Rect(ScrollViewPos2.x, ScrollViewPos2.y, ScrollViewSize2.x, ScrollViewSize2.y));
			GUI.skin = ButtonStyle;
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=0;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+100,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=1;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+200,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=2;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+300,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=3;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+400,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=4;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+500,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=5;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+600,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=6;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+700,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=7;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+800,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=8;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+900,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=9;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1000,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=10;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1100,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=11;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1200,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=12;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1300,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=13;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1400,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=14;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1500,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=15;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1600,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=16;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1700,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=17;
				clickQ=!clickQ;
			}
			if(GUI.Button (new Rect(qCardInfoGroundPos.x,qCardInfoGroundPos.y+1800,qCardInfoGroundSize.x,qCardInfoGroundSize.y),""))
			{
				qTileHQIndex=18;
				clickQ=!clickQ;
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[0]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[0]);
			GUI.Label (new Rect(lODPos.x,lODPos.y,lODSize.x,lODSize.y),QuestionTile.haveLOD[0]);
			
			GUI.Label (new Rect(qStarBack.x,qStarBack.y,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[0]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[0]>=2)
					GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[0]>=3)
					GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y,qStar.height,qStar.width),qStar);
			}
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+100,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[1]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+100,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[1]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+100,lODSize.x,lODSize.y),QuestionTile.haveLOD[1]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+100,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+100,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+100,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[1]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+100,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[1]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+100,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[1]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+100,qStar.height,qStar.width),qStar);
			}
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+200,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[2]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+200,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[2]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+200,lODSize.x,lODSize.y),QuestionTile.haveLOD[2]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+200,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+200,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+200,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[2]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+200,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[2]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+200,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[2]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+200,qStar.height,qStar.width),qStar);
			}
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+300,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[3]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+300,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[3]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+300,lODSize.x,lODSize.y),QuestionTile.haveLOD[3]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+300,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+300,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+300,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[3]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+300,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[3]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+300,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[3]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+300,qStar.height,qStar.width),qStar);
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+400,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[4]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+400,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[4]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+400,lODSize.x,lODSize.y),QuestionTile.haveLOD[4]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+400,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+400,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+400,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[4]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+400,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[4]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+400,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[4]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+400,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+500,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[5]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+500,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[5]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+500,lODSize.x,lODSize.y),QuestionTile.haveLOD[5]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+500,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+500,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+500,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[5]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+500,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[5]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+500,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[5]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+500,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+600,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[6]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+600,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[6]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+600,lODSize.x,lODSize.y),QuestionTile.haveLOD[6]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+600,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+600,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+600,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[6]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+600,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[6]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+600,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[6]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+600,qStar.height,qStar.width),qStar);
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+700,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[7]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+700,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[7]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+700,lODSize.x,lODSize.y),QuestionTile.haveLOD[7]);
			
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+700,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+700,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+700,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[7]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+200,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[7]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+200,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[7]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+200,qStar.height,qStar.width),qStar);
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+800,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[8]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+800,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[8]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+800,lODSize.x,lODSize.y),QuestionTile.haveLOD[8]);
			
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+800,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+800,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+800,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[8]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+800,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[8]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+800,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[8]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+800,qStar.height,qStar.width),qStar);
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+900,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[9]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+900,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[9]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+900,lODSize.x,lODSize.y),QuestionTile.haveLOD[9]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+900,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+900,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+900,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[9]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+900,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[9]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+900,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[9]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+900,qStar.height,qStar.width),qStar);
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1000,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[10]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1000,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[10]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1000,lODSize.x,lODSize.y),QuestionTile.haveLOD[10]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1000,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1000,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1000,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[10]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1000,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[10]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1000,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[10]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1000,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1100,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[11]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1100,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[11]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1100,lODSize.x,lODSize.y),QuestionTile.haveLOD[11]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1100,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1100,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1100,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[11]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1100,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[11]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1100,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[11]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1100,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1200,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[12]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1200,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[12]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1200,lODSize.x,lODSize.y),QuestionTile.haveLOD[12]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1200,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1200,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1200,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[12]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1200,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[12]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1200,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[12]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1200,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1300,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[13]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1300,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[13]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1300,lODSize.x,lODSize.y),QuestionTile.haveLOD[13]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1300,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1300,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1300,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[13]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1300,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[13]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1300,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[13]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1300,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1400,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[14]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1400,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[14]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1400,lODSize.x,lODSize.y),QuestionTile.haveLOD[14]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1400,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1400,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1400,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[14]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1400,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[14]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1400,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[14]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1400,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1500,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[15]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1500,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[15]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1500,lODSize.x,lODSize.y),QuestionTile.haveLOD[15]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1500,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1500,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1500,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[15]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1500,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[15]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1500,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[15]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1500,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1600,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[15]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1600,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[15]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1600,lODSize.x,lODSize.y),QuestionTile.haveLOD[15]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1600,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1600,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1600,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[16]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1600,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[16]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1600,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[16]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1600,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1700,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[16]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1700,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[16]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1700,lODSize.x,lODSize.y),QuestionTile.haveLOD[16]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1700,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1700,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1700,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[17]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1700,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[17]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1700,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[17]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1700,qStar.height,qStar.width),qStar);
			}
			
			
			GUI.Label (new Rect(numGroundPos.x,numGroundPos.y+1800,numGroundSize.x,numGroundSize.y),QuestionTile.haveNum[17]);
			GUI.Label (new Rect(scoreGroundPos.x,scoreGroundPos.y+1800,scoreGroundSize.x,scoreGroundSize.y),QuestionTile.haveScore[17]);
			GUI.Label (new Rect(lODPos.x,lODPos.y+1800,lODSize.x,lODSize.y),QuestionTile.haveLOD[17]);
			GUI.Label (new Rect(qStarBack.x,qStarBack.y+1800,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+30,qStarBack.y+1800,qStarBackground.height,qStarBackground.width),qStarBackground);
			GUI.Label (new Rect(qStarBack.x+60,qStarBack.y+1800,qStarBackground.height,qStarBackground.width),qStarBackground);
			if(QuestionTile.haveUpgrade[18]>=1)
			{
				GUI.Label (new Rect(qUpgradeStar1.x,qUpgradeStar1.y+1800,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[18]>=2)
				GUI.Label (new Rect(qUpgradeStar2.x,qUpgradeStar2.y+1800,qStar.height,qStar.width),qStar);
				if(QuestionTile.haveUpgrade[18]>=3)
				GUI.Label (new Rect(qUpgradeStar3.x,qUpgradeStar3.y+1800,qStar.height,qStar.width),qStar);
			}
			
			
			
			GUI.EndScrollView();
		}
		
		if(checkSCard)
		{	
			scrollPosition = GUI.BeginScrollView(new Rect(scrollViewPos3.x, scrollViewPos3.y, scrollViewSize3.x, scrollViewSize3.y), scrollPosition, new Rect(scrollViewPos4.x, scrollViewPos4.y, scrollViewSize4.x, scrollViewSize4.y));
			
			GUI.skin = ButtonStyle;
			//GUI.Box(new Rect(checkSCardPos.x,checkSCardPos.y,checkSCardSize.x,checkSCardSize.y),"");
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=0;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+100,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=1;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+200,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=2;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+300,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=3;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+400,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=4;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+500,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=5;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+600,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=6;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+700,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=7;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+800,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=8;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+900,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=9;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1000,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=10;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1100,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=11;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1200,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=12;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1300,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=13;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1400,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=14;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1500,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=15;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1600,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=16;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1700,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=17;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1800,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=18;
			}
			if(GUI.Button(new Rect(sCardPos.x,sCardPos.y+1900,sCardSize.x,sCardSize.y),""))
			{
				clickS=!clickS;
				solveTileHEIndex=19;
			}
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[0]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[0]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+100,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[1]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+100,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[1]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+200,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[2]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+200,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[2]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+300,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[3]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+300,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[3]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+400,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[4]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+400,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[4]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+500,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[5]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+500,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[5]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+600,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[6]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+600,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[6]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+700,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[7]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+700,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[7]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+800,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[8]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+800,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[8]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+900,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[9]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+900,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[9]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1000,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[10]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1000,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[10]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1100,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[11]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1100,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[11]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1200,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[12]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1200,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[12]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1300,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[13]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1300,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[13]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1400,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[14]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1400,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[14]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1500,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[15]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1500,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[15]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1600,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[16]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1600,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[16]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1700,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[17]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1700,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[17]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1800,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[18]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1800,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[18]);
			
			GUI.Label(new Rect(sCardNumPos.x,sCardNumPos.y+1900,sCardNumSize.x,sCardNumSize.y),SolveTile.haveNum[19]);
			GUI.Label (new Rect(sCardLodPos.x,sCardLodPos.y+1900,sCardLodSize.x,sCardLodSize.y),SolveTile.haveLOD[19]);
			
			
			GUI.EndScrollView();
		}
		
		
		
		
		
	}
	
    void QuestionCardInventory()
	{
		
	}
	
	void DoubleValues()
	{
		AddTime(10);
	}
	
	void Timer()
	{
		if(timeLeft>0){
		  timeLeft -= Time.deltaTime; 
		}
		Minutes = timeLeft/60;
		Seconds = timeLeft%60;		
	}
	
	void CheckGameOver(){
		
		if(Player.Score<=0 || timeLeft<=0)
		{			
			AppDemo._start = true;
			Application.LoadLevel("GameOver");
		}
	}
	
	void OnAMaze()
	{
		SubTime(subTimeOnAMaze);
		subTimeOnAMaze = subTimeOnAMaze + 30;
	}
	
	void OnASandglass()
	{
		AddTime(addTimeOnASandglass);
		addTimeOnASandglass = addTimeOnASandglass + 30;
	}
	
	void PassStartpoint()
	{
		AddScore(50);	
		timeLeft = timeLeft + 30;	
	}
	void DoubleScore()
	{
		Player.Score = Player.Score * 2;
	}
	void AddScore(int _add)
	{
		Player.Score = Player.Score + _add;
	}
	void SubScore(int _sub)
	{
		Player.Score = Player.Score - _sub;
	}
	void AddTime(float _add)
	{
		timeLeft = timeLeft + _add;
	}
	void SubTime(float _sub)
	{
		timeLeft = timeLeft - _sub;
	}
}
