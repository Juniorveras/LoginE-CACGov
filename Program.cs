using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Seja bem-vindo faça seu login no E-CAC.");
        Console.WriteLine("Login por apontamento do Certificado A1.");

        string pathCertificado = ObterCaminhoCertificado();
        string passwordCertificado = ObterSenhaCertificado();

        await EnviarCertificado(pathCertificado, passwordCertificado);
    }

    static string ObterCaminhoCertificado()
    {
        string? certPath;
        do
        {
            Console.WriteLine("Por favor, insira o caminho do arquivo do certificado:");
            certPath = Console.ReadLine();

            if (string.IsNullOrEmpty(certPath) || !System.IO.File.Exists(certPath))
            {
                Console.WriteLine("Caminho do certificado inválido. Verifique o diretorio.");
            }

        } while (string.IsNullOrEmpty(certPath) || !System.IO.File.Exists(certPath));

        return certPath;
    }

    static string ObterSenhaCertificado()
    {
        SecureString securePassword = new SecureString();
        Console.WriteLine("Insirir a senha do certificado:");

        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace && securePassword.Length > 0)
            {
                securePassword.RemoveAt(securePassword.Length - 1);
                Console.Write("\b \b");
            }
            else
            {
                securePassword.AppendChar(key.KeyChar);
                Console.Write("*");
            }
        }

        return ConvertSecureStringToString(securePassword);
    }

    static string ConvertSecureStringToString(SecureString secureString)
    {
        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
            return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
        }
        finally
        {
            if (ptr != IntPtr.Zero)
                System.Runtime.InteropServices.Marshal.FreeBSTR(ptr);
        }
    }

    static async Task EnviarCertificado(string certPath, string pass)
    {
        try
        {
            X509Certificate2 certificado = new X509Certificate2(certPath, pass);
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificado);

            using (var client = new HttpClient(handler))
            {
                string url = "https://cav.receita.fazenda.gov.br/autenticacao/login";

                var parametros = new MultipartFormDataContent
                {
                    { new StringContent(certPath), "pkcs12_cert" },
                    { new StringContent(pass), "pkcs12_pass" }
                };

                HttpResponseMessage response = await client.PostAsync(url, parametros);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("\nLogin realizado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"\nFalha ao realizar login. Código: {response.StatusCode}");
                }
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"\nErro na requisição: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nErro ao enviar certificado: {e.Message}");
        }
    }
}
