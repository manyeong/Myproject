using UnityEngine;
using System.Collections;

public class PortalTile : MonoBehaviour {
	public Vector2 porGUIPos;
	public Vector2 porGUISize;
	
	public Texture2D porImage;
	public Vector2 porImagePos;
	public Vector2 porImageSize;
	
	
	public GUISkin S1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.name=="Player")
		{
			AppDemo.checkPortal = true;
		}
	}
	void OnGUI(){
		GUI.skin =S1;
		if(AppDemo.checkPortal)
		{	
			GUI.Box(new Rect(porGUIPos.x,porGUIPos.y,porGUISize.x,porGUISize.y),"당신은 어디로든 이동이 가능한 이동포탈에 들어왔습니다.\n 이동을 원하는 타일을 선택하세요.");	
			GUI.DrawTexture(new Rect(porImagePos.x,porImagePos.y,porImageSize.x,porImageSize.y),porImage);
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
