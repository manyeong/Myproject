using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
public class SingledoulbeTile : MonoBehaviour {
	public GUISkin S1;
	
	public Vector4 background;
	public Vector4 fifty;
	public Vector4 onehundred;
	public Vector4 onehundredfifty;
	public Vector4 gobutton;
	public Vector4 _single;
	public Vector4 _double;
	bool _sing = false;
	bool _dou = false;
	bool playerOnATile = false;
	bool playerOnAtile2 = false;
	string point;
	string singordou;
	bool _start = false;
	
	int playerSelect = 0;
	int computerSelect = 0;
	int playercomplex = 0;
	
	bool _success = false;
	bool _fail = false;
	
	string result;
	
	public Vector4 confirmButton;
	
	int count = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(_start)
		{
			computerSelect = UnityEngine.Random.Range(1,2);
			if(computerSelect == playerSelect)
			{
				_success = true;
				result = "성공";
				MainGUI.addHighScore = true;
			}
			else
			{
				result = "실패";
				_fail = true;
			}
			_start = false;
		}
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			playerOnATile = true;	
			playerOnAtile2 = true;
		}
	}
	void OnGUI(){
		GUI.skin =S1;	
		
		
		if(playerOnATile)
		{
			GUI.Box(new Rect(background.x,background.y,background.z,background.w),result+""+point+"\n"+singordou);
			if(playerOnAtile2)
			{
				if(GUI.Button(new Rect(fifty.x,fifty.y,fifty.z,fifty.w),"50"))
				{
					playercomplex = 1;
					point = "\n배팅 점수 : 50";
				}
				if(GUI.Button(new Rect(onehundred.x,onehundred.y,onehundred.z,onehundred.w),"100"))
				{
					playercomplex = 2;
					point = "\n배팅 점수 : 100";
				}
				if(GUI.Button(new Rect(onehundredfifty.x,onehundredfifty.y,onehundredfifty.z,onehundredfifty.w),"150"))
				{
					playercomplex = 3;
					point = "\n배팅 점수 : 150";
				}
				if(GUI.Button(new Rect(gobutton.x,gobutton.y,gobutton.z,gobutton.w),"GO!"))
				{
					if(playercomplex>0&&_sing||playercomplex>0&&_dou)
					{
						_start =true;
						playerOnAtile2 = false;
						point = null;
						singordou =null;
					}
					
				}
				if(GUI.Button(new Rect(_single.x,_single.y,_single.z,_single.w),"홀"))
				{
					_sing = true;
					_dou = false;
					singordou = "홀 을 선택 하셨습니다.";
					playerSelect = 1;
				}
				if(GUI.Button(new Rect(_double.x,_double.y,_double.z,_double.w),"짝"))
				{
					_sing = false;
					_dou = true;
					singordou = "짝 을 선택 하셨습니다.";
					playerSelect = 2;
				}
			}
		}
		
		
		if(_success||_fail)
		{
			if(GUI.Button(new Rect(confirmButton.x,confirmButton.y,confirmButton.z,confirmButton.w),"확 인"))
			{
				playerOnATile = false;
				_success = false;
				_fail = false;
				playercomplex = 0;
				_sing = false;
				_dou = false;
				singordou = null;
				playerSelect = 0;
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
