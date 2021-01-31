using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	HealthController HealthController { get; }
	void TakeDamage(float damageAmount, Vector3 hitPos);
	void DamageResponse(Vector3 hitPos);
	void Die();
}
