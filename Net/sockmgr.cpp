#include "sockmgr.h"
#include <time.h>

IMPLEMENT_SINGLETON(FxMySockMgr)

FxMySockMgr::FxMySockMgr()
{
	m_nSockCount		= 0;
    m_dwNextId          = 0;
}

FxMySockMgr::~FxMySockMgr()
{

}

bool FxMySockMgr::Init(INT32 nMax)
{
    if (!m_oTCPSockPool.Init(nMax, nMax / 2, false, MAX_SOCKET_COUNT))
    {
        return false;
    }

    m_nSockCount    = nMax;
    m_dwNextId      = 0;
	return true;
}

void FxMySockMgr::Uninit()
{
//     CancelIo((HANDLE)hSock);
//     closesocket(hSock);
}

FxTCPConnectSock* FxMySockMgr::CreateCommonTcp()
{
	FxTCPConnectSock* poSock = NULL;
	if (NULL == poSock)
	{
		return NULL;
	}
	if (!poSock->Init())
	{
		Release(poSock);
		return NULL;
	}

	poSock->SetState(SSTATE_INVALID);
	poSock->SetSock(INVALID_SOCKET);
	poSock->SetSockId(m_dwNextId++);

	return poSock;
}

FxWebSocketConnect* FxMySockMgr::CreateWebSocket()
{
	FxWebSocketConnect* poSock = m_oWebSockPool.FetchObj();

	if (NULL == poSock)
	{
		return NULL;
	}
	if (!poSock->Init())
	{
		Release(poSock);
		return NULL;
	}
	poSock->SetState(SSTATE_INVALID);
	poSock->SetSock(INVALID_SOCKET);
	poSock->SetSockId(m_dwNextId++);

	return poSock;
}

FxTCPListenSock* FxMySockMgr::Create(UINT32 dwListenId, IFxSessionFactory* pSessionFactory)
{
	if (m_mapListenSocks.find(dwListenId) != m_mapListenSocks.end())
	{
		return &m_mapListenSocks[dwListenId];
	}

	//这里的拷贝构造有问题~~~~//
//	m_mapListenSocks[dwListenId] = FxListenSock();
	if (!m_mapListenSocks[dwListenId].IFxListenSocket::Init(pSessionFactory))
	{
		m_mapListenSocks.erase(dwListenId);
		return NULL;
	}

	m_mapListenSocks[dwListenId].SetState(SSTATE_INVALID);
	m_mapListenSocks[dwListenId].SetSock(INVALID_SOCKET);
	m_mapListenSocks[dwListenId].SetSockId(m_dwNextId++);

	return &m_mapListenSocks[dwListenId];
}

void FxMySockMgr::Release(FxTCPConnectSock* poSock)
{
	if(NULL == poSock)
    {
		return;
    }
    poSock->Reset();
    m_oTCPSockPool.ReleaseObj(poSock);
}

void FxMySockMgr::Release(FxWebSocketConnect* poSock)
{
	if (NULL == poSock)
	{
		return;
	}
	poSock->Reset();
	m_oWebSockPool.ReleaseObj(poSock);
}

void FxMySockMgr::Release(UINT32 dwListenId)
{
	if (m_mapListenSocks.find(dwListenId) == m_mapListenSocks.end())
	{
		return;
	}

	m_mapListenSocks.erase(dwListenId);
}

