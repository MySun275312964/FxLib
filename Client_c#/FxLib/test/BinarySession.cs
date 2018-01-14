﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxNet;

namespace test
{
	public class BinarySession : FxNet.ISession
	{
		public override void Close()
		{
			if (m_pSocket == null)
			{
				return;
			}
			IoThread.Instance().DelConnectSocket(m_pSocket);
		}

		public override void Init(string szIp, UInt16 wPort)
		{
			m_pDataHeader = new BinaryDataHeader();
			m_pSocket = new FxTcpClientSocket();
			m_szIp = szIp;
			m_wPort = wPort;
		}

		public override bool IsConnected()
		{
			if (m_pSocket == null)
			{
				return false;
			}

			return m_pSocket.IsConnected();
		}

		public override void OnClose()
		{
		}

		public override void OnConnect()
		{
			var st = new System.Diagnostics.StackTrace();
			var frame = st.GetFrame(0);
			string szData = String.Format("{0}, {1}, {2}, {3}, {4}",
				frame.GetFileName(), frame.GetFileLineNumber().ToString(),
				frame.GetMethod().ToString(),
				ToString(), DateTime.Now.ToLocalTime().ToString());
			byte[] pData = Encoding.UTF8.GetBytes(szData);
			Send(pData, (UInt32)pData.Length);
		}

		public override bool OnDestroy()
		{
			return false;
		}

		public override void OnError(uint dwErrorNo)
		{
		}

		public override void OnRecv(byte[] pBuf, uint dwLen)
		{
			NetStream pNetStream = new NetStream(pBuf, dwLen);
			UInt32 dwDataLen = 0;
			pNetStream.ReadInt(ref dwDataLen);
			UInt32 dwMagicNum = 0;
			pNetStream.ReadInt(ref dwMagicNum);

			byte[] pData = new byte[dwDataLen];
			pNetStream.ReadData(ref pData, dwDataLen);

			string szData = Encoding.UTF8.GetString(pData);

			Send(pData, dwDataLen);
        }

		public override IFxClientSocket Reconnect()
		{
			m_pSocket.Init(this);
			m_pSocket.Connect(m_szIp, m_wPort);
			return m_pSocket;
		}

		public override void Release()
		{
		}

		public override bool Send(byte[] pBuf, uint dwLen)
		{
			if (IsConnected())
			{
				m_pSocket.Send(pBuf, dwLen);
				return true;
			}
			return false;
		}
	}
}
