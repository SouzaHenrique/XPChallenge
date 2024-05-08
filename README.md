- Como Configurar:
  - utilizar arquivo de configuração do projecto 'XPChallenge.WebAPI' para:
    - Sobrescrever valores de configuração 'AppMailSenderOptions' (utilizando no momento mailtrap),
      para configurar funcionalidade de envio diário de emails sobre produtos vencidos

- Como executar :
  - instalar e executar mongodb (https://www.mongodb.com/docs/manual/installation/)
  - executar aplicação

- Como utilizar :
  - Ao executar o projeto uma pagina do Swagger surgirá
  - Utilize POST '/customer' para cadastradar novo cliente, ID do cliente será retornado
  - Utilize POST '/Products' para cadastrar novo produto, ID do produto será retornado
  - Usando ID do cliente, ID do produto e quantidade (valor inteiro) utilize POST '/purchases' para comprar um novo produto
  - Usando ID do cliente, ID do produto e quantidade (valor inteiro) utilize POST '/sales' para vender um novo produto
  - Usando ID do produto utilize GET '/products' para obter extrato do produto
  
