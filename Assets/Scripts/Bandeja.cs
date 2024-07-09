using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bandeja : MonoBehaviour
{
    private readonly List<Item> _itens = new();

    [SerializeField]
    private TextMeshProUGUI _display;

    public void CadastrarItem(Item item)
    {
        _itens.Add(item);
        _display.text = item.GetInformacao();
    }

    public void RetirarItem(Item item)
    {
        _itens.Remove(item);
        if (_itens.Count > 0)
            _display.text = _itens[^1].GetInformacao();
    }
}
