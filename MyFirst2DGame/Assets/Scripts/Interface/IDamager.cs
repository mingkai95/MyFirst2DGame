using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager
{
    void InflictDamage(IDamageable target);
}
