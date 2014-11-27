 using UnityEngine;
using System.Collections;
using System;
public class Question2 : MonoBehaviour {
	
	//private int click = 0;
	public static int click =0;
	public GUISkin s1;
	private float tutorialWidth = 1500.0f;
	private float tutorialHeight = 1000.0f;
	private float tutorialLeft = (Screen.width/2)-250f;
	private float tutorialTop = (Screen.height/2)-350f;
	
	private float textWidth = 720.0f;
	private float textHeight = 140.0f;
	private float textLeft = 50f;
	private float textTop = Screen.height-190f;
	
	private float dialogWidth = 800.0f;
	private float dialogHeight = 220.0f;
	private float dialogLeft = 10f;
	private float dialogTop = Screen.height-330f;
	
	private float personWidth = 150.0f;
	private float personHeight = 300.0f;
	private float personLeft = 820f;
	private float personTop = Screen.height-300f;
	
	private GUIStyle style = new GUIStyle();
	public Texture person;
	public Texture dialog;
	public int countContent;
	public GUIContent[] listDialog = new GUIContent[]{};
	public Texture AA;
	public static bool EOI = false;//end of interface
	public string answer;
	public string  stringToEdit;
	public int index;
	public bool _flag=false;
	public bool _flag1=false;
	public float startTime;
	public float finishTime;
	public DateTime solvetime;
	//public DateTime solveddate;
	void Start(){
		index = UnityEngine.Random.Range(0,2);	
		PlayerPrefs.SetInt("QNumber",index+1);
		startTime = Time.realtimeSinceStartup;	
		
		//print (solvetime);
	}
	
	void Update(){
		
		
		
	}
	
	
	
	
	void OnGUI(){
		GUI.skin=s1;
		
		Rect tutorialPosition = new Rect(tutorialLeft, tutorialTop, tutorialWidth, tutorialHeight);
		Rect dialogPosition = new Rect(dialogLeft,dialogTop,dialogWidth,dialogHeight);
		
		Rect AAA = new Rect(130,80,50,50);
		GUI.Box(AAA,AA,GUIStyle.none);
		stringToEdit = GUI.TextArea(new Rect((Screen.width/2f),(Screen.height/1.5f),100,30), stringToEdit, 10);	
		GUI.Box( tutorialPosition, listDialog[index].image, GUIStyle.none);
		
		
		
		if(_flag==true)
		{
			if(GUI.Button(new Rect((Screen.width/4),(Screen.height/2f), 500, 100), "Wrong."))
			{
				
				
				PlayerPrefs.SetInt("CountQuestion",PlayerPrefs.GetInt("CountQuestion")+1);
				_flag=false;
				int a=0;
						a = UnityEngine.Random.Range(0,2);
					if(a==0)
						Application.LoadLevel ("Question2");
					
					else
						Application.LoadLevel("Question");
			}
		}	
		
		if(_flag1==true){
			if(GUI.Button(new Rect((Screen.width/4),(Screen.height/2f), 500, 100), "Correct!"))
			{
				finishTime=Time.realtimeSinceStartup-startTime;
			/*	PlayerPrefs.SetFloat("UseTime5",PlayerPrefs.GetInt("UseTime4"));
				PlayerPrefs.SetFloat("UseTime4",PlayerPrefs.GetInt("UseTime3"));
				PlayerPrefs.SetFloat("UseTime3",PlayerPrefs.GetInt("UseTime2"));
				PlayerPrefs.SetFloat("UseTime2",PlayerPrefs.GetInt("UseTime1"));*/
				PlayerPrefs.SetFloat("UseTime1",finishTime);
				print ("UseTime:"+PlayerPrefs.GetFloat("UseTime1"));
			//	solveddate=DateTime.Today;
			//	print (DateTime.Today);
				solvetime=DateTime.Now;
				
				PlayerPrefs.SetInt("Year",solvetime.Year);
				PlayerPrefs.SetInt("Month",solvetime.Month);
				PlayerPrefs.SetInt("Day",solvetime.Day);
				PlayerPrefs.SetInt("Hour",solvetime.Hour);
				PlayerPrefs.SetInt("Minute",solvetime.Minute);
				PlayerPrefs.SetInt("Second",solvetime.Second);
				
				PlayerPrefs.SetInt("CorrectAnswer",PlayerPrefs.GetInt("CorrectAnswer")+1);
				PlayerPrefs.SetInt("Result", 3);
				Application.LoadLevel("result");
			}
		}
		
			
		
			
			//GUILayout.Button("A Button with fixed width", GUILayout.Width(300));
			if (GUI.Button(new Rect((Screen.width/2),(Screen.height/1.3f), 200, 40), "Click")&&_flag==false)
			{
				answer=stringToEdit;
				
				if(answer==listDialog[index].text)
				{
					_flag1=true;
					/*if(GUI.Button(new Rect((Screen.width/5),(Screen.height/2f), 500, 100), "Correct!"))
					{
					PlayerPrefs.SetInt("Result", 3);
					Application.LoadLevel("result");
					}*/
				}
				else
				{
				//	if(GUI.Button(new Rect((Screen.width/5),(Screen.height/2f), 500, 100), "Wrong!"))
					{
						_flag=true;
					}
				}
			}
					
	}
}
