using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TGUtilsAnimatedTexture : MonoBehaviour {
	// Enums and classes
	public enum TMap {Main,Emission}
	public enum TOrder {Order,PingPong,Random}
	[System.Serializable]
	public class TAnimation {
		public Texture		AnimTexture;
		public TOrder		FramesOrder;
		public int 			PlayCount;
		public int			XTiles;
		public int			YTiles;
		public float 		FPS;
		public TAnimation() {
			FramesOrder=TOrder.Order;
			PlayCount=1;
			XTiles=1;
			YTiles=1;
			FPS=25f;
		}
	}
	// Public variables
	public bool					PlayOnStart=true;
	public bool					Loop=true;
	public TMap					Map=TMap.Main;
	public TOrder				AnimationsOrder=TOrder.Order;
	public List<TAnimation>		Animations=new List<TAnimation>();


	// Private variables
	TAnimation 	anim;		// current animation reference
	int 		animID;		// animation ID
	bool		pong;		// pingpong direction
	Material    mat;		// renderer.material
	Texture		lastText;	// last texture


	// Create one animation on reset
	void Reset () {
		Animations.Add(new TAnimation());
	}

	// Use this for initialization
	void Start () {
		if (PlayOnStart) {
			AnimationsStart();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}



	// Start animations
	public void AnimationsStart() {
		if (Animations.Count>0) {
			mat=GetComponent<Renderer>().material;
			lastText=mat.GetTexture("_EmissionMap");
			animID=-1;
			pong=false;
			if (AnimationsOrder==TOrder.PingPong && Animations.Count<2) {
				AnimationsOrder=TOrder.Order;
			}
			AnimationsNext();
		}
	}

	// Start Next animation
	public void AnimationsNext() {
		// Which animation to play
		switch (AnimationsOrder) {
		case TOrder.Order:
			animID++;
			if (animID>Animations.Count-1) {
				animID=0;
			}
			break;
		case TOrder.PingPong:
			if (!pong) {
				animID++;
				if (animID>Animations.Count-1) {
					animID--;
					pong=true;
				}
			}
			else {
				animID--;
				if (animID<0) {
					animID++;
					pong=false;
				}
			}
			break;
		case TOrder.Random:
			animID=Random.Range(0,Animations.Count);
			break;
		}

		// now we have animation in anim
		anim=Animations[animID];
		if (anim.AnimTexture!=null && anim.FPS>0) {
			StartCoroutine(AnimationPlay());			
		}

	}

	// Stop animation
	public void AnimationsStop() {

	}

	// Play single animation with it's properties
	IEnumerator AnimationPlay() {
		float stepx=1f/anim.XTiles;
		float stepy=1f/anim.YTiles;
		string mapname="";
		bool pong=false;
		Vector4 scaleOffset=new Vector4(stepx,stepy,0,1f-1f/anim.YTiles);
		switch (Map) {
		case TMap.Main:
			mapname="_MainTex";
			if (lastText!=anim.AnimTexture) 
				mat.mainTexture=anim.AnimTexture;
			break;
		case TMap.Emission:
			mapname="_EmissionMapScaleOffset";
			if (lastText!=anim.AnimTexture) 
				mat.SetTexture("_EmissionMap",anim.AnimTexture);
			break;
		}
		lastText=anim.AnimTexture;
		for (int i=0;i<anim.PlayCount;i++) {
			// Play all frames 
			for (int y=0;y<anim.YTiles;y++) {
				for (int x=0;x<anim.XTiles;x++) {
					switch (anim.FramesOrder) {
					case TOrder.Order:
						scaleOffset.z=x*stepx;
						scaleOffset.w=1f-stepy-y*stepy;
						break;
					case TOrder.PingPong:
						if (!pong) {
							scaleOffset.z=x*stepx;
							scaleOffset.w=1f-stepy-y*stepy;
						}
						else {
							scaleOffset.z=1f-stepx-x*stepx;
							scaleOffset.w=y*stepy;
						}
						break;
					case TOrder.Random:
						scaleOffset.z+=Random.Range(0,anim.XTiles-1)*stepx;
						scaleOffset.w+=Random.Range(0,anim.YTiles-1)*stepy;
						break;
					}
					mat.SetVector(mapname,scaleOffset);
					yield return new WaitForSeconds(1/anim.FPS);					
				}
			}
			// After play once all frames
			switch (anim.FramesOrder) {
			case TOrder.Order:
				scaleOffset.z=0;
				scaleOffset.w=0;
				break;
			case TOrder.PingPong:
				pong=!pong;
				break;
			case TOrder.Random:
				break;
			}				
		}
		// Animation finished, start next animation
		AnimationsNext();
	}

}
