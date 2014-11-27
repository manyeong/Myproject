using UnityEngine;
using System.Collections;

public class SandglassTile : MonoBehaviour {
	public Vector2 sanGUIPos;
	public Vector2 sanGUISize;
	
	public Texture2D sanImage;
	public Vector2 sanImagePos;
	public Vector2 sanImageSize;
	public GUISkin S1;
	bool ImageGUI = false;
	// Use this for initialization
	void Start () {
	
	}
	void OnGUI(){
		GUI.skin =S1;
		if(ImageGUI)
		{	
			GUI.Box(new Rect(sanGUIPos.x,sanGUIPos.y,sanGUISize.x,sanGUISize.y),"당신은 시간의 모래를 획득했습니다.\n 1개 획득 : 60초 증가");	
			GUI.DrawTexture(new Rect(sanImagePos.x,sanImagePos.y,sanImageSize.x,sanImageSize.y),sanImage);
		}
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			ImageGUI = false;
			AppDemo._offRollDice = false;
		}
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			AppDemo._offRollDice = true;
			MainGUI.onASandglass=true;
			ImageGUI=true;
		}
	}
	void OnMouseUp()
	{
		if(AppDemo.checkPortal)
		{
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
