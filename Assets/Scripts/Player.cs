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
	[SerializeField] private GameObject bloodParticle;

    private List<ItemDisplay> itemInRange = new List<ItemDisplay>();
    private int layerMask;
	
	public HealthController HealthController => healthController;

	private GameObject[] bloodPool = new GameObject[20];
	
	private void Awake()
	{
		if (bloodParticle)
		{
			for (int i = 0; i < bloodPool.Length; i++)
			{
				bloodPool[i] = Instantiate(bloodParticle);
				bloodPool[i].gameObject.SetActive(false);
			}
		}
	}
	
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
		
        if (Input.GetKey(KeyCode.F))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    ItemDisplay clickedObjDisplay = hit.transform.gameObject.GetComponentInParent<ItemDisplay>();

                    if (clickedObjDisplay)
                    {
                        for (int x = 0; x < itemInRange.Count; x++)
                        {
                            if (itemInRange[x] == clickedObjDisplay)
                            {
                                inv.PlaceItemInInventoryFromPickup(clickedObjDisplay.item, hit.transform.parent.gameObject);
                                clickedObjDisplay.DisableObject();

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
        itemInRange.Add(other.gameObject.GetComponentInParent<ItemDisplay>());
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange.Remove(other.gameObject.GetComponentInParent<ItemDisplay>());
    }
	
	public void TakeDamage(float damageAmount, Vector3 hitPos)
	{
		if (healthController)
		{
			if (!healthController.Alive)
			{
				return;
			}
			healthController.TakeDamage(damageAmount);
		}
		
		DamageResponse(hitPos);
	}

	public void DamageResponse(Vector3 hitPos)
	{
		GameObject bloodfx = null;

		for (int i = 0; i < bloodPool.Length; i++)
		{
			if (!bloodPool[i].activeInHierarchy)
			{
				bloodfx = bloodPool[i];
				break;
			}
		}

		if (!bloodfx)
		{
			bloodfx = bloodPool[0];
		}
		bloodfx.SetActive(false);
		bloodfx.transform.position = hitPos;
		bloodfx.SetActive(true);

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
