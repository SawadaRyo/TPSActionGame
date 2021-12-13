using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeManager : MonoBehaviour
{
    [SerializeField] int maxHp = 150;
    [SerializeField] Slider HpSlider;
    [SerializeField] MoveManager moveManager;


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

    // Update is called once per frame
    void Update()
    {

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
        moveManager.living = !moveManager.living;
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
