namespace QuantoEuCobro.Services;

public static class TemplateEngine
{
    public static string Aplicar(string layoutJson, object dados)
    {
        if (string.IsNullOrWhiteSpace(layoutJson))
            return "{}";

        // por enquanto: retorna bruto
        // futuramente: interpolação de campos
        return layoutJson;
    }
}