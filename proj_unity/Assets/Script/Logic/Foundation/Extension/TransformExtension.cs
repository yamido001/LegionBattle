﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension{

	public static void Reset(this Transform tf)
	{
		tf.localPosition = Vector3.zero;
		tf.localRotation = Quaternion.identity;
		tf.localScale = Vector3.one;
	}


}
