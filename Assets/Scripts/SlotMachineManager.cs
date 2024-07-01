using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public int balance;
    public Text balanceText;
    public Text betText;
    public int currentBet;
    public SlotController[] slots;
    public Combinations[] combinations;
    public float timeInterval = 0.025f;
    private int stoppedSlots = 3;
    private int maxBet = 1000;
    private bool isSpin = false;
    public bool isAuto;

    public Animator btnAnim;
    public AudioSource slotMachineAudio;
    public AudioClip spinSound;
    public AudioClip winSound;

    private const string BALANCE_PREF_KEY = "Balance";

    private void Start()
    {
        btnAnim = GameObject.FindGameObjectWithTag("AutoButton").GetComponent<Animator>();
        balance = PlayerPrefs.GetInt(BALANCE_PREF_KEY, balance);

        UpdateBalance(0);
        UpdateBet(currentBet);
    }

    public void Spin()
    {
        if (!isSpin && balance - currentBet >= 0)
        {
            UpdateBalance(-currentBet);
            isSpin = true;
            slotMachineAudio.PlayOneShot(spinSound);

            foreach (SlotController i in slots)
            {
                i.StartCoroutine("Spin");
            }
        }
    }

    public void WaitResults()
    {
        stoppedSlots -= 1;
        if(stoppedSlots <= 0)
        {
            stoppedSlots = 3;
            slotMachineAudio.Stop();
            CheckResults();
        }
    }

    public void CheckResults()
    {
        isSpin = false;

        foreach (Combinations i in combinations)
        {
            if (slots[0].gameObject.GetComponent<SlotController>().stoppedSlot.ToString() == i.FirstValue.ToString()
                && slots[1].gameObject.GetComponent<SlotController>().stoppedSlot.ToString() == i.SecondValue.ToString()
                && slots[2].gameObject.GetComponent<SlotController>().stoppedSlot.ToString() == i.ThirdValue.ToString())
            {
                UpdateBalance(i.prizeMultiplier * currentBet);
                slotMachineAudio.PlayOneShot(winSound, 1f);
            }
        }
        if (isAuto)
        {
            Invoke(nameof(Spin), 0.4f);
        }
    }

    private void UpdateBalance(int count)
    {
        balance += count;
        balanceText.text = "Balance: " + balance;
        PlayerPrefs.SetInt(BALANCE_PREF_KEY, balance);
    }

    public void UpdateBet(int amount)
    {
        if 
        ( 
            isSpin
            || currentBet + amount <= 0
            || balance - (currentBet + amount) < 0
            || currentBet + amount > maxBet
        ) return;

        currentBet += amount;
        betText.text = "Bet: " + currentBet;
    }

    public void SetAuto()
    {
        if (!isAuto)
        {
            timeInterval = timeInterval / 10;
            isAuto = true;
            btnAnim.SetBool("isAuto", true);
            Spin();
        }
        else
        {
            timeInterval = timeInterval * 10;
            isAuto = false;
            btnAnim.SetBool("isAuto", false);
        }
    }
}

[System.Serializable]
public class Combinations
{
    public enum SlotValue
    {
        Crown,
        Diamond,
        Seven,
        Cherry,
        Bar
    }

    public SlotValue FirstValue;
    public SlotValue SecondValue;
    public SlotValue ThirdValue;
    public int prizeMultiplier;
}
