using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDrop : Item {

	public int yarnValue;

	protected override void CollectedEffect (Cat cat)
	{
		base.CollectedEffect(cat);
		cat.currencyAmount += yarnValue;
		Destroy (gameObject);
	}

}
