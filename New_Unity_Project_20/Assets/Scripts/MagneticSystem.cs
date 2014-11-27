using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagneticSystem : MonoBehaviour {

	public List<BarMagnet> 	barMagnetList;
	List<FerroMagnet>	ferroMagnetList;
	
	static MagneticSystem _instance;
	
	const float k = 1000.0f; // uo / 4pi
	
	void Awake () {
		barMagnetList 	= new List<BarMagnet>();
		ferroMagnetList	= new List<FerroMagnet>();
	}
	
	void Update () {
	}
	
	public float GetFieldModifier(){
		return k;
	}
	
	public int AddMagnet (BarMagnet bm) {
		barMagnetList.Add(bm);
		
		GameObject[] ferros = GameObject.FindGameObjectsWithTag("Ferro");
		foreach (GameObject ferro in ferros) {
			ferro.SendMessage("AddMagnet", bm);
		}
		
		return barMagnetList.IndexOf(bm);
	}
	
	public int AddFerroMagnet (FerroMagnet fm) {
		
		bool exist = false;
		foreach (FerroMagnet fmi in ferroMagnetList) {			
			if (fmi.Equals(fm)) {
				exist = true;
			}				
		}
		
		if (exist) {
			return -1;
		}
		
		ferroMagnetList.Add(fm);
		
		GameObject[] ferros = GameObject.FindGameObjectsWithTag("Ferro");
		foreach (GameObject ferro in ferros) {
			ferro.SendMessage("AddMagnet", fm);
		}		
		
		return ferroMagnetList.IndexOf(fm);
	}
			
	public Vector3 CalcMField (Vector3 position) {

		Vector3 mField = new Vector3(0.0f, 0.0f, 0.0f);
		
		foreach (BarMagnet bmi in barMagnetList) {
			mField += CalcMField(position, bmi);
		}
		
		return mField;
	}
	
	public Vector3 CalcMFieldForVisual (Vector3 position) {

		Vector3 mField = new Vector3(0.0f, 0.0f, 0.0f);
		
		foreach (BarMagnet bmi in barMagnetList) {
			mField += CalcMFieldForVisual(position, bmi);
		}
		
		return mField;
	}	
	
	public Vector3 CalcAFieldForVisual (Vector3 position) {
		Vector3 aField = new Vector3(0.0f, 0.0f, 0.0f);
		
		foreach (BarMagnet bmi in barMagnetList) {
			aField += CalcAFieldForVisual(position, bmi);
		}
		
		return aField;
	}
	
	public Vector3 CalcMFieldForVisualCompass (Vector3 position) {
	
		Vector3 mField = new Vector3(0.0f, 0.0f, 0.0f);
		
		foreach (BarMagnet bmi in barMagnetList) {
			mField += CalcMFieldForVisualCompass(position, bmi);
		}
		
		return mField;
	}
	
	public Vector3 CalcMField (Vector3 position, BarMagnet bm) {
										
		Vector3 dir = position - bm.GetClosestEdgePosition(position);
		
		if (dir.sqrMagnitude < 100.0f) {
			dir = position - bm.transform.position;
		}
		
		Vector3 mi = bm.GetMagneticMoment();
		float midir = Vector3.Dot(mi, dir.normalized);
		
		Vector3 field = 3 * midir * dir.normalized - mi;
		field *= Mathf.Pow(dir.magnitude, -3);
							
		return k * field;
	}
	
	public Vector3 CalcAFieldForVisual (Vector3 position, BarMagnet bm) {
										
		Vector3 dir = position - bm.transform.position;
				
		Vector3 mi = bm.GetMagneticMoment();
		
		Vector3 field = Vector3.Cross(mi, dir.normalized) * Mathf.Pow(dir.magnitude, -3);
							
		return k * field;
	}	
	
	public Vector3 CalcMFieldForVisual (Vector3 position, BarMagnet bm) {
	
		Vector3 dir = position - bm.transform.position;

					
		Vector3 mi = bm.GetMagneticMoment();
		float midir = Vector3.Dot(mi, dir.normalized);
		
		Vector3 field = 3 * midir * dir.normalized - mi;				
		field *= Mathf.Pow(dir.magnitude, -3);
		field *= 1000.0f;
		
		return field;
		//return k * field
		
		//return Vector3.zero;
	}	
	
	public Vector3 CalcMFieldForVisualCompass (Vector3 position, BarMagnet bm) {
										
		Vector3 nDir = position - bm.GetNorthEdgePosition();
		Vector3 sDir = position - bm.GetSouthEdgePosition();
		Vector3 dir = position - bm.transform.position;
	
		float edgefactor = 0.15f;
		float centorfactor = 0.7f;
		
		if (dir.sqrMagnitude < 0.1f * bm.GetSize()) {
			edgefactor = 0.0f;
			centorfactor = 1.0f; 			
		}
				
		Vector3 mi = bm.GetMagneticMoment() * edgefactor;
		Vector3 mic = bm.GetMagneticMoment() * centorfactor;
		
		float minDir = Vector3.Dot(mi, nDir.normalized);
		float misDir = Vector3.Dot(mi, sDir.normalized);
		float miDir = Vector3.Dot(mi, dir.normalized);
		
		Vector3 nField = 3 * minDir * nDir.normalized - mi;
		nField *= Mathf.Pow(nDir.magnitude, -3);
		
		Vector3 sField = 3 * misDir * sDir.normalized - mi;
		sField *= Mathf.Pow(sDir.magnitude, -3);		
							
		Vector3 cField = 3 * miDir * dir.normalized - mic;
		cField *= Mathf.Pow(dir.magnitude, -3);				
		
		Vector3 field = nField + sField + cField;
		
		return k * field;
	}		
	
	public Vector3 CalcMField (Vector3 position, FerroMagnet fm) {
								
		//Vector3 dir = position - fm.transform.position;
		Vector3 dir = position - fm.GetClosestEdgePosition(position);
		
		if (dir.sqrMagnitude < 100.0f) {
			dir = position - fm.transform.position;
		}
		
		Vector3 mi = fm.GetInducedMagneticMoment();
		float midir = Vector3.Dot(mi, dir.normalized);
		
		Vector3 field = 3 * midir * dir.normalized - mi;
		field *= Mathf.Pow(dir.magnitude, -3);
							
		return k * field;
	}		
		
	public Vector3 CalcForceToMagnet (int bmIdx) {
		BarMagnet bmo = barMagnetList[bmIdx];
		Vector3 mo = bmo.GetMagneticMoment();
		
		if (bmo.IsCollide())
			return new Vector3(0.0f, 0.0f, 0.0f);
		
		return CalcForce(mo, bmo.transform.position, bmo, null);
	}
	
	public Vector3 CalcForceToFerro (Vector3 mo, Vector3 p, FerroMagnet fmo) {
		return CalcForce(mo, p, null, fmo);
	}
	
	private Vector3 CalcCellForce (Vector3 dir, Vector3 mi, Vector3 mo) {
		float modir = Vector3.Dot(mo, dir.normalized);
		float midir = Vector3.Dot(mi, dir.normalized);
		float mimo = Vector3.Dot(mi, mo);
		
		Vector3 cellForce = -15 * modir * midir * dir.normalized + 3 * mimo * dir.normalized + 3 * (midir * mo + modir * mi);
				
		if (dir.sqrMagnitude > 9.0f) {
			cellForce /= dir.sqrMagnitude * dir.sqrMagnitude;	
		} else {
			cellForce /= 81.0f;	
		}
		return k * cellForce;					
	}
	
	private Vector3 CalcForce (Vector3 mo, Vector3 p, BarMagnet bmo, FerroMagnet fmo) {
		Vector3 force = new Vector3(0.0f, 0.0f, 0.0f);
		
		foreach (BarMagnet bmi in barMagnetList) {
			
			if (bmo && (bmi.Equals(bmo)))
				continue;
			
			if (bmo && BeBlocked(bmo, bmi))
				continue;
			
			if (fmo && fmo.IsAttachedBar(bmi))
				continue;
									
			Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
			if (fmo) {
				dir = p - bmi.GetClosestEdgePosition(p);
			}
			else {	
				dir = p - bmi.transform.position;			
			}
			//Vector3 dir = p - bmi.GetClosestEdgePosition(p);
			Vector3 mi = bmi.GetMagneticMoment();
			
			force += CalcCellForce(dir, mi, mo);			
			
			//Debug.Log((bmo ? "bmo" : "fmo" ) + "force" + force + "dir" + dir + "mi" + mi + "mo" + mo);
		}
		
		if (!bmo) {
			foreach (FerroMagnet fmi in ferroMagnetList) {
				
				if (fmo && (fmi.Equals(fmo)))
					continue;
				
				if (fmo && BeBlocked(p, fmi)) {
					continue;
				}
				
				Vector3 dir = p - fmi.GetClosestEdgePosition(p);
				Vector3 mi = fmi.GetInducedMagneticMoment();
				
				Vector3 ferroForce = CalcCellForce(dir, mi, mo);
				
				// ferro Magnet 의 영향은 강한 거 한 개만
				if (ferroForce.sqrMagnitude > force.sqrMagnitude) {
					force = ferroForce;
				}
			}
		}
						
		return force;
	}
	
	public Vector3 CalcTorqueToMagnet (int bmIdx) {
		BarMagnet bmo = barMagnetList[bmIdx];
		Vector3 mo = bmo.GetMagneticMoment();
		
		return CalcTorque(mo, bmo.transform.position, bmo);
	}	
	
	public Vector3 CalcTorqueToFerro (Vector3 mo, Vector3 p) {
		return CalcTorque(mo, p, null);
	}
	
	private Vector3 CalcTorque (Vector3 mo, Vector3 p, BarMagnet bmo) {
	
		Vector3 torque = new Vector3(0.0f, 0.0f, 0.0f);
		
		foreach (BarMagnet bmi in barMagnetList) {
			
			if (bmo && bmi.Equals(bmo))
				continue;
			
			if (bmo && BeBlocked(bmo, bmi))
				continue;			
								
			Vector3 dir = p - bmi.transform.position;
			//Vector3 dir = p - bmi.GetClosestEdgePosition(p);
			Vector3 mkcrossdir = Vector3.Cross(mo, dir.normalized);
			
			Vector3 mi = bmi.GetMagneticMoment();
			float midir = Vector3.Dot(mi, dir.normalized);
						
			Vector3 mkcrossmi = Vector3.Cross(mo, mi);
						
			Vector3 cellTorque = 3 * midir * mkcrossdir - mkcrossmi;
			
			if (dir.sqrMagnitude > 100.0f) {
				cellTorque /= Mathf.Pow(dir.magnitude, 3);	
			} else {
				cellTorque /= 1000.0f;	
			}

			torque += k * cellTorque;			
		}
		
		return torque;
	}
	
	private bool BeBlocked(BarMagnet b1, BarMagnet b2) {
		Vector3 b2Edge = b2.GetClosestEdgePosition(b1.transform.position);
		Vector3 b1Edge = b1.GetClosestEdgePosition(b2.transform.position);
		Vector3 dir = b2Edge - b1Edge;
		
		bool blocked = false;
		RaycastHit hitInfo;
		if (Physics.Raycast(b1Edge - dir * 0.1f, dir, out hitInfo, dir.sqrMagnitude)) {
			if (!hitInfo.collider.transform.parent.Equals(b2.transform)) {			
				blocked = true;
			}
			//Debug.Log (hitInfo.collider);
		}
		
		//Debug.Log("blocked" + blocked);
		return blocked;
	}
	
	private bool BeBlocked(Vector3 p, FerroMagnet fm) {
		Vector3 dir = fm.transform.position - p;
		
		bool blocked = false;
		RaycastHit hitInfo;
				
		if (Physics.Raycast(p, dir, out hitInfo, dir.sqrMagnitude)) {
			if (!hitInfo.collider.Equals(fm.collider)) {			
				blocked = true;
			}
		}				
		return blocked;		
	}
	
	public static MagneticSystem Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType(typeof(MagneticSystem)) as MagneticSystem;
				if (_instance == null) {
					_instance = new GameObject("Magnetic System", typeof(MagneticSystem)).GetComponent<MagneticSystem>();
				}
			}
			return _instance;
		}
	}
}
