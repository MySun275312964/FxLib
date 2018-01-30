﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : SingletonObject<NetManager>
{
	void Awake()
	{
		CreateInstance(this);
	}
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
		FxNet.FxNetModule.CreateInstance();
		FxNet.FxNetModule.Instance().Init();
		FxNet.IoThread.CreateInstance();
		FxNet.IoThread.Instance().Init();
		FxNet.IoThread.Instance().Start();
	}
	
	// Update is called once per frame
	void Update ()
	{
		FxNet.FxNetModule.Instance().Run();
	}
}
