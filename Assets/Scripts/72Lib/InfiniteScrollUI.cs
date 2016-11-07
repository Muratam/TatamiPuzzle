using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

//現在、縦のみ

public class InfiniteScrollUI : MonoBehaviour {

	public GameObject itemOriginal;
	[SerializeField] int minValue = 2;
	[SerializeField] int maxValue = 550;
	float scrolledPos = 1;
	float ScrolledPos{
		get{ return scrolledPos;}
		set{
			if (value < minValue) scrolledPos = minValue;
			else if (value > maxValue) scrolledPos = maxValue;
			else scrolledPos = value;
		}
	}
	class IndexedRTItem{
		public RectTransform item;
		public int index;
		public IndexedRTItem(RectTransform item,int index){
			this.item = item;
			this.index = index;
		}
	}
	LinkedList<IndexedRTItem> items = new LinkedList<IndexedRTItem>();
	public Action<RectTransform,int> itemInitAction;
	public void SetItems(){

		var rtOriginal = itemOriginal.GetComponent<RectTransform> ();
		rtOriginal.anchorMax = new Vector2 (0.5f,0);
		rtOriginal.anchorMin = new Vector2 (0.5f,0);
		rtOriginal.pivot = new Vector2 (0.5f,0);

		var parentHeight = this.GetComponent<RectTransform> ().sizeDelta.y;
		var height = rtOriginal.sizeDelta.y;
		int margin = 1;
		int firstIndex = (int)scrolledPos - margin;
		int finalIndex = firstIndex + (int)(parentHeight / height) + margin;
		bool firstRemoved = false;
		bool finalRemoved = false;
		while (items.Count > 0 && items.First().index < firstIndex) {
			Destroy (items.First().item.gameObject);
			items.RemoveFirst ();
			firstRemoved = true;
		}
		while (items.Count > 0 && items.Last ().index > finalIndex) {
			Destroy (items.Last ().item.gameObject);
			items.RemoveLast ();
			finalRemoved = true;
		}
		foreach (var item in items) {
			itemPosInit(item.item,height,item.index);
		}
		if (!finalRemoved && items.Count > 0) firstIndex = items.Last ().index + 1;
		if (!firstRemoved && items.Count > 0) finalIndex = items.First ().index - 1;
		var newItems = new LinkedList<IndexedRTItem> ();
		for (int i = firstIndex ; i <= finalIndex ; i++) {
			var rt = (Instantiate (itemOriginal) as GameObject).GetComponent<RectTransform> ();
			itemPosInit (rt, height,i);
			itemInitAction (rt,i);
			newItems.AddLast(new IndexedRTItem(rt,i));
		}
		if (finalRemoved) {
			foreach (var item in newItems.Reverse()) items.AddFirst (item);
		} else {
			foreach (var item in newItems) items.AddLast (item);
		}
	}
	void itemPosInit(RectTransform rt,float height,int i){
		rt.transform.SetParent (this.transform);
		rt.localScale = new Vector3 (1,1,1);
		rt.anchoredPosition = new Vector2(0,height *(i- (int)scrolledPos - (scrolledPos %1.0f)));
	}

	void Awake(){
		ScrolledPos = scrolledPos;
	}

	void Update () {
		SetItems ();
		ScrolledPos += Input.mouseScrollDelta.y / 20f;
	}
}
