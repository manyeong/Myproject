using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;  
public class CardfeeTile : MonoBehaviour {

	public Vector2 cardFeePos;
	public Vector2 cardFeeSize;
	
	public Vector2 cardFeeConfirmPos;
	public Vector2 cardFeeConfirnSize;
	public GUISkin S1;
	bool playerOnACardFeeTile = false;
	public Texture2D image;
	public Vector2 imagePos;
	public Vector2 imageSize;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.skin = S1;
		if(playerOnACardFeeTile)
		{
			print (SolveTile.countSolve);
			GUI.Box(new Rect(cardFeePos.x,cardFeePos.y,cardFeeSize.x,cardFeeSize.y),"고객님의 카드 사용 수수료가 청부 되었습니다.\n\n\n" +(QuestionTile.countQuestion)*10+"점 차감!");
			GUI.DrawTexture(new Rect(imagePos.x,imagePos.y,imageSize.x,imageSize.y),image);
			if(GUI.Button(new Rect(cardFeeConfirmPos.x,cardFeeConfirmPos.y,cardFeeConfirnSize.x,cardFeeConfirnSize.y),"확 인"))
			{
				playerOnACardFeeTile = false;
			}
		}
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			playerOnACardFeeTile = true;
			MainGUI.cardFeeTile = true;
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
