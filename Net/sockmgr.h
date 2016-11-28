#ifndef __CPSOCKMGR_H__
#define __CPSOCKMGR_H__

#include <list>
#include <map>
#include "mytcpsock.h"
#include "dynamicpoolex.h"
#include "ifnet.h"

class FxMySockMgr
{
	FxMySockMgr();
	~FxMySockMgr();

	DECLARE_SINGLETON(FxMySockMgr)

public:
	bool									Init(INT32 nMax);
	void									Uninit();

	FxTCPConnectSock*						CreateCommonTcp();
	FxWebSocketConnect*						CreateWebSocket();
	FxTCPListenSock*						Create(UINT32 dwListenId, IFxSessionFactory* pSessionFactory);
	void									Release(FxTCPConnectSock* poSock);
	void									Release(FxWebSocketConnect* poSock);
	void									Release(UINT32 dwListenId);

protected:
	INT32									m_nSockCount;
	TDynamicPoolEx<FxTCPConnectSock>		m_oTCPSockPool;
	TDynamicPoolEx<FxWebSocketConnect>		m_oWebSockPool;
	UINT32									m_dwNextId;

	std::map<UINT32, FxTCPListenSock>		m_mapListenSocks;
};

#endif	// __CPSOCKMGR_H__
