using UnityEngine;
using System.Collections;

public class WolfLoadSave : PackUnpackableBehaviour {

	override public void Pack(StreamPacker sp){
		sp.WriteBool(gameObject.activeSelf);
	}
	
	override public void Unpack(StreamUnpacker sp){
		gameObject.SetActive(sp.ReadBool());
	}
}
