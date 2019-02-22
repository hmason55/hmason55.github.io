using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferUI : MonoBehaviour {

    public enum TransactionType {
        Buy,
        Give,
        Sell,
        Take
    }

    public TransactionType transactionType = TransactionType.Give;

    [SerializeField] Image _backImage;
    [SerializeField] Text _titleText;

    [SerializeField] Image _giveItemImage;
    [SerializeField] Image _giveArrowImage;
    [SerializeField] Text _giveItemQuantity;
    

    [SerializeField] Image _takeItemImage;
    [SerializeField] Image _takeArrowImage;
    [SerializeField] Text _takeItemQuantity;

    [SerializeField] Sprite _coinSprite;
    
    public void Preview(BagBehaviour.Actions action, BaseItem item) {
        ShowUI();

        switch(action) {
            case BagBehaviour.Actions.Give:
                transactionType = TransactionType.Give;
            break;

            case BagBehaviour.Actions.Sell:
                transactionType = TransactionType.Sell;
            break;

            default:
            return;
        }

        UpdateTooltip(item);
    }

    public void Preview(ContainerBehaviour.Actions action, BaseItem item) {
        ShowUI();

        switch(action) {
            case ContainerBehaviour.Actions.Buy:
                transactionType = TransactionType.Buy;
            break;

            case ContainerBehaviour.Actions.Take:
                transactionType = TransactionType.Take;
            break;

            default:
            return;
        }

        UpdateTooltip(item);
    }

    void UpdateTooltip(BaseItem item) {
        _titleText.text = transactionType.ToString();

        switch(transactionType) {
            case TransactionType.Buy:
                ShowGiveUI();
                _giveItemImage.sprite = _coinSprite;
                _giveItemQuantity.text = item.value.ToString();

                ShowTakeUI();
                _takeItemImage.sprite = item.sprite;
                if(item.quantity > 1) {
                    _takeItemQuantity.enabled = true;
                    _takeItemQuantity.text = item.quantity.ToString();
                } else {
                    _takeItemQuantity.enabled = false;
                }
            break;

            case TransactionType.Give:
                ShowGiveUI();
                _giveItemImage.sprite = item.sprite;
                if(item.quantity > 1) {
                    _giveItemQuantity.enabled = true;
                    _giveItemQuantity.text = item.quantity.ToString();
                } else {
                    _giveItemQuantity.enabled = false;
                }

                HideTakeUI();
            break;

            case TransactionType.Sell:
                ShowGiveUI();
                _giveItemImage.sprite = item.sprite;
                if(item.quantity > 1) {
                    _giveItemQuantity.enabled = true;
                    _giveItemQuantity.text = item.quantity.ToString();
                } else {
                    _giveItemQuantity.enabled = false;
                }

                ShowTakeUI();
                _takeItemImage.sprite = _coinSprite;
                _takeItemQuantity.text = item.value.ToString();
            break;

            case TransactionType.Take:
                HideGiveUI();

                ShowTakeUI();
                _takeItemImage.sprite = item.sprite;
                if(item.quantity > 1) {
                    _takeItemQuantity.enabled = true;
                    _takeItemQuantity.text = item.quantity.ToString();
                } else {
                    _takeItemQuantity.enabled = false;
                }
            break;
        }
    }

    void ShowGiveUI() {
        _giveItemImage.enabled = true;
        _giveArrowImage.enabled = true;
        _giveItemQuantity.enabled = true;
    }

    void HideGiveUI() {
        _giveItemImage.enabled = false;
        _giveArrowImage.enabled = false;
        _giveItemQuantity.enabled = false;
    }

    void ShowTakeUI() {
        _takeItemImage.enabled = true;
        _takeArrowImage.enabled = true;
        _takeItemQuantity.enabled = true;
    }

    void HideTakeUI() {
        _takeItemImage.enabled = false;
        _takeArrowImage.enabled = false;
        _takeItemQuantity.enabled = false;
    }


    public void ShowUI() {
        _backImage.enabled = true;
        _titleText.enabled = true;

        switch(transactionType) {

            case TransactionType.Give:
                ShowGiveUI();
            break;

            case TransactionType.Take:
                ShowTakeUI();
            break;

            case TransactionType.Buy:
            case TransactionType.Sell:
                ShowGiveUI();
                ShowTakeUI();
            break;
        }
    }
    
    public void HideUI() {
        _backImage.enabled = false;
        _titleText.enabled = false;

        _giveItemImage.enabled = false;
        _giveArrowImage.enabled = false;
        _giveItemQuantity.enabled = false;

        _takeItemImage.enabled = false;
        _takeArrowImage.enabled = false;
        _takeItemQuantity.enabled = false;
    }
}
