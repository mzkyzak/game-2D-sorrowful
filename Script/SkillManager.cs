using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    [Header("Energy Settings")]
    public int maxEnergy = 100;
    [HideInInspector] public int currentEnergy;

    [Header("UI")]
    public Slider energySlider;

    [Header("Audio")]
    public AudioSource audioSource; // 🔹 AudioSource untuk mainin SFX

    [System.Serializable]
    public class SkillData
    {
        public string skillName;
        public Button button;
        public TMP_Text cooldownText;
        public int energyCost;
        public float cooldown;

        [Header("Skill Reference")]
        public SkillBase skillScript;

        [Header("SFX")]
        public AudioClip skillSFX; // 🔹 tiap skill punya SFX beda

        [HideInInspector] public bool isCoolingDown = false;
    }

    [Header("Skill List")]
    public SkillData[] skills;

    [Header("Input")]
    public PlayerInput playerInput;
    private InputAction healAction;
    private InputAction buffAction;

    void Awake()
    {
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput == null)
                Debug.LogError("❌ PlayerInput gak ketemu di scene!");
        }

        healAction = playerInput.actions["Heal"];
        buffAction = playerInput.actions["Buff"];
    }

    void OnEnable()
    {
        if (healAction != null)
            healAction.performed += OnHealPressed;
        if (buffAction != null)
            buffAction.performed += OnBuffPressed;
    }

    void OnDisable()
    {
        if (healAction != null)
            healAction.performed -= OnHealPressed;
        if (buffAction != null)
            buffAction.performed -= OnBuffPressed;
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBar();

        foreach (var skill in skills)
        {
            skill.cooldownText.gameObject.SetActive(false);

            SkillBase[] allSkills = FindObjectsOfType<SkillBase>(true);
            foreach (var s in allSkills)
            {
                if (s.GetType().Name == skill.skillName)
                {
                    skill.skillScript = s;
                    s.skillManager = this;
                    break;
                }
            }

            if (skill.skillScript != null)
                skill.button.onClick.AddListener(() => TryUseSkill(skill));
            else
                Debug.LogWarning($"⚠️ Skill '{skill.skillName}' gak ditemukan di scene!");
        }
    }

    private void OnHealPressed(InputAction.CallbackContext ctx)
    {
        TryUseSkillSafe(0);
    }

    public void OnBuffPressed(InputAction.CallbackContext ctx)
    {
        TryUseSkillSafe(1);
    }

    public void TryUseSkillSafe(int index)
    {
        if (skills.Length > index)
            TryUseSkill(skills[index]);
    }

    // ================= Energy =================
    public bool ConsumeEnergy(int cost)
    {
        if (currentEnergy < cost)
        {
            Debug.Log("⚠️ Energi gak cukup!");
            return false;
        }

        currentEnergy -= cost;
        UpdateEnergyBar();
        return true;
    }

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        if (energySlider != null)
            energySlider.value = (float)currentEnergy / maxEnergy;
    }

    // ================= Skill Logic =================
    public void TryUseSkill(SkillData skill)
    {
        if (skill.isCoolingDown)
        {
            Debug.Log($"{skill.skillName} masih cooldown!");
            return;
        }

        if (!ConsumeEnergy(skill.energyCost)) return;

        // 🔹 Mainin SFX skill
        if (skill.skillSFX != null && audioSource != null)
            audioSource.PlayOneShot(skill.skillSFX);

        skill.skillScript.Activate();
        StartCoroutine(CooldownRoutine(skill));
    }

    private IEnumerator CooldownRoutine(SkillData skill)
    {
        skill.isCoolingDown = true;
        skill.button.interactable = false;
        skill.cooldownText.gameObject.SetActive(true);

        float timer = skill.cooldown;

        while (timer > 0)
        {
            skill.cooldownText.text = Mathf.Ceil(timer).ToString("0");
            timer -= Time.deltaTime;
            yield return null;
        }

        skill.cooldownText.gameObject.SetActive(false);
        skill.button.interactable = true;
        skill.isCoolingDown = false;
    }
}
