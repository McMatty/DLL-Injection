#include "MetaHost.h"

#ifdef BOOTSTRAP_EXPORTS
	#define BOOTSTRAPL_API __declspec(dllexport) 
#else
	#define BOOTSTRAPL_API __declspec(dllimport) 
#endif

namespace InjectionFunctions
{
	class Bootstrap
	{
	public:
		static BOOTSTRAPL_API void StartTheDotNetRuntime();
		static BOOTSTRAPL_API HRESULT LoadManagedLibrary(ICLRRuntimeHost *pClrRuntimeHost);
	};
}
