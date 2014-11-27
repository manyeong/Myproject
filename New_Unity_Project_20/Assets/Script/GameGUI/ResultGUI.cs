using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.IO;  
public class ResultGUI : MonoBehaviour {
	public Vector2 restartButtonPos;
	public Vector2 restartButtonSize;
	public GUISkin S1;
	public Texture2D restart;
	
	public Vector2 playerScorePos;
	public Vector2 playerScoreSize;
	
	public Vector2 playerRankingPos;
	public Vector2 playerRankingSize;
	
	string[] playerRanking = new string[5];
	int[] playerRankingScore = new int[6];
	
	// Use this for initialization
	void Start () {				
		LoadFile("RankFile.txt");
		playerRankingScore[5] = MainGUI.pScore;
		for(int i=0;i<5;i++)
		{
			playerRankingScore[i] = System.Convert.ToInt32(playerRanking[i]);
		}
		Array.Sort(playerRankingScore);
		Array.Reverse(playerRankingScore);
		
		for(int i=0;i<6;i++)
		{
			print("P:["+i+"]:"+playerRankingScore[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.skin = S1;
		GUI.Label(new Rect(playerScorePos.x,playerScorePos.y,playerScoreSize.x,playerScoreSize.y),""+MainGUI.pScore);
		GUI.Label(new Rect(playerRankingPos.x,playerRankingPos.y-30,playerRankingSize.x,playerRankingSize.y),"1. "+playerRankingScore[0]+"\n2. "+playerRankingScore[1]+"\n3. "+playerRankingScore[2]+"\n4. "+playerRankingScore[3]+"\n5. "+playerRankingScore[4]+"\n");
		if(GUI.Button(new Rect(restartButtonPos.x,restartButtonPos.y,restartButtonSize.x,restartButtonSize.y),""))
		{		
			WriteTextFile(playerRankingScore);
			Application.LoadLevel("mainmap");
		}
		
	}
	
	
	private void WriteTextFile(int[] txt)
	{	
	
		for(int i=0;i<5;i++)
		{
			playerRanking[i] = txt[i].ToString();
		}
		
		System.IO.File.WriteAllLines("Assets/TxtFile/RankFile.txt", playerRanking);
	}
	
	private void WriteTextFile(string[] txt)
	{			
		System.IO.File.WriteAllLines("Assets/TxtFile/RankFile.txt", txt);
	}
	private bool LoadFile(string fileName)
	{
		try
		{
			string line = System.IO.File.ReadAllText("Assets/TxtFile/"+fileName);
			if(line==null)
			{
				print ("null");
				return false;
			}
			
			string[] question = line.Split('\n');
			playerRanking = question;
					
			return true;
		}
		catch (Exception e)
        {			
            print ("fail");
            return false;
        }
	}
	
}
