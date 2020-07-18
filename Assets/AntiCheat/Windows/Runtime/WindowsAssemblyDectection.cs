using System;
using UnityEngine;

namespace CQ.AntiCheat
{
	namespace Windows
	{
		public class WindowsAssemblyDectection : WindowsBase
		{
			public bool registerOnAwake = false;
			
			bool isRegistered;
			
			void Awake()
			{
				if (registerOnAwake)
				{
					Register();
				}
				
				
			}

			void OnDestroy()
			{
				Unregister();
			}

			void OnApplicationQuit()
			{
				Unregister();
			}

			void OnAssemblyLoad(object sender, AssemblyLoadEventArgs assemblyLoadEventArgs)
			{
				Debug.Log($"[Windows][AssemblyLoaded]::{assemblyLoadEventArgs.LoadedAssembly.FullName}");
				
				// filter here
			}

			void Register()
			{
				if (isRegistered)
				{
					Debug.Log("Already Registered.");
					return;
				}

				AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
				isRegistered = true;
			}

			void Unregister()
			{
				if (!isRegistered)
				{
					Debug.Log("Not Registered yet.");
					return;
				}
				
				AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoad;
				isRegistered = false;
			}
		}
	}
}