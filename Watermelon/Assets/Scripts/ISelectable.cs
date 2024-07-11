using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{

	Vector3 GetPosition();
	void DestroyObj();
	void SelectedImageOpen();
	Sprite GetSprite();
}
