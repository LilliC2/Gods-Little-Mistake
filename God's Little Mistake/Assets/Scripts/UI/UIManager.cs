using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{

    [Header("Player Feedback")]
    public TMP_Text hpText;
    public TMP_Text levelText;

    [Header("Equip")]
    public Sprite defaultCursor;
    public Image cursor;
    public Sprite cursorClick;
    public GameObject canvas;
    public GameObject playerCanvas;
    public Item heldItem = null;
    bool canEquip;
    float rotation;
    public bool isHoldingItem;

    [Header("Inventory Images")]
    public Sprite emptySlotSprite;
    public Image invenSlot0;
    public Image invenSlot1;
    public Image invenSlot2;
    public Image invenSlot3;
    public Image invenSlot4;
    public Image invenSlot5;
    public Image invenSlot6;

    [Header("Missy Height")]
    public GameObject playerAvatar;
    Vector3 standard = new Vector3(0.239999443f, 0.0800049901f, -0.555116713f);
    Vector3 tall = new Vector3(-0.0209999997f, -0.136000007f, 0.0540000014f);

    [Header("Item Hover Over Panel")]
    public GameObject statsPopUpPanel;
    public TMP_Text popupName;
    public TMP_Text popupDmg;
    public TMP_Text popupCritX;
    public TMP_Text popupCritChance;
    public TMP_Text popupFirerate;

    [Header("Global Scroll UI")]
    public RectTransform scrollContent;
    public float scrollSpeed = 10f;


    [Header("Inventory Pop up")]
    public GameObject invenSegementPopUpPanel;
    public TMP_Text invenPopupName;
    public TMP_Text invenPopupDmg;
    public TMP_Text invenPopupCritX;
    public TMP_Text invenPopupCritChance;
    public TMP_Text invenPopupFirerate;
    public GameObject topEye;
    public GameObject middleEye;
    public GameObject bottomEye;
    public GameObject attackPill;
    public TMP_Text attackPillText;
    public GameObject attackIcon;
    public GameObject rangePill;
    public TMP_Text rangePillText;
    public GameObject rangeIcon;
    public GameObject itemIcon;
    public GameObject typeIcon;

    [Header("Inventory Comparison1")]
    public GameObject statComp1;
    public GameObject arrowComp;
    public TMP_Text popupName1;
    public TMP_Text popupDmg1;
    public TMP_Text popupCritX1;
    public TMP_Text popupCritChance1;
    public TMP_Text popupFirerate1;

    [Header("Animation")]
    public Animator anim;
    public Animator anim1;

    [Header("Pause")]
    public GameObject pausePanel;
    public GameObject pauseFunctionalityPanel;
    public GameObject optionPanel;


    //[Header("Inventory Comparison2")]
    //public GameObject statComp2;
    //public TMP_Text popupName2;
    //public TMP_Text popupDmg2;
    //public TMP_Text popupCritX2;
    //public TMP_Text popupCritChance2;
    //public TMP_Text popupFirerate2;



    private void Start()
    {   
        UpdateInventorySlotImages();
        heldItem = null;
        isHoldingItem = false;
        statsPopUpPanel.SetActive(false);
        statComp1.SetActive(false);
        arrowComp.SetActive(false);
        playerAvatar = GameObject.FindGameObjectWithTag("Player");

        anim1 = statComp1.GetComponent<Animator>();

        //Pause Related
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);

    }

    private void Update()
    {
        if(heldItem == null)
        {
            if (Input.GetKey(KeyCode.Mouse0)) cursor.sprite = cursorClick;
            else cursor.sprite = defaultCursor;
        }

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            Vector3 newPosition = scrollContent.localPosition + Vector3.up * scrollDelta * scrollSpeed;
            scrollContent.localPosition = newPosition;
        }


    }

    #region Pause
    public void OnPause()
    {
        pausePanel.SetActive(true);
    }

    public void OnResume()
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void OptionsOpen()
    {
        optionPanel.SetActive(true);
        pauseFunctionalityPanel.SetActive(false);
        //change to options panel here
    }

    public void OptionsClose()
    {
        optionPanel.SetActive(false);
        pauseFunctionalityPanel.SetActive(true);
        //change to options panel here
    }

    public void OnExit()
    {
        pausePanel.SetActive(false); //temporary
        //Turn on death panel
        //Show score, time, etc.
    }

    


    #endregion

    public void UpdateItemPopUp(Item _hoverItem)
    {
        //ADD LATER FORMATTING FOR FLOATS

        popupName.text = _hoverItem.itemName;
        popupDmg.text = _hoverItem.dmg.ToString();
        popupCritX.text = _hoverItem.critX.ToString();
        popupCritChance.text = _hoverItem.critChance.ToString();
        popupFirerate.text = _hoverItem.fireRate.ToString();

        var matchItem = SearchForItemMatch(_hoverItem);

        if (matchItem != null)
            print(matchItem[0]);
        else print("no match");
    }

    public void UpdateItemPopUpComp1(Item _hoverItem)
    {
        //ADD LATER FORMATTING FOR FLOATS

        popupName1.text = _hoverItem.itemName;
        popupDmg1.text = _hoverItem.dmg.ToString();
        popupCritX1.text = _hoverItem.critX.ToString();
        popupCritChance1.text = _hoverItem.critChance.ToString();
        popupFirerate1.text = _hoverItem.fireRate.ToString();

        var matchItem = SearchForItemMatch(_hoverItem);

        if (matchItem != null)
            print(matchItem[0]);
        else print("no match");
    }

    public void UpdateHealthText(float _hp)
    {
        hpText.text = _hp.ToString("F0"); //removes any decimals
    }
    public void UpdateLevelext(int _lvl)
    {
        levelText.text = _lvl.ToString();
    }

    #region Inventory Item Stats Popup

    public void InvenSegmentPopUp()
    {
        var rt = invenSegementPopUpPanel.GetComponent<RectTransform>();
        //print(rt.anchoredPosition);
        rt.anchoredPosition = new Vector2(522.75f, -400.00f);
        //headSegementPopUpPanel.transform.DOMove(new Vector3(234.489014f, -343f, 0), 1); //COULD ADD EASE HERE;
    }
    public void InvenSegmentPopDown()
    {
        var rt = invenSegementPopUpPanel.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(522.75f, -561.85f);
    }

    public void UpdateInventorySlotImages()
    {
        for (int i = 0; 7 > i; i++)
        {
            switch (i)
            {
                case 0:
                    if (!(i >= -1 && i < _PC.playerInventory.Count))
                    {
                        invenSlot0.sprite = emptySlotSprite;
                    }
                    else invenSlot0.sprite = _PC.playerInventory[i].icon; //images for icon
                    break;

                case 1:
                    if (!(i >= -1 && i < _PC.playerInventory.Count))
                    {
                        invenSlot1.sprite = emptySlotSprite;
                    }
                    else
                    {
                        invenSlot1.sprite = _PC.playerInventory[i].icon; //images for icon
                        
                    }
                    break;

                case 2:
                    if (!(i >= -1 && i < _PC.playerInventory.Count))
                    {
                        invenSlot2.sprite = emptySlotSprite;
                    }
                    else
                    {
                        invenSlot2.sprite = _PC.playerInventory[i].icon; //images for icon
                    }
                    break;

                case 3:
                    if (!(i >= -1 && i < _PC.playerInventory.Count))
                    {
                        invenSlot3.sprite = emptySlotSprite;
                    }
                    else invenSlot3.sprite = _PC.playerInventory[i].icon; 
                    break;

                case 4:
                    if (!(i >= -1 && i < _PC.playerInventory.Count))
                    {
                        invenSlot4.sprite = emptySlotSprite;
                    }
                    else invenSlot4.sprite = _PC.playerInventory[i].icon; 
                    break;

                case 5:
                    if (!(i >= -1 && i < _PC.playerInventory.Count))
                    {
                        invenSlot5.sprite = emptySlotSprite;
                    }
                    else invenSlot5.sprite = _PC.playerInventory[i].icon; 
                    break;

            }


        }


    }

    public void InventorySlotHover(int _whichSlot)
    {
        invenPopupName.text = _PC.playerInventory[_whichSlot].itemName;
        invenPopupDmg.text = "Dmg: " + _PC.playerInventory[_whichSlot].dmg.ToString();
        invenPopupCritX.text = "CritX: " + _PC.playerInventory[_whichSlot].critX.ToString();
        invenPopupCritChance.text = "Crit%: " + _PC.playerInventory[_whichSlot].critChance.ToString();
        invenPopupFirerate.text = "Firerate%: " + _PC.playerInventory[_whichSlot].fireRate.ToString();

        
    }

    #endregion

    #region Item Equip
    /// <summary>
    /// Changes cursor to most recently picked up item
    /// </summary>
    /// <param name="_slot"></param>
    public void CreateItemSelected(Item _itemInfo)
    {

        //print("Create item, in scene id is " + _inSceneId);
        heldItem = _itemInfo;

        //print("in CreateItemSelected ID is " + heldItem.ID);

        //Sprite itemSprite = GameObject.Instantiate(_ISitemD.inSceneItemDataBase[_inSceneId].icon, canvas.transform);
        //cursor.sprite = itemSprite;

        //statsPopUpPanel.SetActive(false);
        isHoldingItem = true;

        int slot = 0;

        //find segement
        if (heldItem.segment == Item.Segment.Head)
        {
            //find category
            if (heldItem.category == Item.Category.Horns)
            {
                //check if slot is free
                if (_AVTAR.slotsOnPlayerFront[0].transform.childCount == 0)
                    slot = 0;
            }
            if (heldItem.category == Item.Category.Eyes)
            {
                if (_AVTAR.slotsOnPlayerFront[1].transform.childCount == 0)
                    slot = 1;
            }
            if (heldItem.category == Item.Category.Mouth)
            {
                if (_AVTAR.slotsOnPlayerFront[2].transform.childCount == 0)
                    slot = 2;
            }
        }
        else if (heldItem.segment == Item.Segment.Torso)
        {
            if (_AVTAR.slotsOnPlayerFront[3].transform.childCount == 0)
            {
                slot = 3;
            }
            else if (_AVTAR.slotsOnPlayerFront[4].transform.childCount == 0)
            {
                slot = 4;
            }

        }
        else if (heldItem.segment == Item.Segment.Legs)
        {
            if (_AVTAR.slotsOnPlayerFront[5].transform.childCount == 0)
            {
                slot = 5;

            }

        }

        print(heldItem.itemName + " is on slot " + slot);

        EquipImage(slot);

        //check category

        //check if slots in cateogry are free

        //if free then add it


    }

    /// <summary>
    /// Equips item to player inventory and resets cursor to default
    /// </summary>
    /// <param name="_slot"></param>
    public void EquipImage(int _slot)
    {
        bool flip = false;

        if (_slot == 4) flip = true;

        //print("Creating item for slot " + _slot);

        //print("in EquipImage ID is " + heldItem.ID);


        var itemExsists = false;
        foreach (var item in _PC.playerInventory)
        {
            if (item == heldItem) itemExsists = true;
        }

        if (itemExsists == false) _ISitemD.AddItemToInventory(heldItem);



        var itemLeftSide = Instantiate(heldItem.avtarPrefabLeft, _AVTAR.slotsOnPlayerLeft[_slot].transform);
        var itemRightSide = Instantiate(heldItem.avtarPrefabRight, _AVTAR.slotsOnPlayerRight[_slot].transform);
        _PC.itemsAnimLeftSide.Add(itemLeftSide.GetComponentInChildren<Animator>());
        _PC.itemsAnimRightSide.Add(itemRightSide.GetComponentInChildren<Animator>());

        //if torso piece make sure its correct

        if (heldItem.segment == Item.Segment.Torso)
        {
            if (_slot == 4) //  RIGHT
            {
                //FRONT
                var itemFront = Instantiate(heldItem.avatarPrefabFrontRight, _AVTAR.slotsOnPlayerFront[_slot].transform);
                _PC.itemsAnimForward.Add(itemFront.GetComponentInChildren<Animator>());

                //BACKL
                var itemBackSide = Instantiate(heldItem.avtarPrefabBackLeft, _AVTAR.slotsOnPlayerBack[_slot].transform);
                _PC.itemsAnimBack.Add(itemBackSide.GetComponentInChildren<Animator>());

            }
            if (_slot == 3) // LEFT
            {
                //FRONT
                var itemFront = Instantiate(heldItem.avatarPrefabFrontLeft, _AVTAR.slotsOnPlayerFront[_slot].transform);
                _PC.itemsAnimForward.Add(itemFront.GetComponentInChildren<Animator>());


                var itemBackSide = Instantiate(heldItem.avtarPrefabBackRight, _AVTAR.slotsOnPlayerBack[_slot].transform);
                _PC.itemsAnimBack.Add(itemBackSide.GetComponentInChildren<Animator>());

                //BACKK

            }

        }
        else
        {
            //default left
            var itemFront = Instantiate(heldItem.avatarPrefabFrontLeft, _AVTAR.slotsOnPlayerFront[_slot].transform);
            _PC.itemsAnimForward.Add(itemFront.GetComponentInChildren<Animator>());

            var itemBackSide = Instantiate(heldItem.avtarPrefabBackLeft, _AVTAR.slotsOnPlayerBack[_slot].transform);
            _PC.itemsAnimBack.Add(itemBackSide.GetComponentInChildren<Animator>());
        }

        if (heldItem.segment == Item.Segment.Legs) _PC.UpdateLegAnimators();


        CheckHeight();

        // (flip) itemFront.transform.localScale = new Vector3(-itemFront.transform.rotation.x, itemFront.transform.rotation.y, itemFront.transform.rotation.z);

        //FOR ALL OTHER ITEMS

        cursor.sprite = defaultCursor;
        heldItem = null;
        isHoldingItem = false;
        //rotate image

        //_PC.CloseSlots();
    }


    public void CheckHeight()
    {
        //find leg item
        for (int i = 0; i < _PC.playerInventory.Count; i++)
        {
            if (_PC.playerInventory[i].segment == Item.Segment.Legs)
            {
                //6 tripod legs, 10 hoover
                if (_PC.playerInventory[i].ID == 10 || _PC.playerInventory[i].ID == 6)
                {
                    print("Go taller");
                    playerAvatar.transform.position = new Vector3(playerAvatar.transform.position.x, tall.y, playerAvatar.transform.position.z);
                }
                else
                {
                    playerAvatar.transform.position = new Vector3(playerAvatar.transform.position.x, standard.y, playerAvatar.transform.position.z);
                }
            }
        }


        //check if item is mean to be tall

        //if yes raise height

        //if no, keep standard

    }

    /// <summary>
    /// Checks whether held item can be placed on slot that is hovered over
    /// </summary>
    /// <param name="_slot"></param>
    //public void CheckSlotHover(int _slot)
    //{

    //    if (cursor.sprite != defaultCursor && cursor.sprite != cursorClick)
    //    {
    //        print(heldItem.itemName);

    //        if (_AVTAR.slotsOnPlayer[_slot].transform.childCount == 0) //check if child object is there
    //        {
    //            //check right segment if slot from 1 to 2 the head etc. 

    //            if (_AVTAR.slotsOnPlayer[_slot].name.Contains(heldItem.segment.ToString()))
    //            {
    //                canEquip = true;
    //            }
    //            else
    //            {
    //                canEquip = false;
    //                //_AVTAR.slotsOnCanvas[_slot].GetComponent<Image>().color = Color.red;
    //            }
    //        }
    //        else
    //        {
    //            canEquip = false;
    //            //_AVTAR.slotsOnCanvas[_slot].GetComponent<Image>().color = Color.red;
    //        }
    //    }
    //}

    //public void DropHeldItem()
    //{

    //    print("Drop held item");
    //    //change cursor
    //    cursor.sprite = defaultCursor;

    //    //create item on player
    //    var item = Instantiate(_IG.itemTemp, GameObject.Find("Player").transform.position, Quaternion.identity);
    //    _ISitemD.inSceneItemDataBase.Add(heldItem);

    //    item.GetComponent<ItemIdentifier>().id = heldItem.inSceneID;
    //    var id = item.GetComponent<ItemIdentifier>().id;

    //    print("item dropping id = " + id);
    //    print(_ISitemD.inSceneItemDataBase[id].icon.name);
    //    //ERROR
    //    item.GetComponentInChildren<SpriteRenderer>().sprite = _ISitemD.inSceneItemDataBase[id].icon;

    //    //add to scene array
    //    int index = _ISitemD.inSceneItemDataBase.Count - 1;
    //    _ISitemD.inSceneItemDataBase[index].inSceneID = index;

    //    heldItem = null;
    //    isHoldingItem = false;
    //}

    /// <summary>
    /// Changes colour of slots when mouse exits hover
    /// </summary>
    /// <param name="_slot"></param>
    //public void CheckSlotHoverExit(int _slot)
    //{
    //    //_AVTAR.slotsOnCanvas[_slot].GetComponent<Image>().color = Color.yellow;
    //}

    #endregion


    public List<Item> SearchForItemMatch(Item _hoverItem)
    {
        List<Item> itemMatchInPlayerInven = new();

        print("Hover item is " + _hoverItem.itemName);

        foreach (var item in _PC.playerInventory)
        {
            if(item.segment == _hoverItem.segment)
            {
                itemMatchInPlayerInven.Add(item);


                //print("ITS A MATCH");
                //statComp1.SetActive(true);
                //anim1.SetTrigger("Open");
                //arrowComp.SetActive(true);
                //UpdateItemPopUpComp1(item);
            }

            //if (item.projectile == true)
            //{
            //    attackPill.GetComponent<SpriteRenderer>().color = new Color(220, 255, 0);
            //}
            //else
            //{
            //    attackPill.GetComponent<SpriteRenderer>().color = new Color(0, 154, 255);

            //}

            var icon = item.icon;




        }

        foreach (var item in itemMatchInPlayerInven)
        {
            print(item.itemName);
        }

        /*
         * Is it projectile?
         * if(item.projectile == true) { IS PROJECTILE}
         * 
         * Get Icon
         * var icon = item.icon
         * 
         * Get Attack Type
         * var attackType = icon.attackType
         * 
         * Get Segment Type
         * var segment = icon.segment
         * 
         * Get Range
         * 
         * item.range9
         */



        return itemMatchInPlayerInven;
    }

    #region Indicators
    
    //public void TopSegmentIndicator()
    //{
    //    topEye.SetActive(true);
    //    topEye.GetComponent<SpriteRenderer>().color = Color.yellow;
    //    middleEye.SetActive(false);
    //    bottomEye.SetActive(false);
    //}

    //public void MiddleSegmentIndicator()
    //{
    //    middleEye.SetActive(true);
    //    middleEye.GetComponent<SpriteRenderer>().color = Color.red;
    //    topEye.SetActive(false);
    //    bottomEye.SetActive(false);

    //}

    //public void BottomSegmentIndicator()
    //{
    //    bottomEye.SetActive(true);
    //    bottomEye.GetComponent<SpriteRenderer>().color = Color.red;
    //    topEye.SetActive(false);
    //    middleEye.SetActive(false);
    //}

    //public void AttackPillChange(int num)
    //{
    //    attackPill.SetActive(true);
    //    if (num == 1)
    //    {
    //        attackPillText.text = "Melee";
    //        //attackIcon.SetActive(true); change the icon
    //        attackPill.GetComponent<SpriteRenderer>().color = Color.red;
    //    }
    //    if (num == 2) 
    //    {
    //        attackPillText.text = "Range";
    //        //attackIcon.SetActive(true); change the icon
    //        attackPill.GetComponent<SpriteRenderer>().color = Color.red;
    //    }
    //}

    //public void RangePillChange(int num)
    //{
    //    rangePill.SetActive(true);
    //    attackPillText.text = num.ToString();
    //}

    //public void ChangeItemIcon()
    //{
    //    //Chnage the item icons
    //}

    //public void ChangeItemType()
    //{
    //    //Chnage the type icons
    //}

    #endregion
}