using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour {

	public static GameMain Instance {
		get;
		private set;
	}

	public StateManager StateMgr {
		get;
		private set;
	}

	public EventManager EventMgr {
		get;
		private set;
	}

	public UnitySceneManager SceneMgr {
		get;
		private set;
	}

	public GameSceneManager GameSceneMgr {
		get;
		private set;
	}

	public FileOperateManager FileOperateMgr {
		get;
		private set;
	}

	public PlatformManager PlatformMgr {
		get;
		private set;
	}

	public ResourceManager ResMgr {
		get;
		private set;
	}

	public FileManager FileMgr {
		get;
		private set;
	}

	public UIManager UIMgr {
		get;
		private set;
	}

	public Transform Tf {
		get;
		private set;
	}

	public HttpManager HttpMgr {
		get;
		private set;
	}

	public GDSKit.GDSManager GDSMgr { 
		get;
		private set;
	}

	public LanguageManager LanguageMgr{
		get;
		private set;
	}

	public TimeManager TimeMgr
	{
		get;
		private set;
	}

	public ProxyManager ProxyMgr
	{
		get;
		private set;
	}

	public SocketManager SocketMgr {
		get;
		private set;
	}

	private HashSet<EventId> mNotFileDepInitWaitEventSet = new HashSet<EventId>(){
		EventId.ResourceManagerInitFinish,
	};

	private HashSet<EventId> mFileDepInitWaitEventSet = new HashSet<EventId>(){
		
	};

	ulong mLastUpdateTime;
	bool mHasUpdated = false;

	void Awake(){
		DontDestroyOnLoad (gameObject);
		Tf = transform;
		Instance = this;
		StateMgr = new StateManager ();
		StateMgr.Init ();
		StateMgr.EnterState (GameStateId.StartUp, null);
	}

	/// <summary>
	/// 不依赖于文件的模块的初始化
	/// </summary>
	public void NotFileDepInit(System.Action hdlOnFinish)
	{
		EventMgr = new EventManager ();
		EventMgr.Init ();

		var enumerator = mNotFileDepInitWaitEventSet.GetEnumerator ();
		string eventIdList = string.Empty;
		while (enumerator.MoveNext ()) {
			EventId eventId = enumerator.Current;
			eventIdList += eventId.ToString () + "\t";
			EventMgr.RegisterObjectEvent (eventId, this, delegate(object eventParam) {
				EventMgr.RemoveObjectEvent(eventId, this);
				mNotFileDepInitWaitEventSet.Remove(eventId);
				if(mNotFileDepInitWaitEventSet.Count == 0)
				{
					hdlOnFinish.Invoke();
				}
			});
		}

		FileOperateMgr = new FileOperateManager ();
		FileOperateMgr.Init ();

		SceneMgr = new UnitySceneManager ();
		SceneMgr.Init ();

		PlatformMgr = new PlatformManager ();
		PlatformMgr.Init ();

		ResMgr = new ResourceManager ();
		ResMgr.Init ();

		FileMgr = new FileManager ();
		FileMgr.Init ();

		HttpMgr = new HttpManager ();
		HttpMgr.Init ();

		TimeMgr = new TimeManager ();
		TimeMgr.Init ();

		SocketMgr = new SocketManager ();
	}

	public void FileDepInit(System.Action hdlOnFinish)
	{
		var enumerator = mFileDepInitWaitEventSet.GetEnumerator ();
		while (enumerator.MoveNext ()) {
			EventId eventId = enumerator.Current;
			EventMgr.RegisterObjectEvent (eventId, this, delegate(object eventParam) {
				EventMgr.RemoveObjectEvent(eventId, this);
				mFileDepInitWaitEventSet.Remove(eventId);
				if(mFileDepInitWaitEventSet.Count == 0)
				{
					hdlOnFinish.Invoke();
				}
			});
		}
		GameSceneMgr = new GameSceneManager ();
		GameSceneMgr.Init ();
		if (mFileDepInitWaitEventSet.Count == 0) {
			hdlOnFinish.Invoke ();
		}

		GDSMgr = new GDSKit.GDSManager ();
		GDSMgr.Init ();
		GDSMgr.LoadGDS ();

		LanguageMgr = new LanguageManager ();
		LanguageMgr.Init ();

		ProxyMgr = new ProxyManager ();
		ProxyMgr.Init ();
	}

	/// <summary>
	/// UI模块的初始化
	/// </summary>
	public void UIMgrInit()
	{
		if (null == UIMgr) {
			UIMgr = new UIManager ();
			UIMgr.Init ();
			Lancher.Instance.RemoveEventSystem ();
		}
	}

	public Transform CreateChildTransform(string name)
	{
		GameObject obj = new GameObject ();
		Transform tf = obj.transform;
		tf.name = name;
		tf.parent = transform;
		tf.Reset ();
		return tf;
	}

	void Update () {

		float dtTime = 0f;
		if (mHasUpdated) {
			dtTime = TimeMgr.SinceTimeSecond (mLastUpdateTime);
		}
		mLastUpdateTime = TimeMgr.CurTimeMs;

		SocketMgr.Update ();
		StateMgr.Update ();
		ResMgr.Update ();
		HttpMgr.Update ();
		if(null != GameSceneMgr)
			GameSceneMgr.Update(dtTime);
	}

	void LateUpdate()
	{
		if(null != GameSceneMgr)
			GameSceneMgr.LateUpdate ();
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

	public void Restart()
	{
		if (null != GDSMgr) {
			GDSMgr.DestroyGDS ();
			GDSMgr = null;
		}
		if (null != LanguageMgr) {
			LanguageMgr.Destroy ();
			LanguageMgr = null;
		}
		if (null != ProxyMgr) {
			ProxyMgr.Destroy ();
			ProxyMgr = null;
		}
		EventMgr.Destroy ();
		SceneMgr.Destroy ();
		StateMgr.Destroy ();
		PlatformMgr.Destroy ();
		ResMgr.Destroy ();
		GameSceneMgr.Destroy ();
		Instance = null;
		GameObject.Destroy (gameObject);
		SceneMgr.EnterScene ("Lancher", null);
	}

	void OnDestroy()
	{
		SocketMgr.OnDestroy ();
	}
}
