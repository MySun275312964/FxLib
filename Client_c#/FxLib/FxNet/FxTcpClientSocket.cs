﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FxNet
{
	public class FxTcpClientSocket : FxClientSocket
    {
		public override void Connect()
		{
			Disconnect();
			String newServerIp = "";
			AddressFamily pAddressFamily = AddressFamily.InterNetwork;
			getIPType(m_szIp, m_nPort, out newServerIp, out pAddressFamily);
			IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(newServerIp), m_nPort);

			CreateSocket(pAddressFamily);
			try
			{
				m_hSocket.BeginConnect(ipe, new AsyncCallback(ConnectCallback), this);
			}
			catch (SocketException e)
			{
				SNetEvent pEvent = new SNetEvent();
				pEvent.eType = ENetEvtType.NETEVT_ERROR;
				pEvent.dwValue = (UInt32)e.SocketErrorCode;
				FxNetModule.Instance().PushNetEvent(this, pEvent);
				Disconnect();
				return;
			}
			catch (Exception e)
			{
				SNetEvent pEvent = new SNetEvent();
				pEvent.eType = ENetEvtType.NETEVT_ERROR;
				FxNetModule.Instance().PushNetEvent(this, pEvent);
				Disconnect();
			}
		}
		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				FxTcpClientSocket pClientSocket = (FxTcpClientSocket)ar.AsyncState;
				m_hSocket.EndConnect(ar);

				OnConnect();
			}
			catch (SocketException ex)
			{
				SNetEvent pEvent = new SNetEvent();
				pEvent.eType = ENetEvtType.NETEVT_ERROR;
				FxNetModule.Instance().PushNetEvent(this, pEvent);
				Disconnect();
			}
		}

		internal override void OnSend(int bytesSent)
		{
			throw new NotImplementedException();
		}

		public override void Update()
		{
			throw new NotImplementedException();
		}

		public override void ProcEvent(SNetEvent pEvent)
		{
			throw new NotImplementedException();
		}

		internal override void OnRecv(byte[] buffer, int bytesRead)
		{
			throw new NotImplementedException();
		}
	}
}
