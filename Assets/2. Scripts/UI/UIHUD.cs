using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHUD : SceneOnlySingleton<UIHUD>
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image staminaBar;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        PlayerController.Instance.StatManager.playerStat[StatType.CurrentHp].OnValueChanged += (curValue)
            =>
        {
            UpdateHpUI(curValue, PlayerController.Instance.StatManager.GetValue(StatType.MaxHp));
        };

        PlayerController.Instance.StatManager.playerStat[StatType.CurrentStamina].OnValueChanged += (curValue)
            =>
        {
            UpdateStaminaUI(curValue, PlayerController.Instance.StatManager.GetValue(StatType.MaxStamina));
        };
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateHpUI(float current, float max)
    {
        hpBar.DOKill();
        hpBar.DOFillAmount(current / max, 0.3f);
    }

    private void UpdateStaminaUI(float current, float max)
    {
        staminaBar.DOKill();
        staminaBar.DOFillAmount(current / max, 0.3f);
    }
}