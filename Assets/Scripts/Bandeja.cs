using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bandeja : MonoBehaviour
{
    private readonly Stack<Item> _itens = new();

    [SerializeField]
    private TextMeshProUGUI _display;

    public void CadastrarItem(Item item)
    {
        if (item.Correto)
            _itens.Push(item);

        _display.text = item.GetInformacao();
    }

}
