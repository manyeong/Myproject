using UnityEngine;
using System.Collections;

public class MagnetBatcher : MonoBehaviour {
	
	public BarMagnet 	driverMagnet;
	
	private bool		isGravitySimul = false;
	public float	    hitDist = 49f;
	
	public float distance;
	private float distanceMin, distanceMax;
	public float xSpeed = 3f;
    public float ySpeed = 3f;
	public float zSpeed = 15f;
	private float mouseX, mouseY;
		
	private Camera _camera;
	private Ray ray;
	private RaycastHit hit;
	
	// Use this for initialization
	void Start () {
		
			_camera = Camera.main;
			driverMagnet = null;
			
			if (Application.loadedLevelName == "FerromagneticMatter3") {
				isGravitySimul = true;
			}
			distanceMin = _camera.near;
			distanceMax = _camera.far;
			//lastRot = transform.rotation;
			
			ray = camera.ViewportPointToRay(new Vector3 (0.5f,0.5f,0f));
			_camera.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(MagTile.checkMagTile){
			mouseX = Input.GetAxis("Mouse X") * xSpeed;
			mouseY = Input.GetAxis("Mouse Y") * ySpeed;	
			distance = Input.GetAxis("Mouse ScrollWheel")*zSpeed;
			
			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
				Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				float castDist = 100.0f;	
				if (isGravitySimul) {
					castDist = 1000.0f;				
				}
					
				if (Physics.Raycast(ray, out hitInfo, castDist)) {
					
					if (isGravitySimul) {
						hitDist = hitInfo.distance;
					}
					if (hitInfo.collider && hitInfo.collider.transform.parent && hitInfo.collider.transform.parent.tag == "MagnetBar") {
						driverMagnet = hitInfo.collider.transform.parent.GetComponent("BarMagnet") as BarMagnet;
						/*if (driverMagnet && !driverMagnet.GetDriverSetting()) {
							driverMagnet = null;
						}*/
						hitDist = hitInfo.distance;
					}				
				}			
			}
			
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (driverMagnet) {
					driverMagnet.transform.Rotate(Vector3.forward, 90, Space.Self);
				}
			}
			
			if (driverMagnet) {
					if(Input.GetMouseButton(0)) {
					
						//hitDist += distance;
						Vector3 pos = new Vector3(mouseX, distance, mouseY);//_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, hitDist-0.1f));
						
						if (isGravitySimul) {
							float x = driverMagnet.transform.position.x;
							float y = pos.y;
							float z = driverMagnet.transform.position.z;					
											
							driverMagnet.transform.position = new Vector3(x, y, z);
											
						} else {				
							//pos.y += distance;
							driverMagnet.transform.position += pos;			
						}
					}
					if(Input.GetMouseButton(1)) {// Rotate magnet
					    //Quaternion rotation = Quaternion.Euler(0, 0, mouseX);
						driverMagnet.transform.Rotate(Vector3.forward, 5, Space.Self);
					}
			}
			else { //no driverMagnet
					if(Input.GetMouseButton(0)) // Move view
						transform.position += transform.rotation * new Vector3(-mouseX, -mouseY,0);
			
			
					if(Input.GetMouseButton(1)) {// Rotate view
					    Quaternion rotation = Quaternion.Euler(-mouseY, mouseX, 0);
						//transform.rotation *= rotation;  
						Physics.Raycast(ray, out hit);
						transform.RotateAround(hit.point, camera.transform.up, mouseX);
						transform.RotateAround(hit.point, camera.transform.right, -mouseY);
					}
					
					transform.position += transform.rotation * new Vector3(0, 0, distance);
			}
				
			if (Input.GetButtonUp("Fire1") || Input.GetMouseButtonUp(1)) {
				if (driverMagnet) {
					driverMagnet = null;
				}
				mouseY = mouseX = 0;
			}	
		}
	}	
	
	public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}
