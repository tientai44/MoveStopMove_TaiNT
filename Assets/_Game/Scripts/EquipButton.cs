using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ChoiceButton
{
     Head=0,Pant=1, Shield=2,Set=3
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
    RawImage rawImage;
    [SerializeField] RawImage equipImage;
    [SerializeField] GameObject Lock;
    [SerializeField] GameObject UnLock;
    public Button Button
    {
        get
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            return button;
        }
    }

    public Equipment EquipmentInfor { get => equipmentInfor; set => equipmentInfor = value; }
    public TextMeshProUGUI PriceText { get => priceText; set => priceText = value; }
    internal ChoiceButton ChoiceButton { get => choiceButton; set => choiceButton = value; }
    public RawImage EquipImage { get => equipImage; set => equipImage = value; }
    public RawImage RawImage { get => rawImage; set => rawImage = value; }

    public void Awake()
    {
        rawImage = GetComponent<RawImage>();
        Button.onClick.AddListener(TaskOnClick);
        

    }
    public void TaskOnClick()
    {
        if(Menu.CurrentButton!=null)
            Menu.CurrentButton.RawImage.color = Color.gray;
        Menu.CurrentButton = this;
        rawImage.color = Color.white;
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
        if (choiceButton is ChoiceButton.Set)
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
    public void SetSkinMenu(SkinMenu menu)
    {
        Menu = menu;
    }
    public void CheckLock()
    {
        if (choiceButton is ChoiceButton.Pant)
        {
            GameController.GetInstance().currentPlayer.SetPant(GameObjectPools.GetInstance().pantMaterials[equipmentInfor.Id - 1]);
            if (SaveLoadManager.GetInstance().Data1.PantOwners.Contains(equipmentInfor.Id))
            {
                SetUnlock();
            }
            else
            {
                SetLock();
            }
        }
        if (choiceButton is ChoiceButton.Head)
        {
            GameController.GetInstance().currentPlayer.SetHead(StaticData.HeadEnum[equipmentInfor.Name]);
            if (SaveLoadManager.GetInstance().Data1.HeadOwners.Contains(equipmentInfor.Name))
            {
                SetUnlock();
            }
            else
            {
                SetLock();
            }
        }
        if (choiceButton is ChoiceButton.Shield)
        {
            GameController.GetInstance().currentPlayer.SetShield(StaticData.ShieldEnum[equipmentInfor.Name]);
            if (SaveLoadManager.GetInstance().Data1.ShieldOwners.Contains(equipmentInfor.Name))
            {
                SetUnlock();
            }
            else
            {
                SetLock();
            }
        }
        if (choiceButton is ChoiceButton.Set)
        {
            GameController.GetInstance().currentPlayer.SetFullSet(StaticData.SetEnum[equipmentInfor.Name]);

            if (SaveLoadManager.GetInstance().Data1.SetOwners.Contains(equipmentInfor.Name))
            {
                SetUnlock();
            }
            else
            {
                SetLock();
            }
        }
    } 
    public void SetUnlock()
    {
        Lock.SetActive(false);
        UnLock.SetActive(true);
    }
    public void SetLock()
    {
        Lock.SetActive(true);
        UnLock.SetActive(false);
    }
}
