using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceImp{
	void Init(System.Action hdlOnFinish);
	void LoadResourceAsync(string filePath, System.Type resourceType, System.Action<Object> hdlSuc, System.Action<string> hdlError);
	void Update();
	void UnloadResource(string filePath);
	void Destroy();
}
