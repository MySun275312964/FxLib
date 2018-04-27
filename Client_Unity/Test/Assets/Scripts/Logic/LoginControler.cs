﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//{"code":200,"token":"asdfghjk","data":{"id":372593,"nick_name":"test","avatar":"no pic","sex":1,"balance":0},"descrp":"\u767b\u5f55\u6210\u529f"}
[Serializable]
class RoleData
{
	public UInt64 id = 0;
	public string nick_name = "";
	public string avatar = "";
	public uint sex = 0;
	public uint balance = 0;
}
[Serializable]
class RoleDataRet
{
	public int code = 0;
	public string token = "";
	public uint last_login_server_id = 0;
	public RoleData data = new RoleData();
	public string descrp = "";
}

public class LoginControler: MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		H5Manager.Instance().GetLoginSessionResetCallBack().Add(OnLoginSessionReset);
	}

	public void OnLoginSessionReset(SessionObject obj)
	{
		m_pSession = obj;
		m_pSession.m_pfOnConnect.Add(OnConnect);
		m_pSession.m_pfOnError.Add(OnError);
		m_pSession.m_pfOnClose.Add(OnClose);

		m_pSession.RegistMessage("GameProto.LoginAckPlayerServerId", OnLoginAckPlayerServerId);
		m_pSession.RegistMessage("GameProto.LoginAckPlayerLoginResult", OnLoginAckPlayerLoginResult);
		m_pSession.RegistMessage("GameProto.LoginAckPlayerMakeTeam", OnLoginAckPlayerMakeTeam);
		m_pSession.RegistMessage("GameProto.LoginAckPlayerGameStart", OnLoginAckPlayerGameStart);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnConnect()
	{
		GameProto.PlayerRequestLoginServerId oTeam = new GameProto.PlayerRequestLoginServerId();
		byte[] pData = new byte[1024];
		FxNet.NetStream pStream = new FxNet.NetStream(FxNet.NetStream.ENetStreamType.ENetStreamType_Write, pData, 1024);
		pStream.WriteString("GameProto.PlayerRequestLoginServerId");
		byte[] pProto = new byte[oTeam.CalculateSize()];
		Google.Protobuf.CodedOutputStream oStream = new Google.Protobuf.CodedOutputStream(pProto);
		oTeam.WriteTo(oStream);
		pStream.WriteData(pProto, (uint)pProto.Length);

		m_pSession.Send(pData, 1024 - pStream.GetLeftLen());
	}

	public void OnClose()
	{
		m_textText.text = "on close!!!!";
		H5Helper.H5AlertString("session close");
	}
	public void OnError(uint dwErrorNo)
	{
		m_textText.text = "on error " + dwErrorNo.ToString() + "!!!!!!!";
		H5Helper.H5AlertString("session error " + dwErrorNo.ToString());
	}

	public void SetPort()
	{
		//ushort.TryParse(m_textPort.text, out H5Manager.Instance().m_wLoginPort);
	}

	public void MakeTeam()
	{
		GameProto.PlayerRequestLoginMakeTeam oTeam = new GameProto.PlayerRequestLoginMakeTeam();
		byte[] pData = new byte[1024];
		FxNet.NetStream pStream = new FxNet.NetStream(FxNet.NetStream.ENetStreamType.ENetStreamType_Write, pData, 1024);
		pStream.WriteString("GameProto.PlayerRequestLoginMakeTeam");
		byte[] pProto = new byte[oTeam.CalculateSize()];
		Google.Protobuf.CodedOutputStream oStream = new Google.Protobuf.CodedOutputStream(pProto);
		oTeam.WriteTo(oStream);
		pStream.WriteData(pProto, (uint)pProto.Length);

		m_pSession.Send(pData, 1024 - pStream.GetLeftLen());
	}
	
	public void TeamStart()
	{
		GameProto.PlayerRequestLoginGameStart oTeam = new GameProto.PlayerRequestLoginGameStart();
		byte[] pData = new byte[1024];
		FxNet.NetStream pStream = new FxNet.NetStream(FxNet.NetStream.ENetStreamType.ENetStreamType_Write, pData, 1024);
		pStream.WriteString("GameProto.PlayerRequestLoginGameStart");
		byte[] pProto = new byte[oTeam.CalculateSize()];
		Google.Protobuf.CodedOutputStream oStream = new Google.Protobuf.CodedOutputStream(pProto);
		oTeam.WriteTo(oStream);
		pStream.WriteData(pProto, (uint)pProto.Length);

		m_pSession.Send(pData, 1024 - pStream.GetLeftLen());
	}

	public void OnLoginAckPlayerServerId(byte[] pBuf)
	{
		GameProto.LoginAckPlayerServerId oRet = GameProto.LoginAckPlayerServerId.Parser.ParseFrom(pBuf);
		if (oRet == null)
		{
			H5Helper.H5LogStr("OnLoginAckPlayerServerId error parse");
			return;
		}

		string szUrl = GameInstance.Instance().proUrlHost + GameInstance.Instance().proGetRoleUri;
		WWWForm form = new WWWForm();
		form.AddField("platform", GameInstance.Instance().proPlatform);
		form.AddField("name", GameInstance.Instance().proName);
		form.AddField("head_img", GameInstance.Instance().proHeadImage);
		form.AddField("sex", GameInstance.Instance().proSex.ToString());
		form.AddField("access_token", GameInstance.Instance().proAccessToken);
		form.AddField("expires_date", GameInstance.Instance().proExpiresDate.ToString());
		form.AddField("openid", GameInstance.Instance().proOpenId);
		form.AddField("server_id", oRet.DwServerId.ToString());

		StartCoroutine(H5Helper.SendPost(szUrl, form, OnRoleData));
	}
	public void OnLoginAckPlayerLoginResult(byte[] pBuf)
	{
		GameProto.LoginAckPlayerLoginResult oRet = GameProto.LoginAckPlayerLoginResult.Parser.ParseFrom(pBuf);
		if (oRet == null)
		{
			H5Helper.H5LogStr("OnTest error parse");
			return;
		}

		H5Helper.H5LogStr("login ret : " + oRet.DwResult.ToString());
	}

	public void OnLoginAckPlayerMakeTeam(byte[] pBuf)
	{
		GameProto.LoginAckPlayerMakeTeam oRet = GameProto.LoginAckPlayerMakeTeam.Parser.ParseFrom(pBuf);
		if (oRet == null)
		{
			H5Helper.H5LogStr("OnLoginAckPlayerMakeTeam error parse");
			return;
		}

		H5Helper.H5LogStr(oRet.ToString());
	}
	public void OnLoginAckPlayerGameStart(byte[] pBuf)
	{
		GameProto.LoginAckPlayerGameStart oRet = GameProto.LoginAckPlayerGameStart.Parser.ParseFrom(pBuf);
		if (oRet == null)
		{
			H5Helper.H5LogStr("OnLoginAckPlayerGameStart error parse");
			return;
		}
		H5Helper.H5LogStr(oRet.ToString());

		H5Manager.Instance().ConnectGame(oRet.SzListenIp, (ushort)oRet.DwPlayerPort);
	}

	public void OnRoleData(string szData)
	{
		Debug.Log(szData);
		RoleDataRet oData = JsonUtility.FromJson<RoleDataRet>(szData);

		GameProto.PlayerRequestLogin oTest = new GameProto.PlayerRequestLogin();
		oTest.SzToken = oData.token;
		oTest.QwPlayerId = oData.data.id;
		oTest.SzAvatar = oData.data.avatar;
		oTest.SzNickName = oData.data.nick_name;
		oTest.DwSex = oData.data.sex;
		oTest.DwBalance = oData.data.balance;

		byte[] pData = new byte[2048];
		FxNet.NetStream pStream = new FxNet.NetStream(FxNet.NetStream.ENetStreamType.ENetStreamType_Write, pData, 2048);
		pStream.WriteString("GameProto.PlayerRequestLogin");
		byte[] pProto = new byte[oTest.CalculateSize()];
		Google.Protobuf.CodedOutputStream oStream = new Google.Protobuf.CodedOutputStream(pProto);
		oTest.WriteTo(oStream);
		pStream.WriteData(pProto, (uint)pProto.Length);

		m_pSession.Send(pData, 2048 - pStream.GetLeftLen());
	}

	public void OnDestroy()
	{
		if (m_pSession != null)
		{
			m_pSession.m_pfOnConnect.Remove(OnConnect);
			m_pSession.m_pfOnError.Remove(OnError);
			m_pSession.m_pfOnClose.Remove(OnClose);

			m_pSession.UnRegistMessage("GameProto.LoginAckPlayerServerId");
			m_pSession.UnRegistMessage("GameProto.LoginAckPlayerLoginResult");
			m_pSession.UnRegistMessage("GameProto.LoginAckPlayerMakeTeam");
			m_pSession.UnRegistMessage("GameProto.LoginAckPlayerGameStart");
		}
		H5Manager.Instance().GetLoginSessionResetCallBack().Remove(OnLoginSessionReset);
	}

	public UnityEngine.UI.Text m_textText;
	public SessionObject m_pSession;
}