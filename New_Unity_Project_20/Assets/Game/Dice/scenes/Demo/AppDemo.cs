/**
 * Copyright (c) 2010-2015, WyrmTale Games and Game Components
 * All rights reserved.
 * http://www.wyrmtale.com
 *
 * THIS SOFTWARE IS PROVIDED BY WYRMTALE GAMES AND GAME COMPONENTS 'AS IS' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL WYRMTALE GAMES AND GAME COMPONENTS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */ 
using UnityEngine;
using System.Collections;

// Demo application script
public class AppDemo : MonoBehaviour {
	public static bool _start = false;
	public static bool _offRollDice = false;
	//check double
	public static int dice1Value = 0;
	public static int dice2Value = 0;
	
	
	public static bool checkPortal = false;
	public static bool selectPosition = false;
	public static Vector3 selPos;
	
	static public int PlayerTileNumber=0;
	
	void Awake()
	{
		_start = false;
		_offRollDice = false;
		dice1Value = 0;
		dice2Value = 0;
		checkPortal = false;
		selectPosition = false;
		selPos.x = 0;
		selPos.y = 0;
		selPos.z = 0;
		PlayerTileNumber = 0;
	}
	public Texture2D rollDiceTexture;	
	
	delegate void DelegateMethod();
	
	// constant of current demo mode
	private const int MODE_GALLERY = 1;
	private const int MODE_ROLL = 2;
	
	private bool rollDice;
	private bool clickForRollDice;
	// current demo mode
	private int mode = 0;
	int Value;
	float shootTime;
	// next camera position when moving the camera after switching mode
	private GameObject nextCameraPosition = null;
	// start camera position when moving the camera after switching mode
	private GameObject startCameraPosition = null;
	// store gameObject (empty) for mode MODE_ROLL camera position
    private GameObject camRoll = null;
	// store gameObject (empty) for mode MODE_GALLERY camera position
    private GameObject camGallery = null;
	// speed of moving camera after switching mode
    private float cameraMovementSpeed = 0.8F;
	private float cameraMovement = 0;

	// initial/starting die in the gallery
	private string galleryDie = "d6-red";
	private GameObject galleryDieObject = null;

	// handle drag rotating the die in the gallery
    private bool dragging = false;
    private Vector2 dragStart;
    private Vector3 dragStartAngle;
    private Vector3 dragLastAngle;
	
	// rectangle GUI area's 
	private Rect rectGallerySelectBox;
	private Rect rectGallerySelect;	
	private Rect rectModeSelect;

    // GUI gallery die selector texture
    private Texture txSelector = null;
    public GameObject Player;
	public GameObject[] GameTile  = new GameObject[40];
	public GUISkin GameGUIStyle;
	
	public Vector2 rollDicePos;
	public Vector2 rollDiceSize;
	
	
    // Use this for initialization
	void Start () {	
		
		clickForRollDice=true;
		// store/cache mode assiociated camera positions
		
		camRoll = GameObject.Find("cameraPositionRoll");
	    camGallery = GameObject.Find("cameraPositionGallery");
		// set GUI rectangles of the (screen related) gallery selector
		rectGallerySelectBox = new Rect(Screen.width - 260, 10, 250, 170);
		rectGallerySelect = new Rect(Screen.width-250,35,219,109);
		rectModeSelect =  new Rect(10,10,180,80);
		// set (first) mode to gallery
        //SetMode(MODE_GALLERY);	
		
//		Player.rigidbody.position=Test111.rigidbody.position;
		SetMode(MODE_ROLL);
//		Test1.rigidbody.position = PlaceTest.rigidbody.position;
		
	}	
	
	private void SetMode(int pMode)
	{
		// camera is already moving - mode switching - no new mode will be set and we exit here
        if (nextCameraPosition != null || pMode == mode) return;

		switch(pMode)
		{
			case MODE_GALLERY:
				// switch to gallery mode
                startCameraPosition = camRoll;
                nextCameraPosition = camGallery;
				// create die that will be displayed in the gallery
                SetGalleryDie(galleryDie);
				break;
			case MODE_ROLL:
				// switch to rolling mode
				startCameraPosition = camGallery;
                nextCameraPosition = camRoll;
				break;
		}
		
		if (nextCameraPosition!=null && mode==0)
		{
			// first time we set mode, so we do not move camera but set it at the right position
			Camera.main.transform.position = nextCameraPosition.transform.position;
			Camera.main.transform.rotation = nextCameraPosition.transform.rotation;
			// next camera position to null so camera will not start moving ( nextCameraPosition is camera moving indicator variable )
			nextCameraPosition = null;
		}		
		
		mode = pMode;
		cameraMovement = 0;		
	}

	// determine a random color
	string randomColor
	{
		get
		{
			string _color = "blue";
			int c = System.Convert.ToInt32(Random.value * 6);
			switch(c)
			{
				case 0: _color = "red"; break;
				case 1: _color = "green"; break;
				case 2: _color = "blue"; break;
				case 3: _color = "yellow"; break;
				case 4: _color = "white"; break;
				case 5: _color = "black"; break;				
			}
			return _color;
		}
	}
	
	// Update is called once per frame
	void Update () {		
		if(_start){
			Player.transform.position=GameTile[0].rigidbody.position;
			PlayerTileNumber = 0;
			Dice.Clear();
			_start=false;
		}
		/*
		if(_start)
		{	
			MovePlayer(0);
			_start = false;
		}
		*/
		// if next camera position is set we are , or have to start moving the camera.
		if (nextCameraPosition!=null)
			MoveCamera();
								
		switch(mode)
		{
			case MODE_GALLERY:
				// gallery mode to update the gallery
				UpdateGallery();
				break;
			case MODE_ROLL:
				// rolling mode to update the dice rolling
				UpdateRoll();
				break;
		}
		OnAPortal();
		
		
	}
	
	// Moving the camera
	void MoveCamera()
	{
		// increment total moving time
		cameraMovement += Time.deltaTime * 1;
		// if we surpass the speed we have to cap the movement because we are 'slerping'
		if (cameraMovement>cameraMovementSpeed) 
			cameraMovement = cameraMovementSpeed;

		// slerp (circular interpolation) the position between start and next camera position
		Camera.main.transform.position = Vector3.Slerp(startCameraPosition.transform.position, nextCameraPosition.transform.position,  cameraMovement / cameraMovementSpeed );
		// slerp (circular interpolation) the rotation between start and next camera rotation
		Camera.main.transform.rotation = Quaternion.Slerp(startCameraPosition.transform.rotation, nextCameraPosition.transform.rotation,  cameraMovement / cameraMovementSpeed );

		// stop moving if we arrived at the desired next camera postion
		if (cameraMovement == cameraMovementSpeed)
			nextCameraPosition = null;	
	}

	// updating the gallery
    void UpdateGallery()
	{
        if (!PointInRect(GuiMousePosition(), rectModeSelect) && !PointInRect(GuiMousePosition(), rectGallerySelectBox ))
        {
            // mouse position is not on GUI mode selector or gallery selector
            if (Input.GetMouseButton(Dice.MOUSE_LEFT_BUTTON))
            {
                // mouse left button is held down
                if (!dragging)
                {
                    // start dragging 
                    dragging = true;
                    // remember where (mouse coords) we started to drag and what the start angle of the die was
                    dragStart = Input.mousePosition;
                    dragStartAngle = galleryDieObject.transform.eulerAngles;
                    // stop the gallery die rotation
                    galleryDieObject.rigidbody.angularVelocity = Vector3.zero;
                }
                else
                {
                    // we are dragging
                    Vector2 delta = Input.mousePosition;
                    // calculate delta vector of the mouse position related to our drag start point
                    delta -= dragStart;
                    // normalize this vector so we can use it to determine the desired rotation angle
                    Vector2 dn = delta.normalized;
                    // initialize the rotation of gallery die to its starting point
                    galleryDieObject.transform.eulerAngles = dragStartAngle;
                    // if we move the mouse horizontal we want to rotate around the Y axis (Vector3.up -> normalized vector)
                    Vector3 mouseXRotationVector = Vector3.up;
                    // if we move the mouse vertical we want to rotate towards the camera. we calculate this normalized rotation
                    // vector by using the vector from the camera to the gallery die and rotating it 45 degrees around the Y-axis
                    Vector3 mouseYRotationVector = Vector3.Lerp(camGallery.transform.position, galleryDieObject.transform.position, 1).normalized;
                    mouseYRotationVector = Quaternion.Euler(0, 45, 0) * mouseYRotationVector;
                    // we only want to rotate around the X and Z axis when moving the mouse vertical so we set y-axis to 0
                    mouseYRotationVector.y = 0;
                    // calculate the right rotation angle and rotate the die around its position using the mouse position delta vector
                    Vector3 angle = (mouseYRotationVector * dn.y) + (mouseXRotationVector * dn.x * -1);
                    galleryDieObject.transform.RotateAround(galleryDieObject.transform.position, angle, delta.magnitude * .6F);
                    // store this angle as the 'last' angle so we can determine the right rotation when we release
                    // the mouse button and stop dragging
                    dragLastAngle = angle;
                }
            }
            else
            if (Input.GetMouseButtonUp(Dice.MOUSE_LEFT_BUTTON) && dragging)
            {
                // left mouse button was released while we were dragging
                float force = .4F;
                dragging = false;
                // add correct torque to spin the gallery die
                galleryDieObject.rigidbody.AddTorque(dragLastAngle.normalized * force, ForceMode.Impulse);
                return;
            }
        }
	}

    // dertermine random rolling force
    private GameObject spawnPoint = null;
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

	void UpdateRoll()
	{
        spawnPoint = GameObject.Find("spawnPoint");
        // check if we have to roll dice
        //if (Input.GetMouseButtonDown(Dice.MOUSE_LEFT_BUTTON) && !PointInRect(GuiMousePosition(), rectModeSelect))
		if(rollDice)
		{
			rollDice=false;
			shootTime = Time.realtimeSinceStartup;
			
			
            // left mouse button clicked so roll random colored dice 2 of each dieType
            Dice.Clear();
			
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint.transform.position, Force());
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint.transform.position, Force());
			
			//Value=Dice.Value("");
			
          /*
			Dice.Roll("1d10", "d10-" + randomColor, spawnPoint.transform.position, Force());
            Dice.Roll("1d10", "d10-" + randomColor, spawnPoint.transform.position, Force());
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint.transform.position, Force());
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint.transform.position, Force());
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint.transform.position, Force());
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint.transform.position, Force());
            */
			
		}
       /*
		else
        if (Input.GetMouseButtonDown(Dice.MOUSE_RIGHT_BUTTON) && !PointInRect(GuiMousePosition(), rectModeSelect))
        {
            // right mouse button clicked so roll 8 dice of dieType 'gallery die'
            Dice.Clear();
            string[] a = galleryDie.Split('-');
            Dice.Roll("8" + a[0], galleryDie, spawnPoint.transform.position, Force());
        }
        */
    }
	
    // handle GUI
	void OnGUI()
    {
		GUI.depth=10;
		GUI.skin = GameGUIStyle;
		if(!_offRollDice&&!ChanceTile._playerOnATile&&!checkPortal&&clickForRollDice&&!MagTile.checkMagTile&&!SolveTile.checkOnASolveTIle&&!PointUpTile.pointUpTile&&!QuestionTile._checkQ){
			//if(GUI.Button(new Rect(rollDicePos.x,rollDicePos.y,rollDiceSize.x,rollDiceSize.y),"주사위굴리기"))
			if(GUI.Button(new Rect(rollDicePos.x,rollDicePos.y+100,rollDiceSize.x,rollDiceSize.y+20),rollDiceTexture))
			{
				clickForRollDice=false;
				rollDice=true;
			}
		}
		
		// Make mode selection box
		/*
		GUI.Box (rectModeSelect, "Dice Demo");
		
		if (GUI.Button (new Rect (20,35,160,20), "Dice Gallery")) {
			SetMode(MODE_GALLERY);
		}
		
		if (GUI.Button (new Rect (20,60,160,20), "주사위 굴리기")) {
			
		}
		*/
		switch(mode)
		{
			case MODE_GALLERY:
				if (nextCameraPosition==null)
				{
                    // camera is not moving so display dice selector
						/*	
						GUI.Box (rectGallerySelectBox, "Dice Selector");						
		                    if (txSelector == null)
		                    {
		                        // determine dieType dependent selection texture
		                        string add = "";
		                        if (galleryDie.IndexOf("-dots") >= 0)
		                            // dice with dots found so we have to append -dots when loading material
		                            add = "-dots";
		                        // we have to load our selector texture
		                        txSelector = (Texture)Resources.Load("Textures/GUI-selector/select-" + galleryDie.Split('-')[0]+add);
		                    }
		                    
		                    if (txSelector != null) 						
							{
		                        // draw our selector texture
								GUI.DrawTexture(rectGallerySelect, txSelector , ScaleMode.ScaleToFit, true, 0f);
							}
						*/
                    // check current mouseposition against selector
					string status = CheckSelection(rectGallerySelect);
                   // if (status == "") status = "[select your dice and color]";

					// display status label
					GUI.Label (new Rect (Screen.width-245, 145, 230, 20), status);					
				}
                GUI.Box(new Rect((Screen.width - 350) / 2, Screen.height - 40, 350, 25), "");
                GUI.Label(new Rect(((Screen.width - 350) / 2) + 10, Screen.height - 38, 350, 22), "You can rotate the die by dragging it with the mouse.");
                break;
			case MODE_ROLL:
                // display rolling message on bottom
                //GUI.Box(new Rect((Screen.width-520)/2, Screen.height-40, 520, 25), "");
               
				//GUI.Label(new Rect(((Screen.width - 520) / 2)+10, Screen.height - 38, 520, 22), "Click with the left (all die types) or right (gallery die) mouse button in the center to roll.");
                if (Dice.Count("")>0)
                {
                    // we have rolling dice so display rolling status
                    /*
					GUI.Box(new Rect( 10 , Screen.height - 75 , Screen.width - 20 , 30), "");
		                    GUI.Label(new Rect(20, Screen.height - 70, Screen.width, 20), Dice.AsString(""));
		                    */
					//Value = Dice.AsString("");
//					print ("Dice.Value:"+Dice.Value(""));
					Value=Dice.Value("");
					//GUI.Box(new Rect( (Screen.width/2)-100 , (Screen.height/2)-100 , 200 , 200), "");
		            //GUI.Label(new Rect((Screen.width/2)-100, (Screen.height/2)-100, 200, 200),""+Value);
					if(TimeOver()){
						MovePlayer();						
						if(dice1Value==dice2Value)
						{
							MainGUI._diceDouble = true;
							var PointScript =	gameObject.AddComponent("DiceDoubleGUI");
						}
					}
                }
			 
				break;
		}	
		
	}
	
	void OnAPortal()
	{
		if(selectPosition)
		{
			Player.transform.position = selPos;
			selectPosition = false;
			checkPortal = false;
		}
	}
	void OnAPortalForAChance()
	{
		if(selectPosition)
		{			
			Player.transform.position = selPos;
			selectPosition = false;
		}
	}
	void MovePlayer()
	{
			clickForRollDice=true;
			//함수로 만든후 클릭시간 체크한 후 몇초후에만 받도록 하기
			PlayerTileNumber = PlayerTileNumber + Value;
			if(PlayerTileNumber>=40)
			{
				var PointScript =	gameObject.AddComponent("Point");
				PlayerTileNumber = PlayerTileNumber - 40;
				MainGUI.passStartPoint=true;
			}			
			Player.transform.position=GameTile[PlayerTileNumber].rigidbody.position;
			shootTime=+10000f;
	}
	
	void MovePlayer(int a)
	{
			clickForRollDice=true;		
			Player.transform.position=GameTile[a].rigidbody.position;
			PlayerTileNumber=a;
			shootTime=+10000f;
	}
	
	// check if a point is within a rectangle
	private bool PointInRect(Vector2 p, Rect r)
	{
		return  (p.x>=r.xMin && p.x<=r.xMax && p.y>=r.yMin && p.y<=r.yMax);
	}
	
	private string CheckSelection(Rect r)
	{
		string status = "";
        // mlb is true when left mouse button is clicked
        bool mlb = Input.GetMouseButtonDown(Dice.MOUSE_LEFT_BUTTON);
        // determine current GUI mouse position
        Vector2 mp = GuiMousePosition();
        // check current GUI mouse position 
        if (PointInRect(mp, new Rect(r.xMin + 12, r.yMin + 9, 200, 46)))
        {
            // we are in dice type selection so determine what dieType we are over
            // set the die if mouse button is clicked
            txSelector = null;
            if (mp.x - r.xMin < 45)
            {
                status = "d4 - not available in Dices - Light";
            }
            else
                if (mp.x - r.xMin < 75)
                {
                    if (mp.y - r.yMin < 30)
                    {
                        if (mlb) SetGalleryDie("d6-" + galleryDie.Split('-')[1] + "-dots");
                        status = "d6 dotted";
                    }
                    else
                    {
                        if (mlb) SetGalleryDie("d6-" + galleryDie.Split('-')[1]);
                        status = "d6";
                    }
                }
                else
                    if (mp.x - r.xMin < 115)
                    {
                        status = "d8  - not available in Dices - Light";
                    }
                    else
                        if (mp.x - r.xMin < 147)
                        {
                            if (mlb) SetGalleryDie("d10-" + galleryDie.Split('-')[1]);
                            status = "d10";
                        }
                        else
                            if (mp.x - r.xMin < 180)
                            {
                                status = "d12  - not available in Dices - Light";
                            }
                            else
                            {
                                status = "d20  - not available in Dices - Light";
                            }
        }
        else
            if (PointInRect(mp, new Rect(r.xMin+ 12, r.yMin + 70, 200, 28)))
            {
                // we are in dice color selection so set active color if mouse button was clicked

                // check if we had a d6 with dots
                string add = "";
                if (galleryDie.IndexOf("-dots") >= 0)
                    // dice with dots found so we have to append -dots when loading material
                    add = "-dots";               
                    
                if (mp.x - r.xMin < 45)
                {
                    if (mlb) SetGalleryDie(galleryDie.Split('-')[0] + "-red" + add);
                    status = "red";
                }
                else
                    if (mp.x - r.xMin < 75)
                    {
                        if (mlb) SetGalleryDie(galleryDie.Split('-')[0] + "-black" + add);
                        status = "black";
                    }
                    else
                        if (mp.x - r.xMin < 115)
                        {
                            if (mlb) SetGalleryDie(galleryDie.Split('-')[0] + "-white" + add);
                            status = "white";
                        }
                        else
                            if (mp.x - r.xMin < 147)
                            {
                                if (mlb) SetGalleryDie(galleryDie.Split('-')[0] + "-yellow" + add);
                                status = "yellow";
                            }
                            else
                                if (mp.x - r.xMin < 180)
                                {
                                    if (mlb) SetGalleryDie(galleryDie.Split('-')[0] + "-green" + add);
                                    status = "green";
                                }
                                else
                                {
                                    if (mlb) SetGalleryDie(galleryDie.Split('-')[0] + "-blue" + add);
                                    status = "blue";
                                }
            }
        return status;	
	}
	
	
	
	// translate Input mouseposition to GUI coordinates using camera viewport
	private Vector2 GuiMousePosition()
	{
			Vector2 mp = Input.mousePosition;
			Vector3 vp = Camera.main.ScreenToViewportPoint (new Vector3(mp.x,mp.y,0));
			mp = new Vector2(vp.x * Camera.main.pixelWidth, (1-vp.y) * Camera.main.pixelHeight);
			return mp;
	}
	public bool TimeOver(){
		return (Time.realtimeSinceStartup-shootTime)>5f;
	}
	// set spcific gallery die type	
	void SetGalleryDie(string die)
	{
        Vector3 newRotation = new Vector3(-90, -65, 0);
        Vector4 angleVelocity = Vector3.zero;
        // destroy current gallery die if we have one
        if (galleryDie != "" && galleryDieObject != null)
		{
            // save rotation and angle velocity so we can set it on the new die later
            newRotation = galleryDieObject.transform.eulerAngles;
            angleVelocity  = galleryDieObject.rigidbody.angularVelocity;
            galleryDieObject.active = false;
            // destroy die gameObject
			GameObject.Destroy(galleryDieObject);
		}		
		galleryDie = die;
		string[] a = galleryDie.Split('-');						
		GameObject g = GameObject.Find("platform-2");
		if (g!=null)
		{
            // create the new die
			galleryDieObject = Dice.prefab(a[0], g.transform.position + new Vector3(0, 3.8F, 0), newRotation , new Vector3(1.4f,1.4f,1.4f),die);
            // disable rigidBody gravity
            galleryDieObject.rigidbody.useGravity = false;
            // add saved angle and angle velocity or torque impulse
            if (angleVelocity.x == 0 && angleVelocity.y == 0 && angleVelocity.z==0)
                galleryDieObject.rigidbody.AddTorque(new Vector3(0, -.4F, 0), ForceMode.Impulse);
            else
                galleryDieObject.rigidbody.angularVelocity = angleVelocity;
        }
	}
		
}
