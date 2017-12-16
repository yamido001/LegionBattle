using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventAble<T>{
	void OnEvent(T param);
}
