using System.Collections.Generic;

using Dalamud.Plugin;

using Dalamud.Plugin.Ipc;

namespace Visibility.Utils;

/// <summary>
/// Manages void and whitelist functionality for game objects
/// </summary>
public class PairedPlayerManager
{
	private readonly HashSet<nint> handledAddresses = new(capacity: 1000);

	private readonly ICallGateSubscriber<List<nint>> lopHandledGameAddresses;
	private readonly ICallGateSubscriber<List<nint>> mareHandledGameAddresses;

	public PairedPlayerManager()
	{
		this.lopHandledGameAddresses = Service.PluginInterface.GetIpcSubscriber<List<nint>>("LoporritSync.GetHandledAddresses");
		this.mareHandledGameAddresses = Service.PluginInterface.GetIpcSubscriber<List<nint>>("MareSynchronos.GetHandledAddresses");
	}

	public void Update()
	{
		this.handledAddresses.Clear();

		if (this.lopHandledGameAddresses.HasFunction)
		{
			try
			{
				foreach (nint addr in this.lopHandledGameAddresses.InvokeFunc())
					this.handledAddresses.Add(addr);
			}
			catch { }
		}

		if (this.mareHandledGameAddresses.HasFunction)
		{
			try
			{
				foreach (nint addr in this.mareHandledGameAddresses.InvokeFunc())
					this.handledAddresses.Add(addr);
			}
			catch { }
		}
	}

	public bool IsHandledAddress(nint address)
	{
		return this.handledAddresses.Contains(address);
	}

	public void Clear()
	{
		this.handledAddresses.Clear();
	}
}
