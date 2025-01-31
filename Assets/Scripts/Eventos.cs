using System;

public static class Eventos
{
    private static Action _comecarFase2;
    private static Action _liberarFinalizarFase2;
    private static Action<string> _gerarMensagemFase2;
    private static Action _terminarFase2;
    private static Action _comecarFase3;
    private static Action _terminarFase3;
    private static Action _segurarBebe;
    private static Action _soltarBebe;

    public static void InscreverComecarFase2(Action acao) => _comecarFase2 += acao;
    public static void InscreverLiberarFinalizarFase2(Action acao) => _liberarFinalizarFase2 += acao;
    public static void InscreverGerarMensagemFase2(Action<string> acao) => _gerarMensagemFase2 += acao;
    public static void InscreverTerminarFase2(Action acao) => _terminarFase2 += acao;
    public static void InscreverComecarFase3(Action acao) => _comecarFase3 += acao;
    public static void InscreverTerminarFase3(Action acao) => _terminarFase3 += acao;
    public static void InscreverSegurarBebe(Action acao) => _segurarBebe += acao;
    public static void InscreverSoltarBebe(Action acao) => _soltarBebe += acao;

    public static void InvocarComecarFase2() => _comecarFase2?.Invoke();
    public static void InvocarLiberarFinalizarFase2() => _liberarFinalizarFase2?.Invoke();
    public static void InvocarGerarMensagemFase2(string obj) => _gerarMensagemFase2?.Invoke(obj);
    public static void InvocarTerminarFase2() => _terminarFase2?.Invoke();
    public static void InvocarComecarFase3() => _comecarFase3?.Invoke();
    public static void InvocarTerminarFase3() => _terminarFase3?.Invoke();
    public static void InvocarSegurarBebe() => _segurarBebe?.Invoke();
    public static void InvocarSoltarBebe() => _soltarBebe?.Invoke();

    public static void LimparEventos()
    {
        _comecarFase2 = null;
        _liberarFinalizarFase2 = null;
        _gerarMensagemFase2 = null;
        _terminarFase2 = null;
        _comecarFase3 = null;
        _terminarFase3 = null;
        _segurarBebe = null;
        _soltarBebe = null;
    }
}
