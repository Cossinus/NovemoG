using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
	private float Gold = 10;
	
	public float CurrentExperience { get; private set; }
	public float ExperienceGoal { get; private set; }

	public Interactable focus;
	
	public LayerMask movementMask;

	public Quest quest;

	Camera cam;
	PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
		cam = Camera.main;
		motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
	    if (EventSystem.current.IsPointerOverGameObject())
		    return;
	    
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100, movementMask)) {
				motor.MoveToPoint(hit.point);

				RemoveFocus();
			}
		}
		
		if (Input.GetMouseButtonDown(1)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100)) {
				Interactable interactable = hit.collider.GetComponent<Interactable>();
				if (interactable != null) {
					SetFocus(interactable);
				}
			}
		}

		if (quest.isActive)
		{
			quest.goal.EnemyKilled();
			if (quest.goal.IsReached())
			{
				CurrentExperience += quest.ExpReward;
				Gold += quest.GoldReward;
				quest.Complete();
			}
		}
    }

    void SetFocus(Interactable newFocus)
    {
	    if (newFocus != focus) {
		    if (focus != null)
			    focus.OnDefocused();
		    
		    focus = newFocus;
		    motor.FollowTarget(newFocus);
	    }

	    newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
	    if (focus != null)
			focus.OnDefocused();
	    
	    focus = null;
	    motor.StopFollowingTarget();
    }
}
