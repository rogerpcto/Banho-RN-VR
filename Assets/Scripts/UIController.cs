using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    [SerializeField]
    private Bandeja _bandeja;
    [SerializeField]
    private GameObject _itens;

    [SerializeField]
    private TextMeshProUGUI _display;
    [SerializeField]
    private TextMeshProUGUI _buttonText;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Button _previousButton;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Button _RestartLevel;
    [SerializeField]
    private Image _imagem;

    private int _indexMensagens = 0;
    private Mensagem[] _mensagens;

    private readonly string[] _instrucoes =
    {
        "Bem-vindo ao nosso jogo de realidade virtual que ensina como dar banho em um rec�m-nascido! Voc� pode ler as regras controlando com os bot�es abaixo ou come�ar o jogo agora apertando em <b>Jogar</b>.",
        "1 - <b>Itens Dispon�veis</b>:\n Voc� ver� v�rios itens ao seu redor, todos ao alcance. Estes itens podem ser �teis ou n�o para dar banho em um rec�m-nascido.\n" +
        "2 - <b>Bandeja Azul</b>:\n Em sua frente, voc� encontrar� uma bandeja azul. Esta � a �rea onde voc� deve colocar os itens que acredita serem necess�rios para dar banho no beb�.",

        "3 - <b>Escolha dos Itens</b>:\n Use os controladores VR para pegar os itens. Coloque na bandeja azul aqueles que voc� acha que s�o apropriados para o banho do rec�m-nascido.\n" +
        "4 - <b>Cores</b>:\n" +
            "<color=green>Verde</color>: Se voc� escolheu um item correto, ele ser� destacado em verde.\n" +
            "<color=red>Vermelho</color>: Se voc� escolheu um item incorreto, ele ser� destacado em vermelho.",

        "5 - <b>Finalizar Escolha</b>:\n Quando voc� achar que j� escolheu todos os itens necess�rios, clique no bot�o <b>Finalizar</b> localizado nessa caixa de texto.\n" +
        "6 - <b>Revis�o</b>:\n Ap�s clicar em <b>Finalizar</b>, voc� receber� uma pontua��o baseada em suas escolhas, com dicas sobre os itens corretos e incorretos.\n" +
        "Divirta-se e aprenda a dar banho em um rec�m-nascido de maneira segura e eficaz!",

    };

    private void Start()
    {
        _button.onClick.AddListener(MostrarInstrucoes);
        _nextButton.onClick.AddListener(Proximo);
        _previousButton.onClick.AddListener(Anterior);
        _RestartLevel.onClick.AddListener(FinalizarJogo);

        Item[] itensEmCena = _itens.GetComponentsInChildren<Item>();
        _bandeja.Iniciar(itensEmCena);
    }

    private void MostrarInstrucoes()
    {
        _mensagens = _instrucoes.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ComecarJogo);
        MostrarMensagens();
    }

    private void MostrarMensagens()
    {
        SetMensagem(_mensagens[0]);
        if (_mensagens.Length > 1)
            _nextButton.gameObject.SetActive(true);
    }

    private void Proximo()
    {
        _indexMensagens++;
        SetMensagem(_mensagens[_indexMensagens]);
        if (_indexMensagens == 1)
            _previousButton.gameObject.SetActive(true);
        if (_indexMensagens == _mensagens.Length - 1)
            _nextButton.gameObject.SetActive(false);
    }

    private void Anterior()
    {
        _indexMensagens--;
        SetMensagem(_mensagens[_indexMensagens]);
        if (_indexMensagens == 0)
            _previousButton.gameObject.SetActive(false);
        if (_indexMensagens == _mensagens.Length - 2)
            _nextButton.gameObject.SetActive(true);
    }

    private void ComecarJogo()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text = "Coloque os itens na bandeja!";
        _itens.SetActive(true);

        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(FinalizarJogo);
        _button.gameObject.SetActive(false);
    }

    private void FinalizarJogo()
    {
        _RestartLevel.gameObject.SetActive(true);
        _RestartLevel.onClick.RemoveAllListeners();
        _RestartLevel.onClick.AddListener(ReiniciarJogo);
        Mensagem[] checklist = _bandeja.Checklist();
        _indexMensagens = 0;
        _mensagens = checklist;
        _buttonText.text = "PR�XIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ToggleFase2);
        MostrarMensagens();
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleFase2()
    {
        _itens.SetActive(false);
        while (_bandeja._itens.Count > 0)
        {
            Item item = _bandeja._itens[0];
            _bandeja._itens.RemoveAt(0);
            Destroy(item.gameObject);
        }
        Destroy(_bandeja.gameObject);
        string sceneName = "Segunda fase";
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void SetMensagem(Mensagem mensagem)
    {
        _display.text = mensagem.mensagem;
        if (mensagem.sprite == null)
            _imagem.enabled = false;
        else
        {
            _imagem.enabled = true;
            _imagem.sprite = mensagem.sprite;
        }
    }

    public void LigarBotao()
    {
        _button.gameObject.SetActive(true);
    }
}

public struct Mensagem
{
    public string mensagem;
    public Sprite sprite;
}
