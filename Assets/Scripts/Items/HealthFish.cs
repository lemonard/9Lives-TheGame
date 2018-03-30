using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFish : Item {

	protected override void CollectedEffect (Cat cat)
	{
		base.CollectedEffect(cat);
		Health health = cat.gameObject.GetComponent<Health>();
        health.heal = true;
		Destroy(gameObject);

	}
}
