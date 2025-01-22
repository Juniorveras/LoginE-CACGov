## Sistema de Login via Certificado Digital do tipo A1.

### Sistema tipo CONSOLE simples e funcional.

Sistema para teste de execuções tipo A1.

Execução.
-- Clone o repositorio.
-- Build.
-- Execute.

Detalhamento do Código

Método ObterCaminhoCertificado:

Solicita ao usuário que insira o caminho do arquivo do certificado.
Verifica se o caminho é válido (ou seja, se não está vazio e se o arquivo existe).
Retorna o caminho do certificado.

Método ObterSenhaCertificado:

Solicita ao usuário que insira a senha do certificado.
Lê a entrada do usuário de forma segura, ocultando os caracteres digitados (substituindo-os por asteriscos).
Converte a senha de SecureString para uma string normal (apenas para o envio) e a retorna.

Método ConvertSecureStringToString:

Converte um objeto SecureString em uma string normal.
Utiliza ponteiros para manipular a memória de forma segura, 
garantindo que a string resultante seja liberada corretamente após o uso.

Método EnviarCertificado:

Tenta criar um objeto X509Certificate2 usando o caminho e a senha fornecidos.
Cria um HttpClientHandler e adiciona o certificado ao manipulador.
Cria um HttpClient usando o manipulador configurado.
Prepara uma solicitação HTTP POST para o URL do ECAC, incluindo o certificado e a senha como parâmetros.
Envia a solicitação e verifica se a resposta indica sucesso ou falha.
Exibe uma mensagem apropriada com base no resultado da solicitação.

Tratamento de Exceções:

O método EnviarCertificado possui blocos try-catch para capturar e tratar exceções que podem ocorrer durante a execução, 
como problemas de rede ou erros ao manipular o certificado.

Resumo
Em resumo, o código é um aplicativo de console que permite ao usuário fazer login em um sistema usando um certificado digital. 
Ele coleta o caminho do certificado e a senha do usuário, envia essas informações para um servidor remoto e exibe o resultado do login. 
O uso de SecureString para a senha e a validação do caminho do certificado são boas práticas de segurança implementadas no código.
