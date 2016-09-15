#include "stdafx.h"
#include "MetaHost.h"
#include "Bootstrap.h"
#include <string>
#include <iostream>
#include <fstream>

#pragma comment(lib, "mscoree.lib")

using namespace std;

namespace InjectionFunctions
{
	void Bootstrap::StartTheDotNetRuntime()
	{		 
		ofstream myfile;
		myfile.open(L"C:\\Temp\\cppLog.txt");
		myfile << "Entered Injection function";
		myfile.close();

		ICLRMetaHost *pMetaHost = NULL;
		CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&pMetaHost);

		ICLRRuntimeInfo *pRuntimeInfo = NULL;
		pMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&pRuntimeInfo);

		ICLRRuntimeHost *pClrRuntimeHost = NULL;
		pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&pClrRuntimeHost);		

		//Run the CLR so run a >NET runtime
		pClrRuntimeHost->Start();		

		HRESULT hr = LoadManagedLibrary(pClrRuntimeHost);
		
		myfile.open(L"C:\\Temp\\cppLog.txt");
		myfile << "After to loading .NET dll";
		myfile.close();		

		// free resources
		pMetaHost->Release();
		pRuntimeInfo->Release();
		pClrRuntimeHost->Release();
	}	

	HRESULT Bootstrap::LoadManagedLibrary(ICLRRuntimeHost *pClrRuntimeHost)
	{
		// execute managed assembly
		HRESULT hr;
		DWORD pReturnValue;
		hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(
			L"C:\\Temp\\Injection\\InjectionFiles\\InjectedLibrary.dll",
			L"InjectedLibrary.Inject",
			L"CallInjected",
			L"hello .net runtime", //Currently not used
			&pReturnValue);

		return hr;
	}
}


