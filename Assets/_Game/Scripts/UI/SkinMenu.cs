using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class SkinMenu : UICanvas
{
    [SerializeField] private Transform scrollView;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject equipButtonPrefabs;
    [SerializeField] List<Texture2D> headTextures;
    [SerializeField] List<Texture2D> pantTextures;
    [SerializeField] List<Texture2D> skinTextures;
    [SerializeField] List<Texture2D> armSkinTextures;
    List<EquipButton> equipButtons = new List<EquipButton>();
    [SerializeField] GameObject buttonBuy;
    [SerializeField] GameObject buttonEquip;
    [SerializeField] List<Image> buttonImages;
    [SerializeField] TextMeshProUGUI equipText;
    ChoiceButton choiceButton;
    Equipment currentEquipment;
    EquipButton currentButton;
    EquipButton prevEquipButton;
    public Equipment CurrentEquipment { get => currentEquipment; set => currentEquipment = value; }
    public GameObject ButtonBuy { get => buttonBuy; set => buttonBuy = value; }
    public GameObject ButtonEquip { get => buttonEquip; set => buttonEquip = value; }
    public EquipButton CurrentButton { get => currentButton; set => currentButton = value; }
    public EquipButton PrevEquipButton { get => prevEquipButton; set => prevEquipButton = value; }

    public override void Open()
    {
        base.Open();
        SetCoinText(SaveLoadManager.GetInstance().Data1.Coin);
        CameraFollow.GetInstance().ZoomIn();

        ButtonBuy.SetActive(false);
        ButtonEquip.SetActive(false);
    }
    public void SetOffPrevButton()
    {
        if (choiceButton is ChoiceButton.Head)
        {
            buttonImages[0].color = Color.black;
        }
        else if (choiceButton is ChoiceButton.Pant)
        {
            buttonImages[1].color = Color.black;
        }
        else if (choiceButton is ChoiceButton.Shield)
        {
            buttonImages[2].color = Color.black;
        }
        if (choiceButton is ChoiceButton.Set)
        {
            buttonImages[3].color = Color.black;
        }


        //TODO: Advantage
        int i = choiceButton switch
        {
            ChoiceButton.Head => 0,
            ChoiceButton.Pant => 1,
            ChoiceButton.Shield => 2,
            ChoiceButton.Set => 3,
            _ => 0,
        };
        buttonImages[i].color = Color.black;
    }
    public void SetOnCurrentButton()
    {
        if (choiceButton is ChoiceButton.Head)
        {
            buttonImages[0].color = Color.black;
        }
        else if (choiceButton is ChoiceButton.Pant)
        {
            buttonImages[1].color = Color.black;
        }
        else if (choiceButton is ChoiceButton.Shield)
        {
            buttonImages[2].color = Color.black;
        }
        if (choiceButton is ChoiceButton.Set)
        {
            buttonImages[3].color = Color.black;
        }
    }
    public void HeadButton()
    {
        SetOffPrevButton();
        SetUpButton(headTextures, StaticData.headEquipments, ChoiceButton.Head);
        choiceButton = ChoiceButton.Head;
        buttonImages[0].color = Color.gray;
    }

    public void PantsButton()
    {
        SetOffPrevButton();
        SetUpButton(pantTextures, StaticData.pantEquipments, ChoiceButton.Pant);
        choiceButton = ChoiceButton.Pant;
        buttonImages[1].color = Color.gray;

    }

    public void ArmSkinButton()
    {
        SetOffPrevButton();
        SetUpButton(armSkinTextures, StaticData.shieldEquipments, ChoiceButton.Shield);
        choiceButton = ChoiceButton.Shield;
        buttonImages[2].color = Color.gray;
    }
    public void SkinButton()
    {
        SetOffPrevButton();
        SetUpButton(skinTextures, StaticData.setEquipments, ChoiceButton.Set);
        choiceButton = ChoiceButton.Set;
        buttonImages[3].color = Color.gray;

    }
    public void BuyButton()
    {
        int currentCoin = SaveLoadManager.GetInstance().Data1.Coin;
        int price = currentEquipment.Price;
        if (currentCoin >= price)
        {
            currentCoin -= price;
            SetCoinText(currentCoin);
            SaveLoadManager.GetInstance().Data1.Coin = currentCoin;
            if (choiceButton is ChoiceButton.Pant)
            {
                SaveLoadManager.GetInstance().Data1.PantOwners.Add(currentEquipment.Id);
            }
            if (choiceButton is ChoiceButton.Head)
            {
                SaveLoadManager.GetInstance().Data1.HeadOwners.Add(currentEquipment.Name);
            }
            if (choiceButton is ChoiceButton.Shield)
            {
                SaveLoadManager.GetInstance().Data1.ShieldOwners.Add(currentEquipment.Name);

            }
            if (choiceButton is ChoiceButton.Set)
            {
                SaveLoadManager.GetInstance().Data1.SetOwners.Add(currentEquipment.Name);
            }
            buttonBuy.SetActive(false);
            buttonEquip.SetActive(true);
            SetEquipText(Constant.EQUIP_STRING);
            currentButton.SetUnlock();
            SaveLoadManager.GetInstance().Save();
        }
        else
        {
            return;
        }

    }
    public void EquipButton()
    {
        if (choiceButton is ChoiceButton.Pant)
        {
            SaveLoadManager.GetInstance().Data1.IdPantMaterialCurrent = currentEquipment.Id;

        }
        if (choiceButton is ChoiceButton.Head)
        {
            SaveLoadManager.GetInstance().Data1.HeadCurrent = currentEquipment.Name;
        }
        if (choiceButton is ChoiceButton.Shield)
        {
            SaveLoadManager.GetInstance().Data1.ShieldCurent = currentEquipment.Name;
        }
        if (choiceButton is ChoiceButton.Set)
        {
            SaveLoadManager.GetInstance().Data1.IdPantMaterialCurrent = currentEquipment.IdPant;
            SaveLoadManager.GetInstance().Data1.HeadCurrent = currentEquipment.HeadName;
            SaveLoadManager.GetInstance().Data1.ShieldCurent = currentEquipment.ShieldName;
            SaveLoadManager.GetInstance().Data1.SetCurrent = currentEquipment.Name;
        }
        SetEquipText(Constant.EQUIPED_STRING);
        currentButton.CheckImage.SetActive(true);
        prevEquipButton.CheckImage.SetActive(false);
        prevEquipButton = currentButton;
        SaveLoadManager.GetInstance().Save();
    }

    public void ReturnButton()
    {
        ClearButton();
        SetOffPrevButton();
        NewUIManager.GetInstance().OpenUI<MainMenu>();
        Close(0.5f);
    }
  
    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void SetPriceText(int price)
    {
        priceText.text = price.ToString();
    }
    public void SetEquipText(string text)
    {
        equipText.text =text;
    }
    public void ClearButton()
    {
        //while (equipButtons.Count != 0)
        //{
        //    Destroy(equipButtons[0].gameObject);
        //    equipButtons.RemoveAt(0);

        //}
        for(int i = 0; i < equipButtons.Count; i++)
        {
            equipButtons[i].OnInit();
            equipButtons[i].gameObject.SetActive(false);
        }
    }
    public void SetUpButton(List<Texture2D> textures, List<Equipment> equipments, ChoiceButton choiceButton)
    {
        ButtonBuy.SetActive(false);
        ButtonEquip.SetActive(false);
        ClearButton();
        
        for (int i = 0; i < textures.Count; i++)
        {
            //equipButtonPrefabs.GetComponent<RawImage>().texture = textures[i];
            if (i >= equipButtons.Count)
            {
                EquipButton button = Instantiate(equipButtonPrefabs, scrollView).GetComponent<EquipButton>();
                button.EquipImage.texture = textures[i];
                button.SetSkinMenu(this);
                equipButtons.Add(button);
                equipButtons[i].EquipmentInfor = equipments[i];
                equipButtons[i].PriceText = priceText;
                equipButtons[i].ChoiceButton = choiceButton;
                equipButtons[i].CheckLock();
            }
            else
            {
                equipButtons[i].EquipImage.texture = textures[i];
                equipButtons[i].SetSkinMenu(this);
                equipButtons[i].EquipmentInfor=equipments[i];
                equipButtons[i].PriceText = priceText;
                equipButtons[i].ChoiceButton=choiceButton;
                equipButtons[i].CheckLock();
                equipButtons[i].gameObject.SetActive(true);
            }
        }
    }
    public override void Close(float delayTime)
    {
        GameController.GetInstance().currentPlayer.OnInit();
        CameraFollow.GetInstance().ZoomOut();
        base.Close(delayTime);
    }
}
