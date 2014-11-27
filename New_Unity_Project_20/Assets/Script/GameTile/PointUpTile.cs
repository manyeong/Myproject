using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
public class PointUpTile : MonoBehaviour {
	public Vector2 imagePos;
	public Vector2 imageSize;
	
	public Vector2 confirmPos;
	public Vector2 confirmSize;
	public Texture2D image;
	public GUISkin S1;
	bool onATile;
	public static bool pointUpTile = false;
	// Use this for initialization
	void Start () {
		 pointUpTile = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			MainGUI.pointUpTile=true;
			onATile = true;
			pointUpTile = true;
		}
	}
	
	void OnGUI()
	{
		if(onATile)
		{
			GUI.skin = S1;
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y,imageSize.x,imageSize.y),	image);
			
			if(GUI.Button(new Rect(confirmPos.x,confirmPos.y,confirmSize.x,confirmSize.y),"확인"))
			{
				onATile = false;	
				pointUpTile =false;
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
