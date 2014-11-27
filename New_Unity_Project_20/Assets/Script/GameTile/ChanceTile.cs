using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
public class ChanceTile : MonoBehaviour {
	public static int rand;
	public Vector2 imagePos;
	public Vector2 imageSize;
	public Vector2 confirmButtonPos;
	public Vector2 confirmButtonSize;
	public GUISkin S1;
	public Texture2D[] chanceImage = new Texture2D[5];
	public static bool _playerOnATile = false;
	
	//0 - 포인트 추가 1 - 시간 추가 2 - 포인트 감소  3 - 시간 감소
	public static bool[] chanceButtonSwitch = new bool[5];
	bool guiOn = false;
	
	void Awake()
	{
		_playerOnATile = false;
	}
	// Use this for initialization
	void Start () {
		 _playerOnATile = false;
		for(int i=0;i<5;i++)
		{
			chanceButtonSwitch[i] = false;			
		}
		rand = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			guiOn = true;
			ChanceFunction();	
			_playerOnATile = true;
		}
	}
	public static void ChanceFunction()
	{
		rand = UnityEngine.Random.Range(0,3);
		rand++;
		switch(rand)
		{			
			case 1: MainGUI.getPointFromChance = true; break;
			case 2: MainGUI.getTimeFromChance = true; break;
			case 3: MainGUI.losePointFromChance = true; break;
			case 4: MainGUI.loseTimeFromChance = true; break;
		}
	}
	
	void OnGUI()
	{
		GUI.skin = S1;
		if(guiOn)
		{
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y,imageSize.x,imageSize.y),chanceImage[rand-1]);
			if(GUI.Button(new Rect(confirmButtonPos.x,confirmButtonPos.y,confirmButtonSize.x,confirmButtonSize.y),"확인"))
			{
				_playerOnATile = false;
				guiOn = false;
			}
		}
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
	
	void OnMouseOver()
	{	
		renderer.material.color = new Color(2f, 2f, 2f);//버튼위에 마우스가 있을시 밝아짐		 
	}

	void OnMouseExit() {
        renderer.material.color = Color.white;//마우스가 버튼을 벗어났을때 기존의 색 으로 돌아옴
    }
}
