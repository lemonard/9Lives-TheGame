using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFish : Item {

	public int healAmount;

	protected override void CollectedEffect (Cat cat)
	{
		base.CollectedEffect(cat);
		cat.life += healAmount;

		Destroy(gameObject);

	}
}
