using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] int maxSp = 100;
    [SerializeField] int rSp = 1;
    [SerializeField] Slider SpSlider;

    GameObject myWeapon;
    WeaponStatas weaponStatas;
    int sp;
    float time;
    bool relodeFlg;
    bool attackFlg;
    public bool AttackFlg { get => attackFlg; set => attackFlg = value; }

    // Start is called before the first frame update
    void Start()
    {
        SpSlider.value = maxSp;
        sp = maxSp;
        myWeapon = GameObject.FindGameObjectWithTag("MyWeapon");
        weaponStatas = myWeapon.GetComponent<WeaponStatas>();
        relodeFlg = true;
    }

    // Update is called once per frame
    void Update()
    {
        staminaContorller();
        SpSlider.value = (float)sp / (float)maxSp;
    }

    void staminaContorller()
    {
        if (sp > 0) AttackFlg = true;
        else if (sp <= 0) AttackFlg = false;
        time += Time.deltaTime;

        if (sp < maxSp)
        {
            if (time >= 0.1f && relodeFlg == true)
            {
                sp += rSp;
                time = 0f;
            }
            //StartCoroutine("AttackReturn");
        }
    }
    //IEnumerator AttackReturn()
    //{
    //    if (!relodeFlg) yield break;
    //    relodeFlg = false;
    //    yield return new WaitForSeconds(1f);
    //    relodeFlg = true;
    //}
    void AttackOn()
    {
        sp -= weaponStatas.DPS;
        weaponStatas.capsuleCollider.enabled = true;
    }
    void AttackOff()
    {
        weaponStatas.capsuleCollider.enabled = false;
    }
}
