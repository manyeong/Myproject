using UnityEngine;
using System.Collections;

public class StreamlineDynamicLength : MonoBehaviour {

	private int numberSeed;
	public int numberLayer = 20;
	private int numberBin = 20;
	public int radius = 1;
	public float xOffsetN = 1.5f, xOffsetS=1.5f;

	private int seedMode=1;
	public bool backIntegrate = false;
	GameObject[] lineRendererForward;
	GameObject[] lineRendererBackward;
	
	private Transform _transform;
	private int currentNumberSeed;
	private Vector3[] seed;
		
	private int numberSeedperMagnet;
	
	private StreamlineCommon _streamlineCommon;	
	private MagneticSystem _magneticSystem;
	
	private int numMagnet=0;
	
	private Vector3 ellipseTrans = new Vector3(0,0,0);
	private Vector3 ellipseRot = new Vector3(0,0,0);
	private Vector3 ellipseScale = new Vector3(1f,1f,1f);
		
	void Awake () {
		_streamlineCommon = StreamlineCommon.Instance;
		_magneticSystem = MagneticSystem.Instance;	
		
	}
	
	// Use this for initialization
	void Start () {
		_transform = transform;		
		//_streamlineCommon.setCheckDomain(false);
		//numMagnet = _magneticSystem.barMagnetList.Count;
		numMagnet = 3;
		numberBin = _streamlineCommon.numberBin;
		
		numberSeedperMagnet = numberBin * 2;//n and s
		numberSeed = numberSeedperMagnet * numMagnet;// + numberBin;		
		
		//xOffsetS = xOffsetN;	
		
		CreateLines ();		
		UpdateLines ();
		
	}
	
	// Update is called once per frame
	void Update () {

		_streamlineCommon.initBGate();
		UpdateLines();
		
		if (seedMode == 1) 
			seedRegularEllipse ();
		else if (seedMode == 0)
			seedRandom ();		
	}
	
	void seedRegularEllipse () {
		
			int id = 0; // magnet id
			Quaternion rot = Quaternion.identity;
			rot.eulerAngles = ellipseRot;
			_streamlineCommon.initBGate();
		
			foreach (BarMagnet m in _magneticSystem.barMagnetList) {				
				
				for(int j=0; j<numberBin; j++) {
					int index = id*numberSeedperMagnet + j;
					Vector3 tmp = new Vector3(seed[index].x+xOffsetN, seed[index].y, seed[index].z);//rot * m.transform.localRotation * Vector3.Scale(ellipseScale, seed[index]);			
					
					tmp = m.transform.position + m.transform.localRotation * tmp;
					
					integrate (tmp, index, j);
			}
				id++;
			}
		//if(backIntegrate) {
			
		id = 0;
		foreach (BarMagnet m in _magneticSystem.barMagnetList) {
			for(int j=numberBin; j<numberSeedperMagnet; j++) {
				
				if(backIntegrate && _streamlineCommon.BGate[(id-1)*numberBin + j] == 1)
					continue;
				
					int index = id*numberSeedperMagnet + j;
					Vector3 tmp = new Vector3(seed[index].x-xOffsetS, seed[index].y, seed[index].z);//rot * m.transform.localRotation * Vector3.Scale(ellipseScale, seed[index]);			
					
					tmp = m.transform.position + m.transform.localRotation * tmp;
					
					integrate (tmp, index, j-numberBin);
						//tmp = _streamlineCommon.newpos;
					
					//tmp = Quaternion.Inverse(m.transform.localRotation)*Quaternion.Inverse(rot)*(tmp - ellipseTrans - m.transform.position);
					//seed[index] = tmp.normalized;//Vector3.Scale(new Vector3(1/ellipseScale.x, 1/ellipseScale.y, 1/ellipseScale.z),tmp);
				}
				id++;
		}
		//}
	}
	
	void seedRandom () {
		int id = 0; // magnet id
			
			//int number = numberSeedperMagnet;
		foreach (BarMagnet m in _magneticSystem.barMagnetList) {
			id++;
			
			for(int j=0; j<numberSeedperMagnet; j++) {
					int index = id*numberSeedperMagnet + j;
			}
		}
	}

	bool integrate (Vector3 tmp, int index, int mId) {
		_streamlineCommon.line = (LineRenderer)lineRendererForward[index].GetComponent("LineRenderer");
		int lengthForward = _streamlineCommon.ForwardIntegration (tmp, mId)-1;

		
		_streamlineCommon.line = (LineRenderer)lineRendererBackward[index].GetComponent("LineRenderer");
		int lengthBackward = _streamlineCommon.BackwardIntegration (tmp, mId)-1;

		
		//calc Seed Color
		Color seedColor = Color.Lerp(_streamlineCommon.c1, _streamlineCommon.c2, (float)(lengthForward)/(lengthForward+lengthBackward));
		_streamlineCommon.line = (LineRenderer)lineRendererForward[index].GetComponent("LineRenderer");
		_streamlineCommon.line.SetColors (seedColor, _streamlineCommon.c1);
		
		_streamlineCommon.line = (LineRenderer)lineRendererBackward[index].GetComponent("LineRenderer");
		_streamlineCommon.line.SetColors (seedColor, _streamlineCommon.c2);
		
		return true;
	}
	
	void updateSeeds (BarMagnet m, int id, int number) {
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = ellipseRot;//new Vector3(ellipseRot.x, ellipseRot.y, ellipseRot.z);//new Vector3 (0, 0, 360f/number);
		
		for(int i=0; i<number; i++) {
			seed[id*number + i] = Vector3.Scale(ellipseScale, seed[id*number + i]);			
			seed[id*number + i] = rot * m.transform.localRotation * seed[id*number + i] + ellipseTrans + m.transform.position;
			
			//Debug.Log("trans"+ ":" + _transform.localPosition.x + " " + _transform.localPosition.y + " " + _transform.localPosition.z);
			//Debug.Log("trans"+ ":" + _transform.position.x + " " + _transform.position.y + " " + _transform.position.z);
		}
	}

	void allocSeedsLineRenderer (int num) {
		
		// Alloc Seeds
		seed = new Vector3 [num];
		
		// Alloc lineRenderers
		_streamlineCommon.allocLineRenderer(num, out lineRendererForward);
		_streamlineCommon.allocLineRenderer(num, out lineRendererBackward);
	}
		
	void random (int num) {
		// Alloc memory
		allocSeedsLineRenderer (num);
			
		// Set position
		for ( int i=0; i<num; i++) {
			seed[i] = new Vector3(Random.Range(StreamlineCommon.Instance.minRange.x, StreamlineCommon.Instance.maxRange.x), Random.Range(StreamlineCommon.Instance.minRange.y, StreamlineCommon.Instance.maxRange.y), Random.Range(StreamlineCommon.Instance.minRange.z, StreamlineCommon.Instance.maxRange.z));
			//Debug.Log("seed" +i+":"+ seed[i].x + " " + seed[i].y + " " + seed[i].z);
		}
	}
	
	void centerCircle (int num) {
		// Alloc memory
		allocSeedsLineRenderer (num);
		
		// Set position
		int id = 0;
		foreach (BarMagnet bmi in _magneticSystem.barMagnetList) {
			for(int i=0; i<num; i++) {
				seed[id*numberSeedperMagnet + i].Set (0, bmi.transform.localScale.y * (i+1), 0);
			}
			id++;
		}
	}
	
	void ellipse (int num) {
		
		// Alloc memory
		allocSeedsLineRenderer (num);
	
		// Set position
		int id = 0;
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (360f/numberBin,0,0);
		
		foreach (BarMagnet bmi in _magneticSystem.barMagnetList) {
			
			// the 1st seed
			seed[id*numberSeedperMagnet].Set (0,radius,0);
			seed[id*numberSeedperMagnet + numberBin].Set (0,radius,0);
			
			for(int i=1; i<numberBin; i++) {
				seed[id*numberSeedperMagnet + i] = rot * seed[id*numberSeedperMagnet + i-1];
				seed[id*numberSeedperMagnet + numberBin + i] = rot * seed[id*numberSeedperMagnet + numberBin + i-1];
			} 	
			
			id++;
		}

	}
	
	// 1 ellipse for Each magnet
	void initSeedPos (BarMagnet m, int id, int number) {
		
		int i=0;
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (360f/number, 0, 0);
		
		// the 1st seed
		seed[id*number].Set (0,1,0);
		
		for(i=1; i<number; i++) {
			seed[id*number + i] = rot * seed[id*number + i-1];	
			//Debug.Log("pos"+ i + ":" + initpos[i].x + " " + initpos[i].y + " " + initpos[i].z);				
		} 		
	}
	
	void CreateLines () {		
		
		currentNumberSeed = numberSeed;
		
		//centerCircle (currentNumberSeed);
		ellipse (currentNumberSeed);			
	}
	
	void UpdateLines () {
		
		for(int i=0; i<numberSeed; i++) {
			_streamlineCommon.line = (LineRenderer)lineRendererForward[i].GetComponent("LineRenderer");			
			_streamlineCommon.line.SetWidth(_streamlineCommon.startWth, _streamlineCommon.endWth); 
			_streamlineCommon.line.SetVertexCount(0);
			
			_streamlineCommon.line = (LineRenderer)lineRendererBackward[i].GetComponent("LineRenderer");		
			_streamlineCommon.line.SetWidth(_streamlineCommon.startWth, _streamlineCommon.endWth); 
			_streamlineCommon.line.SetVertexCount(0);
		}
	}
	
}
