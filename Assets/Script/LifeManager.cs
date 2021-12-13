using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeManager : MonoBehaviour
{
    [SerializeField] int maxHp = 150;
    [SerializeField] Slider HpSlider;
    [SerializeField] string judSubject = "";


    GameObject enemyWeapon;
    WeaponStatas enemyweaponStatas;
    Animator animator;
    int hp;
    bool damageFlg;

    

    void Start()
    {
        HpSlider.value = maxHp;
        hp = maxHp;
        animator = GetComponent<Animator>();
        enemyWeapon = GameObject.Find("EnemyWeapon");
        damageFlg = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            Damage();
        }
    }
    void DeathOrLive()
    {
        if(judSubject == "Player")
        {
            MoveManager moveManager = GetComponent<MoveManager>();
            moveManager.living = !moveManager.living;
        }
        else if(judSubject == "Enemy")
        {
            EnemyActionBase enemyLife = GetComponent<EnemyActionBase>();
        }
    }
    
    void Damage()
    {
        if (damageFlg) return;
        enemyweaponStatas = enemyWeapon.GetComponent<WeaponStatas>();
        int damage = enemyweaponStatas.MyWeaponDamageCalculation();
        hp -= damage;
        HpSlider.value = (float)hp / (float)maxHp;
        Debug.Log(hp);
        if (hp > 0) StartCoroutine("DamageTimer");
        else if (hp <= 0)
        {
            animator.SetTrigger("Death");
            DeathOrLive();
        }
    }
    IEnumerator DamageTimer()
    {
        if (damageFlg) yield break;
        damageFlg = true;
        yield return new WaitForSeconds(0.5f);
        damageFlg = false;
    }
}
