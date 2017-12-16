using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager{

	EventRouterTemplate<int> mIntEventRouter = new EventRouterTemplate<int>();
	EventRouterTemplate<object> mObjectEventRouter = new EventRouterTemplate<object>();
	public void Init()
	{
		
	}

	public void RegisterIntEvent(EventId eventId, object user, System.Action<int> hdl)
	{
		mIntEventRouter.RegisterEvent ((int)eventId, user, hdl);
	}

	public void RemoveIntEvent(EventId eventId, object user)
	{
		mIntEventRouter.RemoveEvent ((int)eventId, user);
	}

	public void PostIntEvent(EventId eventId, int param)
	{
		mIntEventRouter.OnEvent ((int)eventId, param);
	}

	public void RegisterObjectEvent(EventId eventId, object user, System.Action<object> hdl)
	{
		mObjectEventRouter.RegisterEvent ((int)eventId, user, hdl);
	}

	public void RemoveObjectEvent(EventId eventId, object user)
	{
		mObjectEventRouter.RemoveEvent ((int)eventId, user);
	}

	public void PostObjectEvent(EventId eventId, object param)
	{
		mObjectEventRouter.OnEvent ((int)eventId, param);
	}

	public void Destroy()
	{
		
	}
}
