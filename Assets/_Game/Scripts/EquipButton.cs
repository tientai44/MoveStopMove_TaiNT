using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ChoiceButton
{
    Pant,Head,Shield,Set
}
public class EquipButton : MonoBehaviour
{
    Equipment equipmentInfor;
    MeshRenderer meshRenderer;
    Button button;
    TextMeshProUGUI priceText;
    ChoiceButton choiceButton;
    SkinMenu Menu;
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }
    [SerializeField] RawImage equipImage;
    public Button Button
    {
        get
        {
            if(button == null)
            {
                button=GetComponent<Button>();
            }
            return button;
        }
    }

    public Equipment EquipmentInfor { get => equipmentInfor; set => equipmentInfor = value; }
    public TextMeshProUGUI PriceText { get => priceText; set => priceText = value; }
    internal ChoiceButton ChoiceButton { get => choiceButton; set => choiceButton = value; }
    public RawImage EquipImage { get => equipImage; set => equipImage = value; }

    public void Awake()
    {
        Button.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick()
    {
        if (choiceButton is ChoiceButton.Pant)
        {
            GameController.GetInstance().currentPlayer.SetPant(GameObjectPools.GetInstance().pantMaterials[equipmentInfor.Id - 1]);
            if (SaveLoadManager.GetInstance().Data1.PantOwners.Contains(equipmentInfor.Id))
            {
                Menu.ButtonBuy.SetActive(false);
                Menu.ButtonEquip.SetActive(true);
            }
            else
            {
                Menu.ButtonBuy.SetActive(true);
                Menu.ButtonEquip.SetActive(false);
                Menu.SetPriceText(equipmentInfor.Price);
            }
        }
        if (choiceButton is ChoiceButton.Head)
        {
            GameController.GetInstance().currentPlayer.SetHead(StaticData.HeadEnum[equipmentInfor.Name]);
            if (SaveLoadManager.GetInstance().Data1.HeadOwners.Contains(equipmentInfor.Name))
            {
                Menu.ButtonBuy.SetActive(false);
                Menu.ButtonEquip.SetActive(true);
            }
            else
            {
                Menu.ButtonBuy.SetActive(true);
                Menu.ButtonEquip.SetActive(false);
                Menu.SetPriceText(equipmentInfor.Price);
            }
        }
        if (choiceButton is ChoiceButton.Shield)
        {
            GameController.GetInstance().currentPlayer.SetShield(StaticData.ShieldEnum[equipmentInfor.Name]);
            if (SaveLoadManager.GetInstance().Data1.ShieldOwners.Contains(equipmentInfor.Name))
            {
                Menu.ButtonBuy.SetActive(false);
                Menu.ButtonEquip.SetActive(true);
            }
            else
            {
                Menu.ButtonBuy.SetActive(true);
                Menu.ButtonEquip.SetActive(false);
                Menu.SetPriceText(equipmentInfor.Price);
            }
        }
        if(choiceButton is ChoiceButton.Set)
        {
            GameController.GetInstance().currentPlayer.SetFullSet(StaticData.SetEnum[equipmentInfor.Name]);

            if (SaveLoadManager.GetInstance().Data1.SetOwners.Contains(equipmentInfor.Name))
            {
                Menu.ButtonBuy.SetActive(false);
                Menu.ButtonEquip.SetActive(true);
            }
            else
            {
                Menu.ButtonBuy.SetActive(true);
                Menu.ButtonEquip.SetActive(false);
                Menu.SetPriceText(equipmentInfor.Price);
            }
        }
        GameController.GetInstance().currentPlayer.ChangeAnim(Constant.ANIM_DANCE);
        Menu.CurrentEquipment = equipmentInfor;
    }
    public void SetPriceText(int price)
    {
        priceText.text = price.ToString();
    }
    public void SetSkinMenu(SkinMenu menu )
    {
        Menu = menu;
    }
}
