var Point : float;
private var GetHitEffect : float;
private var targY : float;
private var PointPosition : Vector3;
var time : float;

var PointSkin : GUISkin;
var PointSkinShadow : GUISkin;

function Start() {
	
	Point = Mathf.Round(Random.Range(Point/2,Point*2));
	PointPosition = transform.position + Vector3(Random.Range(-1,1),0,Random.Range(-1,1));
	targY = Screen.height /2;
	Point = 50.0f;
	time = 30.0f;	
}

function OnGUI() {
	GUI.depth = -100;
	var screenPos2 : Vector3 = Camera.main.camera.WorldToScreenPoint (PointPosition);
	GetHitEffect += Time.deltaTime*30;
	GUI.color = new Color (1.0f,1.0f,1.0f,1.0f - (GetHitEffect - 50) / 7);
	GUI.skin = PointSkinShadow;
	//GUI.Label (Rect (screenPos2.x+8 , targY-2, 80, 70), "+" + Point.ToString());
	//GUI.Label (Rect (screenPos2.x-310 , targY+98, 80, 70), "+" + Point.ToString()+"Points\n"+time+"Seconds");
	GUI.Label (Rect (screenPos2.x-310 , targY+98,300,210), "+" + Point.ToString()+"Points\n"+"+"+time+"Seconds");
	GUI.skin = PointSkin;
	//GUI.Label (Rect (screenPos2.x+10 , targY, 120, 120), "+" + Point.ToString());
	//GUI.Label (Rect (screenPos2.x-308 , targY+100, 120, 120), "+" + Point.ToString()+"Points\n"+time+"seconds");
	GUI.Label (Rect (screenPos2.x-308 , targY+100, 340, 340), "+" + Point.ToString()+"Points\n"+"+"+time+"seconds");
}

function Update() {
	targY -= Time.deltaTime*200;
	if(targY<0)
	{
		Destroy(this);
	}
}