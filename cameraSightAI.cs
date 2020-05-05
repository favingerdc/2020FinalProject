using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class cameraSightAI : MonoBehaviour
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

        //Variables for Camera Sight
        public float sightDistance = 10;
        public GameObject player;
        public Collider playerColl;
        public Camera myCam;
        private Plane[] planes;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("waypoint");
            waypointIND = Random.Range(0, waypoints.Length);

            playerColl = player.GetComponent<Collider>();

            state = cameraSightAI.State.PATROL;

            alive = true;

           

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
        void Update()
        {
            planes = GeometryUtility.CalculateFrustumPlanes(myCam);
            if (GeometryUtility.TestPlanesAABB(planes, playerColl.bounds))
            {
                Debug.Log("Player Sighted Here!");
                checkForPlayer();
            }
            else
            {

            }
        }
        void checkForPlayer()
        {
            RaycastHit hit;
            Debug.DrawRay(myCam.transform.position,transform.forward * 10, Color.green);
            if (Physics.Raycast(myCam.transform.position, transform.forward, out hit, 10)) ;
            {
                state = cameraSightAI.State.CHASE;
                target = hit.collider.gameObject;

            }
        }
    }
}

