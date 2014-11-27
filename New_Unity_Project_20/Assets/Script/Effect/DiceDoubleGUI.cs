using UnityEngine;
using System.Collections;


public class DiceDoubleGUI : MonoBehaviour {
	float Point;
	private float GetHitEffect;
	private float targY;
	private Vector3 PointPosition;
	float time;
	
	public GUISkin PointSkin;
	public GUISkin PointSkinShadow;

	// Use this for initialization
	void Start () {
			
		Point = Mathf.Round(Random.Range(Point/2,Point*2));
		PointPosition = transform.position;
		targY = Screen.height /2;
		Point = 10.0f;
		time = 30.0f;	
	}
	
	// Update is called once per frame
	void Update () {
		targY -= Time.deltaTime*200;
		if(targY<0)
		{
			Destroy(this);
		}
	}
	
	void OnGUI()
	{
		GUI.depth = -100;
		Vector3 screenPos2 = Camera.main.camera.WorldToScreenPoint (PointPosition);
		GetHitEffect += Time.deltaTime*30;
		GUI.color = new Color (1.0f,1.0f,1.0f,1.0f - (GetHitEffect - 50) / 7);
		GUI.skin = PointSkinShadow;
		GUI.Label (new Rect (screenPos2.x-310 , targY+98,300,210), "주사위 더블!" + Point.ToString()+"초 증가!");
		GUI.skin = PointSkin;
		GUI.Label (new Rect (screenPos2.x-308 , targY+100, 340, 340), "주사위 더블!" + Point.ToString()+"초 증가!");
	}
}
