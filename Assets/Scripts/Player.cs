using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] Inventory inv = null;
	[SerializeField] private Animator animator;
	[SerializeField] private HealthController healthController;

    private List<ItemDisplay> itemInRange = new List<ItemDisplay>();
    private int layerMask;
	
	public HealthController HealthController => healthController;

    void Start()
    {
        layerMask = LayerMask.GetMask("Item");
    }

    void Update()
    {
		if (healthController && !healthController.Alive)
		{
			return;
		}
		
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    ItemDisplay clickedObjDisplay = hit.transform.gameObject.GetComponent<ItemDisplay>();

                    if (clickedObjDisplay)
                    {
                        for (int x = 0; x < itemInRange.Count; x++)
                        {
                            if (itemInRange[x] == clickedObjDisplay)
                            {
                                clickedObjDisplay.DisableMesh();
                                inv.PlaceItemInInventoryFromPickup(clickedObjDisplay.item);

                                break;
                            }
                        }
                    }
                }     
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        itemInRange.Add(other.gameObject.GetComponent<ItemDisplay>());
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange.Remove(other.gameObject.GetComponent<ItemDisplay>());
    }
	
	public void TakeDamage(float damageAmount)
	{
		if (healthController)
		{
			if (!healthController.Alive)
			{
				return;
			}
			healthController.TakeDamage(damageAmount);
		}
		
		DamageResponse();
	}

	public void DamageResponse()
	{
		Debug.Log("Damage Response");
	}

	public void Die()
	{
		animator.SetBool("Shoot", false);
		animator.SetBool("isRunning", false);
		animator.SetTrigger("Death");
		_ = StartCoroutine(RestartSequence());
	}

	private IEnumerator RestartSequence()
	{
		yield return new WaitForSeconds(6f);
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
}
