using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catnip : Item {

	protected override void CollectedEffect (Cat cat)
	{
		base.CollectedEffect(cat);
		cat.freakoutManager.IncreaseFBBar();
		Destroy(gameObject);
    
	}
}
