using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {

    Dictionary<string, Color> KillableColor     = new Dictionary<string, Color>();
    Dictionary<string, Color> GrabbableColor    = new Dictionary<string, Color>();
    Dictionary<string, Color> NonGrabbableColor = new Dictionary<string, Color>();
    Dictionary<string, Color> CheckpointColor   = new Dictionary<string, Color>();

    Dictionary<int, string> colorsets = new Dictionary<int, string>();
    string colorSet1 = "original";
    string colorSet2 = "exotic";
    string colorSet3 = "alternate";
    string colorSet4 = "darks";

    public enum Colorset
    {
        Original, Exotic, Alternate, Dark
    }

    public Colorset currentColor;

    // Use this for initialization
    void Start () {
        
        
	}
	
	
	void Awake()
    {
        print("color manager loaded");
        setupKillable();
        setupGrabbable();
        setupNonGrabbable();
        setupCheckpoint();
        colorsets.Add((int)Colorset.Original, "original");
        colorsets.Add((int)Colorset.Exotic, "exotic");
        colorsets.Add((int)Colorset.Alternate, "alternate");
        colorsets.Add((int)Colorset.Dark, "darks");

        setupScene(colorsets[(int)currentColor]);
    }

    void setupKillable()
    {
        //reds
        KillableColor.Add(colorSet1, new Color32(255, 0, 0, 255));
        KillableColor.Add(colorSet2, new Color32(127, 38, 38, 255));
        KillableColor.Add(colorSet3, new Color32(255, 76, 76, 255));
        KillableColor.Add(colorSet4, new Color32(127, 0, 0, 255));

    }

    void setupGrabbable()
    {
        //blues
        GrabbableColor.Add(colorSet1, new Color32(174, 174, 174, 255));
        GrabbableColor.Add(colorSet2, new Color32(38, 101, 127, 255));
        GrabbableColor.Add(colorSet3, new Color32(76, 203, 255, 255));
        GrabbableColor.Add(colorSet4, new Color32(153, 225, 255, 255));
    }

    void setupNonGrabbable()
    {
        //yellows
        NonGrabbableColor.Add(colorSet1, new Color32(0, 0, 0, 255));
        NonGrabbableColor.Add(colorSet2, new Color32(255, 222, 31, 255));
        NonGrabbableColor.Add(colorSet3, new Color32(127, 127, 54, 255));
        NonGrabbableColor.Add(colorSet4, new Color32(255, 233, 107, 255));

    }

    void setupCheckpoint()
    {
        //greens
        CheckpointColor.Add(colorSet1, new Color32(35, 176, 0, 255));
        CheckpointColor.Add(colorSet2, new Color32(57, 127, 63, 255));
        CheckpointColor.Add(colorSet3, new Color32(51, 255, 0, 255));
        CheckpointColor.Add(colorSet4, new Color32(25, 127, 0, 255));

    }

    void setupScene(string colorPallete)
    {
        GameObject[] unhookables = GameObject.FindGameObjectsWithTag("Unhookable");
        for(int i = 0; i < unhookables.Length; i++)
        {
            unhookables[i].GetComponent<SpriteRenderer>().color = NonGrabbableColor[colorPallete];
        }

        GameObject[] grabbables = GameObject.FindGameObjectsWithTag("Ground");
        for (int i = 0; i < grabbables.Length; i++)
        {
            grabbables[i].GetComponent<SpriteRenderer>().color = GrabbableColor[colorPallete];
        }

        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<SpriteRenderer>().color = CheckpointColor[colorPallete];
            print(checkpoints[i].GetComponent<SpriteRenderer>().color);
        }

        GameObject[] killables = GameObject.FindGameObjectsWithTag("Killable");
        for (int i = 0; i < killables.Length; i++)
        {
            killables[i].GetComponent<SpriteRenderer>().color = KillableColor[colorPallete];
        }
    }
}
