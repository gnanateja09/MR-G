using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempKillwe : MonoBehaviour
{
    // Start is called before the first frame update
    public void TempKill()
    {
        EnemyShooter[] shooters = FindObjectsOfType<EnemyShooter>();
        foreach (EnemyShooter shooter in shooters)
        {
            if (shooter != null)
            {
                shooter.Die();
            }
        }
    }
}
