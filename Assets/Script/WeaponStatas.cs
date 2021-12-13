using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class WeaponStatas : MonoBehaviour
{
    [SerializeField] int attackPower = 100;
    [SerializeField] int attackPowerFire = 0;
    [SerializeField] int attackPowerThunder = 0;
    [SerializeField] int attackPowerSorcery = 0;
    [SerializeField] int attackPowerDark = 0;
    [SerializeField] int attackPowerWater = 0;
    [SerializeField] int dsp = 15;
    [SerializeField] int weaponLevel = 1;

    //int needValueOfMussle = 15;
    //int needValueOfTecnic = 12;
    //int maxWeaponLevel = 10;
    public CapsuleCollider capsuleCollider;

    private void Start()
    {
        capsuleCollider.enabled = false; 
    }

    public int AttackPower { get => attackPower;  set => attackPower = value;  }
    public int AttackPowerFire { get => attackPowerFire;  set => attackPower = value;  }
    public int AttackPowerThunder { get => attackPowerThunder;  set => attackPower = value;  }
    public int AttackPowerSorcery { get => attackPowerSorcery;  set => attackPower = value;  }
    public int AttackPowerDark { get => attackPowerDark;  set => attackPower = value;  }
    public int AttackPowerWater { get => attackPowerWater;  set => attackPower = value;  }
    public int DPS { get => dsp;  set => dsp = value;  }
    public int WeaponLevel { get => weaponLevel; set => weaponLevel = value; }

    public int MyWeaponDamageCalculation()
    {
        float[] allPower = new float[5];
        allPower[0] = AttackPower;
        allPower[1] = AttackPowerDark;
        allPower[2] = AttackPowerFire;
        allPower[3] = AttackPowerSorcery;
        allPower[4] = AttackPowerThunder;
        float sumPower = 0;
        float weaponLevel = WeaponLevel;
        for (int i = 0; i < 5; i++)
        {
            sumPower += allPower[i] * (weaponLevel * 1.1f);
        }

        return (int)sumPower;
    }
}
