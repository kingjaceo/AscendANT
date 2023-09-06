using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChangingDropdown : TMP_Dropdown 
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < options.Count; i++)
        {            
            base.OnPointerClick(eventData);

            Image image = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i+1).GetChild(0).GetComponent<Image>();

            image.color = PaletteManager.Colors[i*3];
        }
    }

    protected override void Start()
    {
        base.Start();
        onValueChanged.AddListener((v) => { UpdateDropdownColor();});
        GetComponent<Image>().color = PaletteManager.Colors[0];
    }

    private void UpdateDropdownColor()
    {
        GetComponent<Image>().color = PaletteManager.Colors[value];
    }
    // protected override GameObject CreateDropdownList(GameObject template) 
    // {
    //     GameObject dropdownList = base.CreateDropdownList(template);
        
    //     dropdownList.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.red;
        
    //     return dropdownList;
    // }
}
