using UnityEngine;
using System.Collections;

// Use this script to change periodic Animator param
public class TGUtilsAnimatorPeriodParam : MonoBehaviour {
	public float	PeriodMin=2f;
	public float	PeriodMax=4f;
	public int		IndexMin=1;
	public int		IndexMax=1;
	public bool		UseIndex=false;
	public string	ParamName="Start";
	public bool 	Loop=true;

	Animator anim;


	void Start () {
		Invoke("ChangeParam",Random.Range(PeriodMin,PeriodMax));
		anim=GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ChangeParam() {
		string s="";
		if (UseIndex) {
			s=Random.Range(IndexMin,IndexMax).ToString();
		}
		anim.SetTrigger(ParamName+s);
		if (Loop) {
			Invoke("ChangeParam",Random.Range(PeriodMin,PeriodMax));
		}
	}

}
