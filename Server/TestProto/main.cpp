#include "test.pb.h"
#include "proto_dispatcher.h"
#include "callback_dispatch.h"

#include "exception_dump.h"

#include <iostream>
#include "fxtimer.h"
#include "fxmeta.h"

class BBB
{
public:
	BBB(){}
};

class AAA
{
public:
	AAA(){}

	void Test(BBB* a, const test& t1){}
	void Fun(BBB& a, test& t1){}
	void Fun1(BBB& a){}
	bool Fun2(BBB& a, google::protobuf::Message& t1) { return false; }
protected:
private:
	int a;
	int b;
};

class Test : public CEventCaller<Test, 9>
{
public:
	Test()
	{
	}

	~Test()
	{
	}

	bool F1(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddSecontTimer(12, &CEventCaller<Test, 1>::GetEvent());
		return true;
	}
	bool F2(double fSecond)
	{
		static int dwT = 10;
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddSecontTimer(15, &CEventCaller<Test, 2>::GetEvent());
		--dwT;
		if (dwT <= 0)
		{
			delete this;
		}
		return true;
	}
	bool F3(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddDayHourTimer(5, &CEventCaller<Test, 3>::GetEvent());
		return true;
	}
	bool F4(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddDayHourTimer(4, &CEventCaller<Test, 4>::GetEvent());
		return true;
	}
	bool F5(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddHourTimer(2, &CEventCaller<Test, 5>::GetEvent());
		return true;
	}
	bool F6(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddMinuteTimer(15, &CEventCaller<Test, 6>::GetEvent());
		return true;
	}
	bool F7(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddMinuteTimer(12, &CEventCaller<Test, 7>::GetEvent());
		return true;
	}
	bool F8(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddTimer(15, &CEventCaller<Test, 8>::GetEvent());
		return true;
	}
	bool F9(double fSecond)
	{
		LogExe(LogLv_Debug3, "%s", "");
		GetTimeHandler()->AddTimer(10, &CEventCaller<Test, 9>::GetEvent());
		return true;
	}

private:

};


void DumpTest()
{
	int * p = NULL;
	*p = 12;

	int a1 = 1;
	int a2 = 1;

	float a = 1 / (a1 - a2);
}

int main(int argc, char **argv)
{
	ExceptionDump::RegExceptionHandler();
	AAA ta;
	CallBackDispatcher::ProtoCallBackDispatch<AAA, BBB> ta3(ta);
	//ta3.GetFunction(NULL);
	ta3.RegistFunction(test::descriptor(), &AAA::Fun2);

	BBB b;
	ta3.Dispatch("test", NULL, 0, NULL, b);

	test ttt;
	ttt.set_id(123);
	ttt.set_str("asdf");

	GetTimeHandler()->Init();
	GetTimeHandler()->Run();
	try
	{
		DumpTest();
	}
	catch (const std::exception& e)
	{
		LogExe(LogLv_Error, "%s", e.what());
	}
	catch (...) {}

	Test* t1 = new Test;
	GetTimeHandler()->AddTimer(10, &t1->CEventCaller<Test, 9>::MakeEvent(t1, &Test::F9));
	GetTimeHandler()->AddTimer(9, &t1->CEventCaller<Test, 8>::MakeEvent(t1, &Test::F8));
	GetTimeHandler()->AddTimer(8, &t1->CEventCaller<Test, 7>::MakeEvent(t1, &Test::F7));
	GetTimeHandler()->AddTimer(7, &t1->CEventCaller<Test, 6>::MakeEvent(t1, &Test::F6));
	GetTimeHandler()->AddTimer(6, &t1->CEventCaller<Test, 5>::MakeEvent(t1, &Test::F5));
	GetTimeHandler()->AddTimer(5, &t1->CEventCaller<Test, 4>::MakeEvent(t1, &Test::F4));
	GetTimeHandler()->AddTimer(4, &t1->CEventCaller<Test, 3>::MakeEvent(t1, &Test::F3));
	GetTimeHandler()->AddTimer(3, &t1->CEventCaller<Test, 2>::MakeEvent(t1, &Test::F2));
	GetTimeHandler()->AddTimer(2, &t1->CEventCaller<Test, 1>::MakeEvent(t1, &Test::F1));
	while (true)
	{
		GetTimeHandler()->Run();
		FxSleep(1);
	}
	return 0;
}
