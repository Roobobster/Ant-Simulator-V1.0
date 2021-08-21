using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour
{

   
    [SerializeField]
    private float speed;
    [SerializeField]
    private float changeDirectionMinTime;
    [SerializeField]
    private float changeDirectionMaxTime;
    [SerializeField]
    private float distanceToDeposit;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private Transform foodTrail;
    [SerializeField]
    private float trailDropDelay;
    [SerializeField]
    private float trailFollowReleaseDistance;



    private Vector2 direction;
    private float randomMoveTimeLeft;
    private float dropScentTimeLeft;

    private Transform trailFollowing;
    private bool isFollowingTrail;



    private bool hasFood;

    private bool hasScent;
    private Transform foodScent;

    private Vector3 homeVector;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private int inTrailScent;

    // Use this for initialization
    void Start () {
        randomMoveTimeLeft = 0;
        dropScentTimeLeft = 0;
        inTrailScent = 0;
        hasFood = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void decrementTimers() {
        dropScentTimeLeft -= Time.deltaTime;
        randomMoveTimeLeft -= Time.deltaTime;
    }


    void Update(){
        decrementTimers();
        HandleMovement();
    }

    private void stopMovement(){
        rigidbody2d.velocity = Vector2.zero;
    }

    private void HandleMovement() {
        move();
        setDirection();
        depositFood();
        setColour();
    }

    private void setColour() {
        if (hasFood)
        {
            spriteRenderer.color = Color.green;
        }
        else if (hasScent)
        {
            spriteRenderer.color = Color.blue;
        }
        else if (isFollowingTrail) {
            spriteRenderer.color = Color.magenta;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private void depositFood() {
        if (hasFood){
            float distanceToFood = (homeVector - this.transform.position).magnitude;
            if (distanceToFood < distanceToDeposit) {
                stopMovement();
                removeFood();
                hasFood = false;
            }
        }
    }

    private void setMaxSpeed() {
        if (rigidbody2d.velocity.magnitude > maxSpeed)
        {
            rigidbody2d.velocity = rigidbody2d.velocity.normalized * maxSpeed;
        }
    }

    private void removeFood() {
        Destroy(this.transform.GetChild(0).gameObject);
    }

    private void move() {
        rigidbody2d.AddForce(direction * speed);
        setMaxSpeed();

    }

    private void setDirection() {
        if (hasFood)
        {
            setHomeDirection();
            setTrail();
        }
        else if (hasScent)
        {
            setScentDirection();
        }
        else if (isFollowingTrail)
        {
            setTrailDirection();
        }
        else
        {
            setRandomDirection();
        }
    }

    private void setTrail() {
        if (dropScentTimeLeft <= 0 && inTrailScent == 0 ) {
            dropScentTimeLeft = trailDropDelay;
            Transform trail = Instantiate(foodTrail, this.transform.position, Quaternion.identity);
        }
    }

    private void setDirection(Vector3 destination) {
        Vector3 currentPosition = this.transform.position;
        direction = (destination - currentPosition).normalized;
    }

    private void setHomeDirection() {
        setDirection(homeVector);
    }

    private void setRandomDirection() {
        if (randomMoveTimeLeft <= 0)
        {
            randomMoveTimeLeft += Random.Range(changeDirectionMinTime, changeDirectionMaxTime);
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
    }


    private void checkFoodCollision(Collider2D collider) {
        if (collider.CompareTag("Food") && !hasFood && collider.transform.parent.CompareTag("Food"))
        {
            stopMovement();
            collider.transform.SetParent(this.transform);
            Destroy(collider.transform.GetComponent<Collider2D>());
            hasFood = true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        checkSmellTrigger(collision);
        checkFoodCollision(collision);
        checkTrailScent(collision);

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        checkTrailScent(collision, true);
    }

    private void checkTrailScent(Collider2D collision, bool isExit = false) {
        if (collision.CompareTag("FoodTrail")) {
            if (isExit){
                inTrailScent--;
            }
            else {
                isFollowingTrail = true;
                trailFollowing = collision.transform;
                inTrailScent++;
                stopMovement();

            }

            if (inTrailScent == 0) {
                isFollowingTrail = false;
                stopMovement();
            }
        }
    }


    private void setTrailDirection() {
        if (trailFollowing != null && (transform.position.magnitude - trailFollowing.position.magnitude) > trailFollowReleaseDistance)
        {
            setDirection(trailFollowing.position);
        }
        else {
            trailFollowing = null;
            isFollowingTrail = false;
            stopMovement();
        }
    }

    private void checkSmellTrigger(Collider2D collider) {
        if (collider.CompareTag("Smell")  && !hasFood && collider.transform.parent.parent.CompareTag("Food") && !hasScent) {
            hasScent = true;
            foodScent = collider.transform;
        }
    }

    private void setScentDirection() {
        //Needs to not have been deposited or being carried by another ant
        if (foodScent != null && foodScent.transform.parent.parent.CompareTag("Food") ) {
            setDirection(foodScent.position);
        }
        else {
            hasScent = false;
        }
    }

    public void setHome(Vector3 homeVector) {
        this.homeVector = homeVector;
    }


}
