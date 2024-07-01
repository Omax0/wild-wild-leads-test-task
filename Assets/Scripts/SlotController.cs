using System.Collections;
using UnityEngine;

public enum SlotValue
{
    Crown,
    Diamond,
    Seven,
    Cherry,
    Bar
}
public class SlotController : MonoBehaviour
{

    private int randomValue;
    [HideInInspector]public float timeInterval;
    private float startSpeed;
    private float speed;
    public SlotValue stoppedSlot;
    private SlotMachine sm;

    private const float CROWN_Y_POS = -2.5f;
    private const float DIAMOND_Y_POS = -1.55f;
    private const float SEVEN_Y_POS = -0.6f;
    private const float CHERRY_Y_POS = 0.4f;
    private const float BAR_Y_POS = 1.35f;
    private const float END_SLOT_Y_POS = 2.17f;

    private void Start()
    {
        sm = gameObject.GetComponentInParent<SlotMachine>();
    }
    public IEnumerator Spin()
    {
        timeInterval = sm.timeInterval;
        randomValue = Random.Range(0, 90);
        speed = 30f + randomValue;
        while(speed >= 10f)
        {
            speed = speed / 1.01f;
            transform.Translate(Vector2.up * Time.deltaTime * -speed);
            if (transform.localPosition.y <= CROWN_Y_POS)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, END_SLOT_Y_POS);
            }

            yield return new WaitForSeconds(timeInterval);
        }
        StartCoroutine("EndSpin");
        yield return null;
    }
    private IEnumerator EndSpin()
    {
        while (speed >= 2f)
        {
            if (transform.localPosition.y < DIAMOND_Y_POS)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,
                    new Vector2(transform.localPosition.x, CROWN_Y_POS), speed * Time.deltaTime);
                if (new Vector2(transform.localPosition.x, transform.localPosition.y) == new Vector2(transform.localPosition.x, CROWN_Y_POS))
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < SEVEN_Y_POS)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,
                    new Vector2(transform.localPosition.x, DIAMOND_Y_POS), speed * Time.deltaTime);
                if (new Vector2(transform.localPosition.x, transform.localPosition.y) == new Vector2(transform.localPosition.x, DIAMOND_Y_POS))
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < CHERRY_Y_POS)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,
                    new Vector2(transform.localPosition.x, SEVEN_Y_POS), speed * Time.deltaTime);
                if (new Vector2(transform.localPosition.x, transform.localPosition.y) == new Vector2(transform.localPosition.x, SEVEN_Y_POS))
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < BAR_Y_POS)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,
                    new Vector2(transform.localPosition.x, CHERRY_Y_POS), speed * Time.deltaTime);
                if (new Vector2(transform.localPosition.x, transform.localPosition.y) == new Vector2(transform.localPosition.x, CHERRY_Y_POS))
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < END_SLOT_Y_POS)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,
                    new Vector2(transform.localPosition.x, BAR_Y_POS), speed * Time.deltaTime);
                if (new Vector2(transform.localPosition.x, transform.localPosition.y) == new Vector2(transform.localPosition.x, BAR_Y_POS))
                {
                    speed = 0;
                }
            }
            speed = speed / 1.01f;
            yield return new WaitForSeconds(timeInterval);
        }
        speed = 0;
        CheckResults();
        yield return null;
    }
    private void CheckResults()
    {
        if (transform.localPosition.y == CROWN_Y_POS)
        {
            stoppedSlot = SlotValue.Crown;
        }
        else if (transform.localPosition.y == DIAMOND_Y_POS)
        {
            stoppedSlot = SlotValue.Diamond;
        }
        else if (transform.localPosition.y == SEVEN_Y_POS)
        {
            stoppedSlot = SlotValue.Seven;
        }
        else if (transform.localPosition.y == CHERRY_Y_POS)
        {
            stoppedSlot = SlotValue.Cherry;
        }
        else if (transform.localPosition.y == BAR_Y_POS)
        {
            stoppedSlot = SlotValue.Bar;
        }

        sm.WaitResults();
    }
}
