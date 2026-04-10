using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector] public SkillManager skillManager;
    public abstract void Activate();
}
