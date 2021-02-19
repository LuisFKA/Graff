# Projeto Graff Avaliação técnica

O projeto consiste em um web service para cadastro de itens de um leilão, onde pessoas podem ser cadastradas e fazerem lances sobre os produtos do leilão. Os lances podem ser listados e filtrados por produto e por pessoa. Menores de 18 anos não podem fazer lances e um lance é somente aceito se for maior do que o lance atual do produto.


# Pré-requisitos
Instale o Visual Studio 2019 com o pacote de Asp.NET para desenvolvimento Web. O Visual Studio instalará os demais necessários pacotes para a execução do projeto, quando ele for aberto.

# Instalação e Execução

O projeto foi desenvolvido utilizando o Visual Studio 2019 com a instalação do pacote de Asp.NET para desenvolvimento Web. A solução do projeto pode ser aberta com tal ferramenta e executada a partir dela. Foi utizilado a porta 44325. 

Ao entrar no projeto com o Visual Studio, clique em IIS Express, que executa o projeto. 

Isso te levará ao browser. Ao clicar no menu de Produtos, ou Lances, ou Pessoas, um erro aparecerá. Clique no botão de "Apply Migrations" e então recarregue a página. O projeto funcionará como devido, então.

# Notas
O arquivo Graff.sql contém as queries de criação das tabelas usadas no projeto, assim como a query que pega todos lances dos produtos, contendo as informações do lance, do produto relacionado, e a pessoa que fez o lance. A query mencionada se dá da seguinte forma: 

SELECT  l.Id, pr.Nome, l.Valor, pe.Nome  FROM (dbo.Lance l INNER JOIN  dbo.Pessoa pe ON  l.PessoaId  =  pe.Id) INNER JOIN  dbo.Produto pr ON  l.ProdutoId  =  pr.Id;
