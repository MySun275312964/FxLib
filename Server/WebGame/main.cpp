#include "SocketSession.h"
#include "fxtimer.h"
#include "fxdb.h"
#include "fxmeta.h"
#include "GameServer.h"

#include <signal.h>
#include "gflags/gflags.h"

bool g_bRun = true;

DEFINE_string(game_manager_ip, "127.0.0.1", "game manager ip");
DEFINE_uint32(game_manager_port, 20000, "game manager port");

void EndFun(int n)
{
	if (n == SIGINT || n == SIGTERM)
	{
		g_bRun = false;
	}
	else
	{
		printf("unknown signal : %d !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n", n);
	}
}

int main(int argc, char **argv)
{
	//----------------------order can't change begin-----------------------//
	gflags::SetUsageMessage("TestServer");
	gflags::ParseCommandLineFlags(&argc, &argv, false);
	signal(SIGINT, EndFun);
	signal(SIGTERM, EndFun);

	// must define before goto
	if (!GetTimeHandler()->Init())
	{
		g_bRun = false;
		goto STOP;
	}
	GetTimeHandler()->Run();

	IFxNet* pNet = FxNetGetModule();
	if (!pNet)
	{
		g_bRun = false;
		goto STOP;
	}

	if (!GameServer::CreateInstance())
	{
		g_bRun = false;
		goto STOP;
	}

	//----------------------order can't change end-----------------------//

	if (!GameServer::Instance()->Init(inet_addr(FLAGS_game_manager_ip.c_str()), FLAGS_game_manager_port))
	{
		g_bRun = false;
		goto STOP;
	}

	while (g_bRun)
	{
		GetTimeHandler()->Run();
		pNet->Run(0xffffffff);
		FxSleep(1);
	}
	GameServer::Instance()->Stop();
	FxSleep(10);
	pNet->Release();
STOP:
	printf("error!!!!!!!!\n");
}