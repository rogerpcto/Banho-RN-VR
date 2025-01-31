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
    private Button _RestartLevelMenuButton;
    [SerializeField]
    private Button _RestartGameMenuButton;
    [SerializeField]
    private MenuExpandButton _MenuExpandButton;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Image _imagem;

    private int _indexMensagens = 0;
    private Mensagem[] _mensagens;

    #region Instru��es

    private readonly string[] _instrucoes1 =
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

    private readonly string[] _instrucoes2 =
    {
        "Para come�ar o banho no beb�, � importante que a pessoa respons�vel retire an�is, pulseiras ou acess�rios, para evitar arranh�es. Al�m disso, a pessoa deve estar com unhas curtas" +
            " e com cabelo preso, caso esse seja longo.",

        "A �gua para banho deve estar na temperatura do corpo, cerca de 37�C, e � importante que o banho n�o ultrapasse a faixa de 5 a 10 minutos," +
            " porque os beb�s perdem calor com muita facilidade.",

        "Tamb�m � importante que o local do banho esteja com janelas e portas fechadas, para evitar correntes de ar no beb�, para que ele n�o sinta frio. O banho do beb� come�a pelo rosto do beb�.",

        "Evite passar sabonete no rosto do beb�, j� que pode causar irrita��o nos seus olhos. Nesse momento do banho, o beb� deve estar enrolado numa toalha para evitar perda de calor." +
            "Aqui no nosso jogo, voc� limpar� o rosto do beb� utilizando um algod�o, que se encontra na bandeja.",

        "Voc� pode pegar o beb� sempre apoiando sua cabe�a para evitar solavancos no seu pesco�o. O algod�o pode ser molhado na banheira � direita e voc� precisa pass�-lo na testa, nas duas " +
            "bochechas do beb� e no seu queixo.",

        "Essas �reas est�o manchadas, simulando uma sujeira, basta esfregar o algod�o molhado para retirar a sujeira. Caso voc� tenha �xito, o beb� ir�" +
            " responder animadamente �s suas a��es! Para come�ar, basta clicar em <b>Jogar</b>.",
    };

    #endregion

    private void Start()
    {
        _button.onClick.AddListener(MostrarInstrucoes);
        _nextButton.onClick.AddListener(Proximo);
        _previousButton.onClick.AddListener(Anterior);
        _RestartLevelMenuButton.onClick.AddListener(ReiniciarJogo);

        Item[] itensEmCena = _itens.GetComponentsInChildren<Item>();
        _bandeja.Iniciar(itensEmCena);
    }

    private void MostrarInstrucoes()
    {
        _mensagens = _instrucoes1.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ComecarFase1);
        MostrarMensagens();
    }

    private void MostrarMensagens()
    {
        _indexMensagens = 0;
        SetMensagem(_mensagens[_indexMensagens]);
        _previousButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(_mensagens.Length > 1);
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

    private void ComecarFase1()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text = "Coloque os itens na bandeja!";
        _itens.SetActive(true);

        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(FinalizarFase1);
        _button.gameObject.SetActive(false);
    }

    private void FinalizarFase1()
    {
        Mensagem[] checklist = _bandeja.Checklist();
        _indexMensagens = 0;
        _mensagens = checklist;
        _buttonText.text = "PR�XIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(IniciarFase2);
        MostrarMensagens();
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(0);
    }

    private void MostrarInstrucoes2()
    {
        _mensagens = _instrucoes2.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.gameObject.SetActive(true);
        _button.onClick.RemoveAllListeners();

        Eventos.InscreverComecarFase2(ComecarFase2);
        Eventos.InscreverSegurarBebe(SegurandoBebeFase2);
        Eventos.InscreverSoltarBebe(ComecarFase2);
        Eventos.InscreverLiberarFinalizarFase2(LigarBotaoFinalizarFase2);
        Eventos.InscreverGerarMensagemFase2(FinalizarFase2);

        _button.onClick.AddListener(Eventos.InvocarComecarFase2);
        MostrarMensagens();
    }

    private void ComecarFase2()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text =
            "Comece pegando o beb�! Segure o beb� de barriga para cima, apoiando bem a cabe�a e o pesco�o. Mantenha-o pr�ximo ao peito," +
            " com a cabe�a mais alta, e evite movimentos bruscos. Conforto e seguran�a s�o essenciais! ";

        _button.gameObject.SetActive(false);
    }

    private void SegurandoBebeFase2()
    {
        _display.text = "Pegue um algod�o do pote de algod�o na bandeija � esquerda e umede�a-o na banheira � sua direita." +
            " Depois passe no rosto do beb� at� que a mancha de sujeira seja removida.";
    }

    private void LigarBotaoFinalizarFase2()
    {
        _button.gameObject.SetActive(true);
        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(Eventos.InvocarTerminarFase2);
    }

    private void FinalizarFase2(string mensagem)
    {
        _display.text = mensagem;
        _buttonText.text = "PR�XIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(IniciarFase3);
    }

    private void IniciarFase2()
    {
        string sceneName = "Segunda fase";
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName).completed += (operation) =>
            {
                _MenuExpandButton.ToggleMenu();
                PrepararFase2();
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            };
        }
        else
        {
            PrepararFase2();
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    private void PrepararFase2()
    {
        Eventos.LimparEventos();

        if (_itens != null)
            Destroy(_itens);

        if (_bandeja != null)
            _bandeja.Destruir();

        _MenuExpandButton.AddButton(_RestartGameMenuButton.gameObject.transform);

        _RestartLevelMenuButton.onClick.RemoveAllListeners();
        _RestartLevelMenuButton.onClick.AddListener(IniciarFase2);
        _imagem.enabled = false;

        MostrarInstrucoes2();
    }

    private void IniciarFase3()
    {
        throw new System.NotImplementedException();
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
