using UnityEngine;
using System.Collections;

public class StreamlineCommon : MonoBehaviour {

	static StreamlineCommon _instance;
	public MagneticSystem _magneticSystem;
	
	public Vector3 minRange, maxRange;
	private Camera cam;
	//private Plane[] planes;
	private GameObject cube;
	private Transform _transform;
		
	private Vector3 Bnorm;
	private Vector3 B;
	public float DistThreshold = 1f;
	public float BmagThreshold = 0.1f;
	float sqrDistThreshold;
	public int MIN=2, MAX=50;
	private float cellsize = 1f;
	private Color cell = new Color(1, 1, 0, .5f);
	public LineRenderer line;
	public Color c1 = Color.red, c2 = Color.cyan;
	public Material lineMaterial;
	public float startWth = 0.2f, endWth = 0.02f;
	public float stepsize = 0.4f;
	
	private Vector3[] neighborVectors = new Vector3 [8];
	private bool[] diffCount = new bool [3];
	private bool[] occupancyBuffer;
	
	private bool DomainIsView = true;
	private bool checkDomain = false;
	private bool RKmode = false;
	public int numberBin=20;
	public int[] BGate;
	
	
	public float seedStep;
	public int itrW, itrH;
	private Vector2 scrOffset;
	
	private int cpCount=0;
	private bool existSaddle=false;
	private Vector3 newpos;// = new Vector3 (0,0,0);
	
	private float gridScale = 1f;
	private bool checksaddle = false;	
	
	int numMagnet=0;
	
	void Awake () {
		_magneticSystem = MagneticSystem.Instance;
		
	}
	
	// Use this for initialization
	void Start () {
		//numMagnet = _magneticSystem.barMagnetList.Count;
		numMagnet = 3;
		_transform = transform;
		
		/*cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Destroy(cube.GetComponent("BoxCollider"));
		cube.renderer.material = new Material (Shader.Find("Transparent/Diffuse"));
		cube.renderer.material.color = new Color(1, 1, 1, .3f);*/
		sqrDistThreshold = DistThreshold*DistThreshold;
		diffCount[0] = diffCount[1] = diffCount[2] = false;		
		allocBGate();
		if(cam == null)
			cam = Camera.main;	
		if(lineMaterial == null) //Additive, Alpha Blended, Multiply, Vertexlit Blended
			lineMaterial = new Material (Shader.Find("Particles/Alpha Blended"));//Mobile/Particles/Alpha Blended
		
	}
	
	public void allocOccupancyBuffer (int itrH, int itrW) {
		occupancyBuffer = new bool [itrH * itrW];
	}
	
	public void allocBGate () {
		
		BGate = new int [numMagnet*numberBin];		
	}
	
	public void initBGate () {
		for (int i=0; i<numMagnet*numberBin; i++)
			BGate[i] = 0;
	}
	
	// Update is called once per frame
	void Update () {
		/*int i = 0;
		for (i=0; i<planes.Length; i++)
			Destroy(planes[i]);
		
		planes = GeometryUtility.CalculateFrustumPlanes(cam);
        
        while (i < planes.Length) {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            i++;
        }*/
		/*Vector3 farplaneSize; 
		farplaneSize.y = cam.farClipPlane * Mathf.Tan(Mathf.Deg2Rad*cam.fieldOfView/2f)*2f;
		farplaneSize.x = farplaneSize.y * cam.aspect;		
		farplaneSize.z = cam.farClipPlane - cam.nearClipPlane;
		
		cube.transform.position = cam.transform.position + cam.transform.rotation * (Vector3.forward*(cam.farClipPlane + cam.nearClipPlane)/2f);
		cube.transform.localScale = new Vector3 (farplaneSize.x, farplaneSize.y, farplaneSize.z);
		cube.transform.rotation = cam.transform.rotation;
		
		boundaryFromFrustum ();*/
	}
	
	public void allocLineRenderer (int num, out GameObject[] lineRenderer) {
		
		lineRenderer = new GameObject[num];
		
		for ( int i=0; i<num; i++) {
			
			lineRenderer[i] = new GameObject("Line");//.AddComponent("LineRenderer") as GameObject;
			lineRenderer[i].AddComponent("LineRenderer");
			line = (LineRenderer)lineRenderer[i].GetComponent("LineRenderer");
			line.material = lineMaterial;
			//lineRenderer[i].transform.parent = gameObject.transform;
		}
	}
	
	void boundaryFromFrustum () {
		//minRange, maxRange
		minRange = new Vector3 (_transform.position.x-_transform.localScale.x/2f, _transform.position.y-_transform.localScale.y/2f, _transform.position.z-_transform.localScale.z/2f);
		maxRange = minRange + _transform.localScale;
		
		minRange = cube.transform.position-cube.transform.localScale/2f;
		maxRange = minRange + cube.transform.localScale;
		
		/*int i = 1;
		minRange = maxRange = -planes[0].normal * planes[0].distance;
        while (i < planes.Length) {
			Vector3 pos = -planes[i].normal * planes[i].distance;
			if(minRange.x > pos.x)
				minRange.x = pos.x;
			if(minRange.y > pos.y)
				minRange.y = pos.y;
			if(minRange.z > pos.z)
				minRange.z = pos.z;
			
			if(maxRange.x < pos.x)
				maxRange.x = pos.x;
			if(maxRange.y < pos.y)
				maxRange.y = pos.y;
			if(maxRange.z < pos.z)
				maxRange.z = pos.z;
			
			i++;
		}*/
		
		//Debug.Log("minRange:" + minRange.x + " " + minRange.y + " " + minRange.z);
		//Debug.Log("maxRange:" + maxRange.x + " " + maxRange.y + " " + maxRange.z);
	}
	
	public Vector2 setScrOffset (Vector3 mpos) {
		Vector3 mScrPos = cam.WorldToScreenPoint(mpos);
		//scrOffset.y = (Mathf.FloorToInt(mScrPos.y/seedStep))*seedStep + seedStep/2f;
		//scrOffset.x = (Mathf.FloorToInt(mScrPos.x/seedStep))*seedStep + seedStep/2f;
		
		scrOffset.Set(mScrPos.x - (Mathf.FloorToInt(mScrPos.x/seedStep))*seedStep, mScrPos.y - (Mathf.FloorToInt(mScrPos.y/seedStep))*seedStep);
		return scrOffset;
	}
	
	public int getOccupancyBufferIndex (Vector3 pos, out int row, out int col) {
		Vector3 scrPos = cam.WorldToScreenPoint(pos);
		row = Mathf.Min(Mathf.Abs(Mathf.FloorToInt(scrPos.y/seedStep)),itrH-1);
		col = Mathf.Min(Mathf.Abs(Mathf.FloorToInt(scrPos.x/seedStep)),itrW-1);
		//Debug.Log("scrPos:" + scrPos.x + "," + scrPos.y + ","+scrPos.z);
		return (row*itrW + col);
	}
	
	public bool checkOccupancyBuffer (Vector3 pos) {
		int row, col;
		return (occupancyBuffer[getOccupancyBufferIndex(pos, out row, out col)]);
	}
	
	public void setOccupancyBuffer (Vector3 pos) {
		
		Vector3 scrPos = cam.WorldToScreenPoint(pos);
		int row = Mathf.Abs(Mathf.FloorToInt(scrPos.y/seedStep));
		int col = Mathf.Abs(Mathf.FloorToInt(scrPos.x/seedStep));
	
        occupancyBuffer[Mathf.Min(row,itrH-1)*itrW + Mathf.Min(col,itrW-1)] = true;
	}
	
	public bool getOccupancyBuffer (int j) {
		return occupancyBuffer[j];
	}
	
	public void setOccupancyBuffer (int i, bool value) {
		occupancyBuffer[i] = value;
	}
		
	public bool isInsideViewport (Vector3 pos) {
		
		Vector3 viewPos = cam.WorldToViewportPoint(pos);
		
        if (viewPos.x < 0F || viewPos.x > 1f || viewPos.y < 0F || viewPos.y > 1f ) {
            //print("target is out of viewport!");
			return false;
		}
        else
			return true;
	}
	
	bool searchCP (Vector3 pos, float size)
	{
		setNeighborVectors (pos, size);
		
		if (existCriticalPoint ()) {
			return true;
		}
		else
			return false;
	}
	
	Vector3 locateCriticalPoint (Vector3 pos) {
		float size = cellsize/3f;
		Vector3 offset = new Vector3(0,0,0);
		
		for(int i=-1; i<2; i++)
			for(int j=-1; j<2; j++)
				for(int k=-1; k<2; k++) {
					if(i==0 && j==0 && k==0) continue;
				
					offset.x = pos.x + i*size;
					offset.y = pos.y + j*size;
					offset.z = pos.z + k*size;
				
					if(searchCP(offset, size)) {						
						existSaddle = true;
						return (offset);
					}
				}
				
		existSaddle = false;
		return (offset);
	}
	
	void setNeighborVectors (Vector3 pos, float size) {
		
		float halfcellsize = 0.5f*size;
		
		neighborVectors[0] = calcBnorm(new Vector3(pos.x-halfcellsize, pos.y-halfcellsize, pos.z-halfcellsize));//1
		neighborVectors[1] = calcBnorm(new Vector3(pos.x-halfcellsize, pos.y-halfcellsize, pos.z+halfcellsize));//3
		neighborVectors[2] = calcBnorm(new Vector3(pos.x-halfcellsize, pos.y+halfcellsize, pos.z-halfcellsize));//2
		neighborVectors[3] = calcBnorm(new Vector3(pos.x-halfcellsize, pos.y+halfcellsize, pos.z+halfcellsize));//4		
		neighborVectors[4] = calcBnorm(new Vector3(pos.x+halfcellsize, pos.y-halfcellsize, pos.z-halfcellsize));//5		
		neighborVectors[5] = calcBnorm(new Vector3(pos.x+halfcellsize, pos.y-halfcellsize, pos.z+halfcellsize));//6
		neighborVectors[6] = calcBnorm(new Vector3(pos.x+halfcellsize, pos.y+halfcellsize, pos.z-halfcellsize));//7
		neighborVectors[7] = calcBnorm(new Vector3(pos.x+halfcellsize, pos.y+halfcellsize, pos.z+halfcellsize));//8
	}
	
	bool existCriticalPoint () {
		
		diffCount[0] = diffCount[1] = diffCount[2] = false;
		
		// neighborVectors[] contains Bx, By, Bz
		// X
		int itr=0;
		while (itr<4) {
			checkSign (itr, itr+4);
			// Possibilly EXIST critical point
			if(diffCount[0] && diffCount[1] && diffCount[2])
				return true;
			itr++;
		}
		// Y
		itr=0;
		while (itr<4) {
			checkSign (0, 2); checkSign (1, 3); checkSign (4, 6); checkSign (5, 7);
			
			// Possibilly EXIST critical point
			if(diffCount[0] && diffCount[1] && diffCount[2])
				return true;
			itr++;
		}
		// Z
		itr=0;
		while (itr<4) {
			checkSign (itr*2, itr*2 + 1);
			
			// Possibilly EXIST critical point
			if(diffCount[0] && diffCount[1] && diffCount[2])
				return true;
			itr++;
		}
		
		return false;
	}

	void checkSign (int i1, int i2) {
		// different sign X, Y, Z
		if(!diffCount[0] && (neighborVectors[i1].x > 0 && neighborVectors[i2].x < 0) || (neighborVectors[i1].x < 0 && neighborVectors[i2].x > 0))
			diffCount[0] = true;
		if(!diffCount[1] && (neighborVectors[i1].y > 0 && neighborVectors[i2].y < 0) || (neighborVectors[i1].y < 0 && neighborVectors[i2].y > 0))
			diffCount[1] = true;
		if(!diffCount[2] && (neighborVectors[i1].z > 0 && neighborVectors[i2].z < 0) || (neighborVectors[i1].z < 0 && neighborVectors[i2].z > 0))
			diffCount[2] = true;		
	}
	
	void drawCPcell (Vector3 pos) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Destroy(cube.GetComponent("BoxCollider"));
		cube.tag = "cell";
		cube.transform.localPosition = pos;
		cube.transform.localScale = new Vector3 (cellsize, cellsize, cellsize);
		cube.renderer.material = new Material (Shader.Find("Transparent/Diffuse"));
		cube.renderer.material.color = cell;
	}
	
	Vector3 seedSearch (Vector3 saddlepos, int mId, bool isForward) {
		int i=0;
		Vector3 pos = saddlepos;
		
		BarMagnet bm = _magneticSystem.barMagnetList[mId];
		Bnorm = calcBnorm (pos); 
		
		while(Terminate(pos, i++, isForward, out mId)==0 && (pos-bm.transform.position).sqrMagnitude > gridScale*gridScale) {
			if(isForward)//arrived by forward, then return by backward
				pos -= stepsize * calcBnorm(pos);
			else
				pos += stepsize * calcBnorm(pos);
			
			Bnorm = calcBnorm (pos); 
		}	
		
		return pos;
	}
	
	void rearrangeStreamlines (Vector3 saddlepos, Vector3 dir, bool isForward, int mId) {
		//newpos
		newpos = saddlepos + dir*cellsize/2f;
		/*Vector3 newSeed;
		
		// return to source magnet
		newSeed = seedSearch (newpos, mId, isForward);
		
		newpos = newSeed;
		drawCPcell(newSeed);*/
		//Debug.Log("rearrange");
	}
	
	bool checkSaddle (Vector3 pos, bool isForward, int mId) {
		existSaddle=false;
		
		// 1. cube cell around pos
		setNeighborVectors (pos, cellsize);
		
		// 2. check sign change inside cell
		if(!existCriticalPoint ())
			return false; // no saddle in this cell
				
		//drawCPcell(pos);
			
		// 3. intersection? cell center		
		Vector3 saddle = locateCriticalPoint (pos);
		if(existSaddle) {
			drawCPcell(saddle);
			
			// 4. distance check
			if((pos-saddle).sqrMagnitude < cellsize*cellsize)
				rearrangeStreamlines (saddle, (pos-saddle).normalized, isForward, mId);
		}
		else 
			return false;
		
		cpCount++;
		
		return true;
	}

	
	public int Terminate (Vector3 pos, int i, bool isForward, out int mId) {
				
		// 1. Null vector
		//Bnorm = calcBnorm (pos); 
		mId = -1;
		
		if (B.sqrMagnitude < BmagThreshold) {
			return 1;
		}
		/*if (Bnorm == new Vector3(0,0,0)) {
			//Debug.Log("Zero B");
			return 1;
		}*/
		
		// 2. Out of Volume
		if(checkDomain && !isInsideDomain(pos)) {
			return 2;
		}
					
		// 3. Reach a Magnet
		foreach (BarMagnet bmi in _magneticSystem.barMagnetList) {
			float dist = (pos - bmi.transform.position).sqrMagnitude;
			if(dist < sqrDistThreshold) {
				mId = _magneticSystem.barMagnetList.IndexOf(bmi);
				return 3;
			}
		}
		
		// 4. TOO LONG 
		if (i > MAX)
			return 4;
		
		/*if (checksaddle && mId >= 0 && checkSaddle (pos, isForward, mId)) {
			//Bnorm = calcBnorm (newpos); 
			return -1;
		}*/
		
		return 0;
	}
	
	bool isInsideDomain (Vector3 pos) {
		
		if (DomainIsView)
			return isInsideViewport(pos);
		else {//regular cube
			if (pos.x > maxRange.x || pos.y > maxRange.y || pos.z > maxRange.z)
				return false;
			else if (pos.x < minRange.x || pos.y < minRange.y || pos.z < minRange.z)
				return false;
		}	
		return true;	
	}
	
	public int Stop (Vector3 pos) {
		
		Bnorm = calcBnorm (pos); 
		
		// 2. Out of Volume
		if(!isInsideViewport(pos)) {
			return numMagnet;
		}

		// 3. Reach a Magnet
		foreach (BarMagnet bmi in _magneticSystem.barMagnetList) {
			float dist = (pos - bmi.transform.position).sqrMagnitude;
			if(dist < DistThreshold*DistThreshold)
				return _magneticSystem.barMagnetList.IndexOf(bmi);
		}

		return -1;
	}
		
	public Vector3 integrateSingleStep (Vector3 pos, bool isForward) {
		if(!RKmode) {
			Bnorm = calcBnorm (pos); 
			if(isForward)
				return(pos + stepsize * Bnorm);
			else
				return(pos - stepsize * Bnorm);
		}
		else { //RK
			Bnorm = RK4Forward(pos);
			if(isForward)
				return(pos + Bnorm);
			else 
				return(pos - Bnorm);
		}
	}
	
	public int ForwardIntegrationWthOccupancy (Vector3 seed, int mId) {
		
		Vector3 pos = seed;
		int i=0, type=0;	
					
		while(Terminate(pos, i, true, out mId) <= 0) {
			line.SetVertexCount(i+1);
			line.SetPosition(i++, pos);
			
			setOccupancyBuffer(pos);
			
			//type = Terminate(pos, i, true, mId);				
			pos = integrateSingleStep(pos, true);
		}	
	
		return i;
	}
	
	public int BackwardIntegrationWthOccupancy (Vector3 seed, int mId) {
		
		Vector3 pos = seed;
		int i=0, type=0;
				
		while(Terminate(pos, i, false, out mId) <= 0) {
			line.SetVertexCount(i+1);
			line.SetPosition(i++, pos);
			
			setOccupancyBuffer(pos);
			
			//type = Terminate(pos, i, false, mId);				
			pos = integrateSingleStep(pos, false);
		}	

		return i;
	}
		
	public int ForwardIntegration (Vector3 seed, int mId) {
		
		Vector3 pos = seed;
		int i=0, type=0, mIdout=-1;	
					
		calcB(pos);
		
		while(type <= 0) {
			line.SetVertexCount(i+1);
			line.SetPosition(i++, pos);			

			type = Terminate(pos, i, true, out mIdout);
			
			/*if(type < 0) {
				//Debug.Log("Saddle");
				return false;
			}*/
			pos = integrateSingleStep(pos, true);
		}
		
		if(type == 3)
			BGate[mIdout*numberBin+mId] = 1;
		
		/*if(type == 2) 
			line.SetColors(c1, c1);		
		else 
			line.SetColors(c1, c2);		
		*/
		return i;
	}
	
	public int BackwardIntegration (Vector3 seed, int mId) {
		
		Vector3 pos = seed;
		int i=0, type=0, mIdout;
				
		calcB(pos);
		
		while(type <= 0) {
			line.SetVertexCount(i+1);
			line.SetPosition(i++, pos);

			type = Terminate(pos, i, false, out mIdout);
			/*if(type < 0) {
				//Debug.Log("Saddle");
				return false;
			}*/
			pos = integrateSingleStep(pos, false);
		}	
		/*
		if(type == 2) 
			line.SetColors(c2, c2);		
		else 
			line.SetColors(c2, c1);
		*/
		return i;
	}
	
	public void SetColorOpacity (float MaxBsqrM, float minOpacity) {
		float opacity = getBsqrM()/MaxBsqrM + minOpacity;
		c1.a = c2.a = opacity;
	}
	
	public Vector3 calcBnorm (Vector3 pos) {
		Bnorm = calcB(pos).normalized;
		return (Bnorm);
	}
	
	public Vector3 getBnorm () {
		return Bnorm;
	}
	
	Vector3 calcB (Vector3 pos) {
		B = _magneticSystem.CalcMFieldForVisual(pos);
		return (B);			
	}
	
	public float getBsqrM () {
		return B.sqrMagnitude;
	}
	
	public Vector3 RK4Forward (Vector3 pos) {
		float h = stepsize;
		Vector3 k1 = calcBnorm(pos);
		Vector3 k2 = calcBnorm(pos + 0.5f*h * k1);
		Vector3 k3 = calcBnorm(pos + 0.5f*h * k2);
		Vector3 k4 = calcBnorm(pos + k3);
		
		Vector3 incr = h*(k1 + 2f*k2 + 2f*k3 + k4)/6f;
		return incr.normalized;
	}
	
	public Vector3 RK4Backward (Vector3 pos) {
		float h = stepsize;
		Vector3 k1 = calcBnorm(pos);
		Vector3 k2 = calcBnorm(pos - 0.5f*h * k1);
		Vector3 k3 = calcBnorm(pos - 0.5f*h * k2);
		Vector3 k4 = calcBnorm(pos - k3);
		
		Vector3 incr = h*(k1 + 2f*k2 + 2f*k3 + k4)/6f;
		return incr.normalized;
	}
	
	public void setCheckDomain (bool On) {
		checkDomain = On;
	}
	
	public static StreamlineCommon Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType(typeof(StreamlineCommon)) as StreamlineCommon;
				if (_instance == null) {
					_instance = new GameObject("Streamline Common", typeof(StreamlineCommon)).GetComponent<StreamlineCommon>();
				}
			}
			return _instance;
		}
	}
}
