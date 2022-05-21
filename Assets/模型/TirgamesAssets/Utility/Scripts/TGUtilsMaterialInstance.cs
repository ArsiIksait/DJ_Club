using UnityEngine;
using System.Collections;

public class TGUtilsMaterialInstance : MonoBehaviour {
	public Color MainColor=Color.white;
	[ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
	public Color EmissionColor=Color.black;

	Material mat;
	Color oldMain;
	Color oldEmission;

	void Start () {
		mat=GetComponent<Renderer>().material;
		oldMain=MainColor;
		oldEmission=EmissionColor;
	}
	

	void Update () {
		// Check of color property was changed 		
		if (!MainColor.Equals(oldMain)) {
			oldMain=MainColor;
			mat.color=MainColor;
		}
		if (!EmissionColor.Equals(oldEmission)) {
			oldEmission=EmissionColor;
			mat.SetColor("_EmissionColor",EmissionColor);
		}
	}


}
