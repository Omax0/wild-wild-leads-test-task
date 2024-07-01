using UnityEngine;

public class ScreenAdapter : MonoBehaviour
{
    [SerializeField] Transform slotMachine;

    private void Update()
    {
        if (((float) Screen.width / Screen.height) >= 0.7f)
        {
            slotMachine.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            return;
        }

        slotMachine.localScale = Vector3.one;
    }
}
