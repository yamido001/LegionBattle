using System.Collections;
using System.Collections.Generic;

public class EventRouterTemplate<T>{

	class EventHandler
	{
		public object user;
		public System.Action<T> hdl;
	}

	Dictionary<int, List<EventHandler>> mEventRouter = new Dictionary<int, List<EventHandler>>();
	Queue<List<EventHandler>> mHandlerListCache = new Queue<List<EventHandler>> ();

	public void RegisterEvent(int eventId, object user, System.Action<T> hdl)
	{
		List<EventHandler> eventHandlerList = null;
		if (!mEventRouter.TryGetValue (eventId, out eventHandlerList)) {
			eventHandlerList = new List<EventHandler> ();
			mEventRouter [eventId] = eventHandlerList;
		}
		#if UNITY_EDITOR
		//只有在编辑器上才能够检测，设备上不做检测
		EventHandler handlerFind = eventHandlerList.Find (delegate(EventHandler obj) {
			return obj.user == user;
		});
		if (null != handlerFind) {
			Logger.LogError("Event manager post not registered event,, id is " + eventId.ToString());
			return;
		}
		#endif
		EventHandler handler = new EventHandler ();
		handler.user = user;
		handler.hdl = hdl;
		eventHandlerList.Add (handler);
	}

	public void RemoveEvent(int eventId, object user)
	{
		List<EventHandler> eventHandlerList = null;
		if (!mEventRouter.TryGetValue (eventId, out eventHandlerList)) {
			return;
		}
		int removeIndex = -1;
		for (int i = 0; i < eventHandlerList.Count; ++i) {
			EventHandler handlerInfo = eventHandlerList [i];
			if (handlerInfo.user == user) {
				removeIndex = i;
				break;
			}
		}
		if (-1 != removeIndex) {
			eventHandlerList.RemoveAt (removeIndex);
		}
	}

	public void OnEvent(int eventId, T param)
	{
		List<EventHandler> eventHandlerList = null;
		if (!mEventRouter.TryGetValue (eventId, out eventHandlerList)) {
			return;
		}
		if (eventHandlerList.Count == 0) {
			//为了避免GC，在删除的时候，不会把List也删除
			return;
		}
		//之所以重新创建一个，是因为在执行事件的handler的时候，有可能会有再次的Add和Remove，遍历执行就会有问题
		List<EventHandler> tempHandlerList = CreateHandlerList();
		tempHandlerList.AddRange (eventHandlerList);

		for (int i = 0; i < tempHandlerList.Count; ++i) {
			EventHandler handlerInfo = tempHandlerList [i];
			if (null != handlerInfo.user) {
				handlerInfo.hdl.Invoke (param);
			}
		}
		RecycleHandlerList (tempHandlerList);
	}


	List<EventHandler> CreateHandlerList()
	{
		List<EventHandler> ret = null;
		if (mHandlerListCache.Count > 0) {
			ret = mHandlerListCache.Dequeue ();
			ret.Clear ();
		} 
		else {
			ret = new List<EventHandler> ();
		}
		return ret;
	}

	void RecycleHandlerList(List<EventHandler> handlerList)
	{
		mHandlerListCache.Enqueue (handlerList);
	}
}
