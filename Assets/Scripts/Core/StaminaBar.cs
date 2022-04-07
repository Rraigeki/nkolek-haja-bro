using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;
    private int maxStamina = 100;
    public int currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;


    #region Sigleton
    private static StaminaBar instance;
    public static StaminaBar Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<StaminaBar>();
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    
   public  void UseStamina(int amount)
    {
        if ( currentStamina - amount >= 0 )
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null)
                StopCoroutine(regen);


            regen = StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("Not enough Stamina");
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1);
        
        while (currentStamina <maxStamina)
            {
            if (!ShieldBlock.Instance.ShieldUp)
            {
                currentStamina += maxStamina / 10;
            }
            else
                currentStamina += maxStamina / 100;

            staminaBar.value = currentStamina;
            yield return regenTick;
            }
        regen = null;
        

    }
}
