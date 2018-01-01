using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class DialogEngine : MonoBehaviour {

	public Text DialogText;
	Line dialog = new Line ("Assets/Data/NormalTalk.dat");
	Dictionary<String, String> ren = new Dictionary<String, String>();
	// Use this for initialization
	void Start() {
		DialogText.text = "";
	}
	
	// Update is called once per frame
	void Update() {
		ren = dialog.GetLine (dialog.GetPointer());
		switch (ren["type"]) {
		case "normal":
			if (ren ["who"] == "narrator") {
				DialogText.text = ren ["dialog"];
			} 
			else {
				DialogText.text = ren["who"] +"\n"+ ren ["dialog"];
			}
			break;
		}	
	}

	public void OnClick() {
		DialogText.text = "";
		dialog.Next();
	}
}
	
public class Line {
	private string[] script;
	private uint pointer = 0;
	private Dictionary<String, String> flags = new Dictionary<String, String>();

	public Line (String filename) {
		StreamReader reader = new StreamReader(filename); 
		script = reader.ReadToEnd().Split(
			new[] { Environment.NewLine },
			StringSplitOptions.None
		);
		reader.Close();
	}

	public void Next() {
		string[] commands = { "-", "@" };
		this.pointer += 1;
		// empty line
		if (this.script [this.pointer].Length == 0) {
			Next();
		}
		// line start without command character
		else if (commands.Contains(this.script[this.pointer].Substring(0, 1)) == false) {
			Next();
		}

	}

	public Dictionary<String, String> GetLine(uint pointer) {
		Dictionary<String, String> res = new Dictionary<String, String> ();
		switch(this.script[pointer].Substring(0, 1)) {
		case "-":
			res.Add ("type", "normal");
			res.Add ("who", this.script [pointer].Substring (1));
			res.Add ("dialog", this.script [pointer+1]);
			break;
		case "@":
			res.Add ("type", "choose");
			res.Add ("choose_num", this.script [pointer].Substring (1));
			res.Add ("choose_a", this.script [pointer + 1]);
			break;
		}
		return res;
	}

	public uint GetPointer() {
		return this.pointer;
	}
		


}