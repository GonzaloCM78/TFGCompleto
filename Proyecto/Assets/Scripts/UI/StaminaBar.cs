using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    public Slider staminaSlider;

    public float maxStamina = 100;

    private float currentStamina;

    private float regenerateStaminaTine = 0.1f;
    private float regenerateAmount = 2;

    private float losingStaminaTime = 0.1f;

    private Coroutine myCouroutineLosing;

    private Coroutine myCouroutineRegenerate;



    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }


    public void UseStamina(float amount)
    {
        if(currentStamina-amount > 0)
        {
            if(myCouroutineLosing != null)
            {
                StopCoroutine(myCouroutineLosing);
            }

            myCouroutineLosing = StartCoroutine(LosingStaminaCoroutine(amount));

            if(myCouroutineRegenerate != null)
            {
                StopCoroutine(myCouroutineRegenerate);
            }

            myCouroutineRegenerate = StartCoroutine(RegenerateStaminaCoroutine());

        } else
        {
            Debug.Log("No tengo estamina");
            FindObjectOfType<PlayerMovement>().isSprinting = false;
        }
    }


    private IEnumerator LosingStaminaCoroutine(float amount)
    {
        while(currentStamina >= 0)
        {
            currentStamina-= amount;

            staminaSlider.value = currentStamina;

            yield return new WaitForSeconds(losingStaminaTime);
        }
        myCouroutineLosing = null;

        FindObjectOfType<PlayerMovement>().isSprinting = false;

    }

    private IEnumerator RegenerateStaminaCoroutine()
    {
        yield return new WaitForSeconds(1);

        while (currentStamina < maxStamina)
        {
            currentStamina += regenerateAmount;

            staminaSlider.value = currentStamina;

            yield return new WaitForSeconds(regenerateStaminaTine);

        }

        myCouroutineRegenerate = null;




    }

}
