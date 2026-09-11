namespace PROJETOCNP.Services;

public static class CpfFormatter
{
    public static string Formatar(string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
            return string.Empty;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            throw new ArgumentException("CPF deve conter exatamente 11 dígitos.");

        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }

    public static bool TryFormatar(string cpf, out string cpfFormatado)
    {
        cpfFormatado = string.Empty;

        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length != 11)
            return false;

        cpfFormatado = Convert.ToUInt64(digits).ToString(@"000\.000\.000\-00");
        return true;
    }

    public static string Normalizar(string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
            return string.Empty;

        return new string(cpf.Where(char.IsDigit).ToArray());
    }
}