using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
public class MazeTile : MonoBehaviour {
	public Vector2 mazeGUIPos;
	public Vector2 mazeGUISize;
	
	public Texture2D mazeImage;
	public Vector2 mazeImagePos;
	public Vector2 mazeImageSize;
	
	private bool GUIMazeTile;
	
	public GUISkin S1;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			GUIMazeTile = false;
			AppDemo._offRollDice = false;
		}
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			AppDemo._offRollDice = true;
			MainGUI.onAMaze=true;
			GUIMazeTile = true;
		}
	}
	void OnGUI(){
		GUI.skin =S1;
		if(GUIMazeTile)
		{	
			GUI.Box(new Rect(mazeGUIPos.x,mazeGUIPos.y,mazeGUISize.x,mazeGUISize.y),"\n\n\n당신은 미궁에 빠져 해맸습니다.\n 1회 방문 : 60초 감소 \n 2회 방문 : 90초 감소 \n 3회 방문 : 120초 감소");	
			GUI.DrawTexture(new Rect(mazeImagePos.x,mazeImagePos.y,mazeImageSize.x,mazeImageSize.y),mazeImage);
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
