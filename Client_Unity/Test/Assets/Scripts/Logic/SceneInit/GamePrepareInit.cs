﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrepareInit : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		TeamData.Instance().Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

