#include "ChatServerSession.h"
#include "netstream.h"

const static unsigned int g_dwChatServerSessionBuffLen = 64 * 1024;
static char g_pChatServerSessionBuf[g_dwChatServerSessionBuffLen];

ChatServerSession::ChatServerSession()
	: m_dwChatPort(0)
	, m_dwChatServerPort(0)
{
}


ChatServerSession::~ChatServerSession()
{
}

void ChatServerSession::OnConnect(void)
{

}

void ChatServerSession::OnClose(void)
{

}

void ChatServerSession::OnError(UINT32 dwErrorNo)
{

}

void ChatServerSession::OnRecv(const char* pBuf, UINT32 dwLen)
{
	CNetStream oStream(pBuf, dwLen);
	Protocol::EChatProtocol eProrocol;
	oStream.ReadInt((int&)eProrocol);
	const char* pData = pBuf + sizeof(UINT32);
	dwLen -= sizeof(UINT32);

	switch (eProrocol)
	{
		case Protocol::CHAT_SEND_CHAT_MANAGER_INFO:	OnChatServerInfo(pData, dwLen);	break;
		default:	Assert(0);	break;
	}
}

void ChatServerSession::Release(void)
{
}

void ChatServerSession::OnChatServerInfo(const char* pBuf, UINT32 dwLen)
{
	CNetStream oStream(pBuf, dwLen);
	oStream.ReadInt(m_dwChatPort);
	oStream.ReadInt(m_dwChatServerPort);

	ChatServerSessionManager::Instance()->OnChatServerInfo(this);
}

//----------------------------------------------------------------------
FxSession* ChatServerSessionManager::CreateSession()
{
	m_oLock.Lock();
	FxSession* pSession = NULL;
	for (int i = 0; i < ChatConstant::g_dwChatServerNum; ++i)
	{
		if (m_oChatServerSessions[i].GetConnection() == NULL)
		{
			pSession = &m_oChatServerSessions[i];
		}
	}
	m_oLock.UnLock();
	return pSession;
}

void ChatServerSessionManager::Release(FxSession* pSession)
{

}

void ChatServerSessionManager::OnChatServerInfo(ChatServerSession* pChatServerSession)
{
	stCHAT_MANAGER_NOTIFY_CHAT_INFO oCHAT_MANAGER_NOTIFY_CHAT_INFO;
	for (unsigned int i = 0; i < ChatConstant::g_dwChatServerNum; ++i)
	{
		if (pChatServerSession == m_oChatServerSessions + i)
		{
			// ͬһ������
			for (unsigned int j = 0; j < ChatConstant::g_dwHashGen; ++j)
			{
				if (j % ChatConstant::g_dwHashGen == i)
				{
					m_mapSessionIpPort[j] = pChatServerSession;
				}
			}
			oCHAT_MANAGER_NOTIFY_CHAT_INFO.dwHashIndex = i;
		}
		else
		{
			if (m_oChatServerSessions[i].m_dwChatPort && m_oChatServerSessions[i].m_dwChatServerPort)
			{
				stCHAT_MANAGER_NOTIFY_CHAT_INFO::stRemoteChatInfo oInfo;
				oInfo.dwIp = m_oChatServerSessions[i].GetRemoteIP();
				oInfo.dwPort = m_oChatServerSessions[i].GetRemotePort();
				oInfo.dwHashIndex = i;
			}
		}
	}
	CNetStream oStream(ENetStreamType_Write, g_pChatServerSessionBuf, g_dwChatServerSessionBuffLen);
	oStream.WriteInt(Protocol::CHAT_MANAGER_NOTIFY_CHAT_INFO);
	oCHAT_MANAGER_NOTIFY_CHAT_INFO.Write(oStream);
	pChatServerSession->Send(g_pChatServerSessionBuf, g_dwChatServerSessionBuffLen - oStream.GetDataLength());
}