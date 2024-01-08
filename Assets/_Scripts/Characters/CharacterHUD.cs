using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TestCharactersMovement.CharactersSystem
{
    public class CharacterHUD : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {

        [Header("General")]
        [SerializeField] private Character character;
        [SerializeField] private Image characterImage;
        [SerializeField] private bool isActive = false;

        [Header("Colors")]
        [SerializeField] private Color selectColor;
        [SerializeField] private Color deselectColor;
        [SerializeField] private Color hoverColor;

        public delegate void CharacterDelegate(Character character);
        public static CharacterDelegate OnCharacterSelected;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!isActive)
            {
                characterImage.color = hoverColor;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive)
            {
                SetCharacterSelected();

                OnCharacterSelected?.Invoke(character);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isActive)
            {
                characterImage.color = deselectColor;
            }
        }

        public void SetCharacterSelected()
        {
            characterImage.color = selectColor;
            isActive = true;
        }

        public void SetCharacterDeselected()
        {
            characterImage.color = deselectColor;
            isActive = false;
        }


    }
}


