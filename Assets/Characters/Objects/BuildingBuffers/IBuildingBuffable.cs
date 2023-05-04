using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingBuffable
{
    bool OnBuff(IBuildingBuff buff);
    bool OnDebuff(IBuildingBuff buff);
}
