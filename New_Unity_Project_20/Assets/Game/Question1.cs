 using UnityEngine;
using System.Collections;
using System;
public class Question1 : MonoBehaviour {
	
	//private int click = 0;
	
	public static int click =0;
	public GUISkin s1;
	private float tutorialWidth = 1500.0f;
	private float tutorialHeight = 1300.0f;
	private float tutorialLeft = (Screen.width/2)-250f;
	private float tutorialTop = (Screen.height/2)-300f;
	
	private float textWidth = 720.0f;
	private float textHeight = 140.0f;
	private float textLeft = 50f;
	private float textTop = Screen.height-190f;
	
	private float dialogWidth = 1800.0f;
	//private float dialogHeight = 220.0f;
	private float dialogHeight = 500.0f;
	private float dialogLeft = 10f;
	private float dialogTop = Screen.height-230f;
	
	private float personWidth = 150.0f;
	private float personHeight = 300.0f;
	private float personLeft = 820f;
	private float personTop = Screen.height-300f;
	
	private GUIStyle style = new GUIStyle();
	public Texture person;
	public Texture dialog;
	public int countContent;
	public GUIContent[] listDialog = new GUIContent[]{};
	public Texture[] example = new Texture[] {};
	public static bool EOI = false;//end of interface
	public int[] answer = new int[]{};
	public string  stringToEdit;
	public int index;
	public int playerschoice=10;
	public bool _flag=false;
	public Texture AA;
	public float startTime;
	public float finishTime;
	public DateTime solvetime;
	void Start()
	{
		
		index = UnityEngine.Random.Range(1,4);
		PlayerPrefs.SetInt("QNumber1",index+3);
		startTime = Time.realtimeSinceStartup;	
	}
	
	void Update()
	{
		if(playerschoice==answer[index])
		{
			print ("Get Score");	
			
		}
		
		else
		{
			print ("No Score");			
			
		}
		
	}
		

	
	
	
	void OnGUI(){
			Rect AAA = new Rect(130,80,50,50);
			GUI.Box(AAA,AA,GUIStyle.none);
		
		
			GUI.skin=s1;
			Rect tutorialPosition = new Rect(tutorialLeft, tutorialTop, tutorialWidth, tutorialHeight);
			
			GUI.Box( tutorialPosition, listDialog[index].image, GUIStyle.none);
			
			if ((GUI.Button(new Rect(150,(Screen.height/1.3f), 150, 50), example[(index*5)-4],GUIStyle.none))&&_flag==false)
			{
				playerschoice=1;
				_flag=true;
			}
		
		
			if (GUI.Button(new Rect(300,(Screen.height/1.3f), 150, 50), example[index*5-3],GUIStyle.none)&&_flag==false)
			{
				playerschoice=2;
				_flag=true;
			}
            
		
			if (GUI.Button(new Rect(450,(Screen.height/1.3f), 150, 50), example[index*5-2],GUIStyle.none)&&_flag==false)
          	{
				playerschoice=3;
				_flag=true;
			}
			if (GUI.Button(new Rect(600,(Screen.height/1.3f), 150, 50), example[index*5-1],GUIStyle.none)&&_flag==false)
           	{
				playerschoice=4;
				_flag=true;
			}
			if (GUI.Button(new Rect(750,(Screen.height/1.3f), 150, 50), example[index*5],GUIStyle.none)&&_flag==false)
           	{
				playerschoice=5;
				_flag=true;
			}
		
		
		
			if(_flag==true&&answer[index]==playerschoice){
				if(GUI.Button(new Rect((Screen.width/5),(Screen.height/2f), 500, 100), "Correct!"))
				{
					Application.LoadLevel ("qs2");			
				} 
			}
			
			else if(_flag==true&&answer[index]!=playerschoice){
				if(GUI.Button(new Rect((Screen.width/5),(Screen.height/2f), 500, 100), "Wrong!"))
				{
					PlayerPrefs.SetInt("CountQuestion",PlayerPrefs.GetInt("CountQuestion")+1);
					//Application.LoadLevel ("result");
					//Application.LoadLevel ("Question");
					_flag=false;				
				
						Application.LoadLevel ("qs2");
					
				} 
			}
		
		
	}
	
	
}

