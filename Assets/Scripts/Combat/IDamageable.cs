using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	HealthController HealthController { get; }
	void TakeDamage(float damageAmount);
	void DamageResponse();
	void Die();
}
