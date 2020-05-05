using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class enemySightAI : MonoBehaviour
    {

        public UnityEngine.AI.NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            PATROL,
            CHASE,
            INVESTIGATE
        }

        public State state;
        private bool alive;

        //Variables for Patrolling
        public GameObject[] waypoints;
        private int waypointIND;
        public float patrolSpeed = 0.5f;

        //Variables for Chasing
        public float chaseSpeed = 1f;
        public GameObject target;

        //Variables for Investigating
        private Vector3 investigateSpot;
        private float timer = 0;
        public float investigateWait = 10;

        //Variables for Sight
        public float heightMultiplier;
        public float sightDistance = 10;
            
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("waypoint");
            waypointIND = Random.Range(0, waypoints.Length);

            state = enemySightAI.State.PATROL;

            alive = true;

            heightMultiplier = 1.36f;

            StartCoroutine("FSM");
        }
        IEnumerator FSM()
        {
            while (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        Patrol();
                        break;
                    case State.CHASE:
                        Chase();
                        break;
                    case State.INVESTIGATE:
                        Investigate();
                        break;
                }
                yield return null;
            }
        }

        void Patrol()
        {
            agent.speed = patrolSpeed;
            if (Vector3.Distance(this.transform.position, waypoints[waypointIND].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointIND].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointIND].transform.position) < 2)
            {
                waypointIND = Random.Range(0, waypoints.Length);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }

        void Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
            character.Move(agent.desiredVelocity, false, false);
        }

        void Investigate()
        {
            timer += Time.deltaTime;
            
            agent.SetDestination(this.transform.position);
            character.Move(Vector3.zero,false,false);
            transform.LookAt(investigateSpot);
            if (timer >= investigateWait)
            {
                state = enemySightAI.State.PATROL;
                timer = 0;
            }
        }

        void onTriggerEnter(Collider coll)
        {
            if (coll.tag == "Player")
            {
                state = enemySightAI.State.INVESTIGATE;
                investigateSpot = coll.gameObject.transform.position;
            }
        }
        void FixedUpdate()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDistance, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDistance, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDistance, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = enemySightAI.State.CHASE;
                    target = hit.collider.gameObject;
                }
            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = enemySightAI.State.CHASE;
                    target = hit.collider.gameObject;
                }
            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = enemySightAI.State.CHASE;
                    target = hit.collider.gameObject;
                }
            }
        }
    }
}

